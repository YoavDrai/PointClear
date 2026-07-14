using System.Collections;
using UnityEngine;
using PointClear.Combat;

namespace PointClear.Enemies
{
    /// <summary>
    /// Block 2A — Kill Feedback (greybox, disposable). Replaces an enemy's instant
    /// disappearance with a brief "death beat": on <see cref="Health.Died"/> the
    /// enemy stops behaving (its IEnemyBehaviour driver(s) and all colliders are
    /// disabled, so it can no longer move, attack, collide, or be hit again), plays
    /// a short pop→collapse with a white flash, then destroys itself.
    ///
    /// Restrained per the juice ceiling: very short, small, no freeze, no camera
    /// shake, no blocking effect, no persistent debris — it reads as "defeated"
    /// rather than "vanished" and stays legible with many simultaneous deaths.
    ///
    /// It owns ONLY the visual death sequence + teardown. All kill REWARDS (XP,
    /// currency, kill counter, Operation quota, mark detonation) fire on Health.Died
    /// through their own subscribers and are unaffected: Health raises Died exactly
    /// once, and the `beating` guard here makes double-processing impossible. Not a
    /// feedback framework — one small component per enemy.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class EnemyDeathBeat : MonoBehaviour
    {
        [Tooltip("Death-beat length before the enemy is destroyed (seconds). Keep short so it never slows the Arena rhythm.")]
        [SerializeField] private float duration = 0.15f;

        [Tooltip("Brief pop scale at the start of the beat, relative to the enemy's current size.")]
        [SerializeField] private float popScale = 1.2f;

        [SerializeField] private Color flashColor = Color.white;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private Health health;
        private Renderer bodyRenderer;
        private bool beating;

        private void Awake()
        {
            health = GetComponent<Health>();
            bodyRenderer = GetComponentInChildren<Renderer>();
        }

        private void OnEnable()
        {
            health.Died += HandleDied;
        }

        private void OnDisable()
        {
            health.Died -= HandleDied;
        }

        private void HandleDied()
        {
            if (beating)
            {
                return;
            }
            beating = true;

            StopBehaviour();
            StartCoroutine(Beat());
        }

        // Stop everything that would let a dead enemy keep acting or be re-processed:
        // its behaviour driver(s) and all colliders. Disabling EnemyAI here also fires
        // its OnDisable, which is where the authoritative ActiveCount is decremented —
        // so the count stays correct exactly once, as before.
        private void StopBehaviour()
        {
            MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                if (behaviours[i] is IEnemyBehaviour)
                {
                    behaviours[i].enabled = false;
                }
            }

            Collider[] colliders = GetComponentsInChildren<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    colliders[i].enabled = false;
                }
            }
        }

        private IEnumerator Beat()
        {
            Vector3 pop = transform.localScale * Mathf.Max(1f, popScale);
            SetColor(flashColor);

            float t = 0f;
            float length = Mathf.Max(0.01f, duration);
            while (t < length)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / length);
                // Quick pop, then collapse to nothing (ease-in shrink) — reads as defeat.
                transform.localScale = Vector3.Lerp(pop, Vector3.zero, k * k);
                yield return null;
            }

            Destroy(gameObject);
        }

        private void SetColor(Color color)
        {
            if (bodyRenderer == null)
            {
                return;
            }

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            bodyRenderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, color);
            bodyRenderer.SetPropertyBlock(block);
        }
    }
}
