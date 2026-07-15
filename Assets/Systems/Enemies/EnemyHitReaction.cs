using System.Collections;
using UnityEngine;
using PointClear.Combat;

namespace PointClear.Enemies
{
    /// <summary>
    /// Block 2B (greybox, disposable). The SHARED enemy hit reaction, one per enemy
    /// prefab. On <see cref="Health.Damaged"/> it briefly flashes the body cyan and
    /// pops its scale, then restores the colour and scale that were live WHEN THE HIT
    /// LANDED — so each enemy's own runtime tint and telegraph/aura state (Charger
    /// states, Empowerer gold, Surrounder tint) survive being shot.
    ///
    /// This is the same cyan reaction the Walker's EnemyAI used to own inline, moved
    /// out so every enemy prototype gets it consistently. It is NOT a new feedback
    /// channel or framework — colour flash + scale pop only.
    ///
    /// Death-safe: it ignores the killing blow and stops on <see cref="Health.Died"/>,
    /// leaving the death visuals entirely to EnemyDeathBeat.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class EnemyHitReaction : MonoBehaviour
    {
        [Tooltip("Hit flash colour. Cyan — distinct from death white, yellow/orange telegraphs, and the player's red hurt flash.")]
        [SerializeField] private Color flashColor = new Color(0.4f, 0.95f, 1f);

        [Tooltip("How long the flash/pop lasts (seconds).")]
        [SerializeField] private float flashDuration = 0.1f;

        [Tooltip("Scale multiplier at the peak of the hit pop, relative to the enemy's live scale.")]
        [SerializeField] private float popScale = 1.35f;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private Health health;
        private Renderer bodyRenderer;
        private MaterialPropertyBlock block;
        private Coroutine routine;

        private bool flashing;       // a flash is currently showing (cyan + popped)
        private bool hadOverride;    // did the body have a _BaseColor override before the flash?
        private Color restoreColor;  // tint to return to (valid when hadOverride)
        private Vector3 restoreScale;

        private void Awake()
        {
            health = GetComponent<Health>();
            // The body renderer is on the root, so it is returned first even when an
            // enemy later builds a child renderer (e.g. the Empowerer's aura).
            bodyRenderer = GetComponentInChildren<Renderer>();
            block = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            health.Damaged += HandleDamaged;
            health.Died += HandleDied;
        }

        private void OnDisable()
        {
            health.Damaged -= HandleDamaged;
            health.Died -= HandleDied;

            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }
            // If disabled mid-flash, put the body back so it is never left cyan/enlarged.
            if (flashing)
            {
                Restore();
                flashing = false;
            }
        }

        private void HandleDamaged(float amount)
        {
            // The killing blow belongs to EnemyDeathBeat — don't fight its death visuals.
            if (health.IsDead)
            {
                return;
            }

            // Capture the live tint + scale ONLY when not already flashing, so a hit
            // that lands mid-flash can never capture the cyan/popped state as the
            // restore target (which would corrupt the colour or leave it enlarged).
            if (!flashing)
            {
                CaptureLiveState();
                flashing = true;
            }

            if (routine != null)
            {
                StopCoroutine(routine);
            }
            routine = StartCoroutine(FlashRoutine());
        }

        private void HandleDied()
        {
            // Stop reacting and hand all death visuals to EnemyDeathBeat.
            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }
            flashing = false;
        }

        private void CaptureLiveState()
        {
            restoreScale = transform.localScale;
            if (bodyRenderer != null)
            {
                bodyRenderer.GetPropertyBlock(block);
                hadOverride = block.isEmpty == false;
                restoreColor = block.GetColor(BaseColorId);
            }
        }

        private IEnumerator FlashRoutine()
        {
            ApplyFlash();
            yield return new WaitForSeconds(Mathf.Max(0.01f, flashDuration));
            Restore();
            flashing = false;
            routine = null;
        }

        private void ApplyFlash()
        {
            transform.localScale = restoreScale * Mathf.Max(1f, popScale);
            if (bodyRenderer != null)
            {
                bodyRenderer.GetPropertyBlock(block);
                block.SetColor(BaseColorId, flashColor);
                bodyRenderer.SetPropertyBlock(block);
            }
        }

        private void Restore()
        {
            transform.localScale = restoreScale;
            if (bodyRenderer == null)
            {
                return;
            }

            // Return to the enemy's live tint. If it had no override (e.g. the grey
            // Walker), fall back to the shared material's colour — never a hardcoded
            // colour, so an untinted enemy is never recoloured by being hit.
            Color target = hadOverride
                ? restoreColor
                : (bodyRenderer.sharedMaterial != null
                    ? bodyRenderer.sharedMaterial.GetColor(BaseColorId)
                    : flashColor);

            bodyRenderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, target);
            bodyRenderer.SetPropertyBlock(block);
        }
    }
}
