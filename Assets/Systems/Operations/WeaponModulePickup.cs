using System.Collections.Generic;
using UnityEngine;
using PointClear.Player;

namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.9: the physical Weapon Module drop. A trigger volume the player
    /// walks over to equip the Detonator Module (WeaponModule.Equip). Mirrors
    /// CurrencyPickup's feel (attract radius, collect-once, self-destroy, static
    /// ClearAll for run-end cleanup) but is a distinct, non-currency pickup: at
    /// most one exists per run, it grants a build-changing module rather than a
    /// value, and it is tinted a distinct "power" color so it never reads as gold.
    /// Not a general pickup/equipment framework — one module, one slot.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class WeaponModulePickup : MonoBehaviour
    {
        private static readonly List<WeaponModulePickup> Active = new List<WeaponModulePickup>();

        // Sprint 2.9 review: the module must be unmistakable vs. the gold currency
        // coins it co-drops with — a bright, saturated cyan (distinct hue from
        // matte gold). Real HDR emission was tried and rejected: enabling _EMISSION
        // on a runtime URP/Lit material rendered magenta in this pipeline (and
        // without a Bloom post-process it would not glow anyway). A bright base
        // color reads as "emissive" here; distinction comes from color + the
        // floating beacon + height + spin, not a post-process.
        private static readonly Color ModuleColor = new Color(0.1f, 0.9f, 1f); // bright cyan
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int LegacyColorId = Shader.PropertyToID("_Color");
        private static Material sharedModuleMaterial;

        [Tooltip("Sprint 2.9 game-feel: once the player is within this radius the module is drawn in and collected. Matches CurrencyPickup — a small convenience, not a long-range vacuum.")]
        [Min(0f)]
        [SerializeField]
        private float pickupRadius = 1.75f;

        [Tooltip("Speed the pickup moves toward the player once inside the radius.")]
        [SerializeField]
        private float attractSpeed = 8f;

        [Tooltip("Sprint 2.9 review: cosmetic spin speed (deg/sec) so the module reads as a live, distinct object, not a static coin.")]
        [SerializeField]
        private float spinSpeed = 90f;

        [Tooltip("Sprint 2.9 review: pulse speed of the floating beacon above the module.")]
        [SerializeField]
        private float pulseSpeed = 4f;

        private const float CollectDistance = 0.5f;

        private Transform playerTransform;
        private WeaponModule playerModule;
        private Collider trigger;
        private bool collected;
        private GameObject beacon;
        private float pulsePhase;

        private void Awake()
        {
            trigger = GetComponent<Collider>();
            trigger.isTrigger = true;
            Active.Add(this);
            SetupVisual();
        }

        // Sprint 2.9 review: make the module visually unmistakable next to currency.
        // Self-contained (no VFX/loot framework, same asset-free runtime-material
        // approach DetonationMark uses): an emissive cyan body plus a small floating
        // beacon above it so the drop location is legible over coins/corpses/effects.
        private void SetupVisual()
        {
            Renderer bodyRenderer = GetComponentInChildren<Renderer>();
            if (bodyRenderer != null)
            {
                bodyRenderer.sharedMaterial = GetModuleMaterial();
            }

            // Floating marker beacon above the module — the "which location produced
            // it" cue. Collider-less so it never blocks or is queried; a child, so it
            // is destroyed with the pickup.
            beacon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            beacon.name = "WeaponModuleBeacon";
            Collider beaconCol = beacon.GetComponent<Collider>();
            if (beaconCol != null)
            {
                Destroy(beaconCol);
            }
            beacon.transform.SetParent(transform, false);
            beacon.transform.localScale = Vector3.one * 0.55f;
            beacon.transform.localPosition = new Vector3(0f, 2.4f, 0f);
            Renderer beaconRenderer = beacon.GetComponent<Renderer>();
            if (beaconRenderer != null)
            {
                beaconRenderer.sharedMaterial = GetModuleMaterial();
            }
        }

        // Same asset-free runtime-material approach DetonationMark uses: a plain
        // shared URP/Lit material tinted bright cyan (no emission keyword — see the
        // ModuleColor note above).
        private static Material GetModuleMaterial()
        {
            if (sharedModuleMaterial == null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit");
                if (shader == null)
                {
                    shader = Shader.Find("Sprites/Default");
                }
                sharedModuleMaterial = new Material(shader) { name = "WeaponModuleVfx (runtime shared)" };
                sharedModuleMaterial.SetColor(BaseColorId, ModuleColor);
                sharedModuleMaterial.SetColor(LegacyColorId, ModuleColor);
            }

            return sharedModuleMaterial;
        }

        private void OnDestroy()
        {
            Active.Remove(this);
        }

        // Cosmetic only — spins the body and pulses/bobs the beacon. Applied to the
        // body's rotation and the beacon child (never the root position), so it can't
        // interfere with the attract/collect distance logic below.
        private void AnimateVisual()
        {
            transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);

            if (beacon != null)
            {
                pulsePhase += Time.deltaTime * pulseSpeed;
                float pulse = Mathf.Sin(pulsePhase);
                beacon.transform.localScale = Vector3.one * (0.55f + 0.12f * pulse);
                beacon.transform.localPosition = new Vector3(0f, 2.4f + 0.2f * pulse, 0f);
            }
        }

        private void Update()
        {
            if (collected)
            {
                return;
            }

            AnimateVisual();
            ResolvePlayer();
            if (playerTransform == null)
            {
                return;
            }

            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= CollectDistance)
            {
                Collect();
                return;
            }

            if (distance <= pickupRadius)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, playerTransform.position, attractSpeed * Time.deltaTime);
            }
        }

        // Fallback for direct contact (e.g. the player walks straight onto it).
        private void OnTriggerEnter(Collider other)
        {
            if (collected)
            {
                return;
            }

            if (other.CompareTag("Player") || other.GetComponentInParent<PlayerController>() != null)
            {
                Collect();
            }
        }

        // Single guarded collection path shared by the radius pull and the trigger
        // — the `collected` flag makes double collection impossible.
        private void Collect()
        {
            if (collected)
            {
                return;
            }

            ResolveModule();
            if (playerModule == null)
            {
                return;
            }

            collected = true;
            playerModule.Equip();
            Destroy(gameObject);
        }

        private void ResolvePlayer()
        {
            if (playerTransform == null && PlayerReference.Instance != null)
            {
                playerTransform = PlayerReference.Instance;
            }
        }

        private void ResolveModule()
        {
            if (playerModule != null)
            {
                return;
            }

            ResolvePlayer();
            if (playerTransform != null)
            {
                playerModule = playerTransform.GetComponentInParent<WeaponModule>();
            }
        }

        /// <summary>Destroys all live module pickups without collecting them
        /// (run-end cleanup — an uncollected module is never banked).</summary>
        public static void ClearAll()
        {
            WeaponModulePickup[] snapshot = Active.ToArray();
            for (int i = 0; i < snapshot.Length; i++)
            {
                if (snapshot[i] != null)
                {
                    Destroy(snapshot[i].gameObject);
                }
            }
            Active.Clear();
        }
    }
}
