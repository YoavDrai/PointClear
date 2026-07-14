using UnityEngine;
using PointClear.Combat;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.3: the "marked" state Detonation Field applies to an enemy,
    /// added at runtime to the enemy GameObject. If the enemy dies before the
    /// mark expires, it detonates — an area-of-effect burst at its position.
    /// Otherwise the mark expires silently and removes itself.
    ///
    /// All mark logic lives here, subscribing to the enemy's existing
    /// Health.Died event — EnemyAI and Health are not modified. Explosion
    /// chains (an explosion killing another marked enemy, which detonates in
    /// turn) are intended and always terminate: Health.TakeDamage ignores
    /// damage once IsDead, so Died fires at most once per enemy.
    ///
    /// Sprint 2.5 (review feedback): added minimal prototype visuals so the
    /// interaction is legible in normal play — a small marker orb above a
    /// marked enemy, and a short-lived burst sphere sized to the detonation
    /// radius at the death position. Both are collider-less primitives that
    /// clean themselves up (the marker with the mark, the burst on a timer),
    /// so nothing accumulates. Because both the Detonation Field cast and the
    /// Volatile Fracture shards create a DetonationMark and call Apply, they
    /// share these visuals automatically. Not final art — no VFX system, no
    /// shake, no audio.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class DetonationMark : MonoBehaviour
    {
        private static readonly Collider[] ExplosionBuffer = new Collider[64];
        private static readonly Color MarkColor = new Color(1f, 0.45f, 0.05f);      // hot orange = "primed"
        private static readonly Color BurstColor = new Color(1f, 0.85f, 0.2f);      // bright yellow burst
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int LegacyColorId = Shader.PropertyToID("_Color");

        private static Material sharedVfxMaterial;

        private Health health;
        private float expiryTime;
        private float explosionRadius;
        private float explosionDamage;
        private bool subscribed;
        private bool detonated;
        private GameObject markIndicator;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        /// <summary>Marks (or re-marks, refreshing expiry) this enemy.</summary>
        public void Apply(float duration, float radius, float damage)
        {
            expiryTime = Time.time + duration;
            explosionRadius = radius;
            explosionDamage = damage;

            if (!subscribed)
            {
                health.Died += HandleDeath;
                subscribed = true;
            }

            if (markIndicator == null)
            {
                markIndicator = CreateVisual("DetonationMarkIndicator", transform, 0.5f, MarkColor);
                markIndicator.transform.localPosition = new Vector3(0f, 1.4f, 0f);
            }
        }

        private void Update()
        {
            if (Time.time >= expiryTime)
            {
                Unsubscribe();
                Destroy(this);
            }
        }

        private void HandleDeath()
        {
            // Guard against a death arriving in the same frame the mark expired.
            if (Time.time >= expiryTime)
            {
                return;
            }

            Detonate();
        }

        /// <summary>
        /// Sprint 2.9: triggers this mark's detonation immediately — the Detonator
        /// Module's weapon-triggered path — instead of waiting for the enemy's
        /// death. Shares the exact death-detonation path (and its one-shot guard),
        /// then removes the mark so it can never detonate twice: a lethal shot on a
        /// marked enemy detonates once via death and this call becomes a no-op.
        /// Detonation Field still owns marking; this only exposes a trigger entry.
        /// </summary>
        public void DetonateEarly()
        {
            Detonate();

            // Consume the mark after an early trigger: unsubscribe from Health.Died
            // and destroy this component (OnDestroy also clears the marker visual).
            Unsubscribe();
            Destroy(this);
        }

        // Single shared explosion path for both the death and early-trigger routes.
        // The one-shot guard makes the two mutually idempotent.
        private void Detonate()
        {
            if (detonated)
            {
                return;
            }

            detonated = true;
            Explode();
        }

        private void Explode()
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                explosionRadius,
                ExplosionBuffer,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Collider other = ExplosionBuffer[i];
                if (other == null)
                {
                    continue;
                }

                // The dying enemy that triggered this explosion is excluded by
                // target != health; anything already dead is excluded by
                // !IsDead. A chained explosion on a killed marked neighbor
                // fires through its own DetonationMark.HandleDeath, not here.
                // No self-damage / friendly fire from player abilities (PC-017):
                // the player is never hurt by their own explosion, even though the
                // self-centred Detonation Field means marked enemies die nearby.
                Health target = other.GetComponentInParent<Health>();
                if (target != null && target != health && !target.IsDead && !PlayerAbilityDamage.IsPlayer(target))
                {
                    target.TakeDamage(explosionDamage);
                }
            }

            SpawnBurst(transform.position, explosionRadius);
        }

        private void OnDestroy()
        {
            Unsubscribe();

            // The marker is a child of this enemy, so it is normally destroyed
            // with the enemy; destroy it explicitly too so an expired mark on a
            // still-living enemy leaves nothing behind.
            if (markIndicator != null)
            {
                Destroy(markIndicator);
                markIndicator = null;
            }
        }

        private void Unsubscribe()
        {
            if (subscribed)
            {
                health.Died -= HandleDeath;
                subscribed = false;
            }
        }

        /// <summary>Short-lived burst sphere sized to the detonation radius so
        /// the player can read the affected area. Unparented (the enemy is being
        /// destroyed) and auto-removed on a timer.</summary>
        private void SpawnBurst(Vector3 position, float radius)
        {
            GameObject burst = CreateVisual("DetonationBurst", null, radius * 2f, BurstColor);
            burst.transform.position = position;
            Destroy(burst, 0.35f);
        }

        /// <summary>Creates a collider-less, uniformly-scaled sphere primitive
        /// tinted via a MaterialPropertyBlock on a shared material. Collider is
        /// removed so it never interferes with overlaps, raycasts, or enemy
        /// separation.</summary>
        private static GameObject CreateVisual(string name, Transform parent, float diameter, Color color)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = name;

            Collider col = go.GetComponent<Collider>();
            if (col != null)
            {
                Destroy(col);
            }

            if (parent != null)
            {
                go.transform.SetParent(parent, false);
            }
            go.transform.localScale = Vector3.one * diameter;

            Renderer renderer = go.GetComponent<Renderer>();
            renderer.sharedMaterial = GetSharedMaterial();
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor(BaseColorId, color);
            block.SetColor(LegacyColorId, color);
            renderer.SetPropertyBlock(block);

            return go;
        }

        private static Material GetSharedMaterial()
        {
            if (sharedVfxMaterial == null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit");
                if (shader == null)
                {
                    shader = Shader.Find("Sprites/Default");
                }
                sharedVfxMaterial = new Material(shader) { name = "DetonationVfx (runtime shared)" };
            }

            return sharedVfxMaterial;
        }
    }
}
