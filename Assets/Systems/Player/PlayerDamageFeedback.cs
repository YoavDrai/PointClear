using System.Collections;
using UnityEngine;
using PointClear.Combat;

namespace PointClear.Player
{
    /// <summary>
    /// Block 2C — Player Damage Feedback (greybox, disposable). Gives *getting hit*
    /// a fair, instantly-readable beat with two channels, both driven off
    /// <see cref="Health.Damaged"/>:
    ///
    ///  1. World-space body flash — the player mesh flashes a hurt colour (red, so it
    ///     reads as "I got hit", distinct from the white "I hit something" flash on
    ///     enemies). Colour-only, no scale pop: the player is the visual/aim anchor,
    ///     so popping it would fight readability of your own position.
    ///  2. Screen-edge danger vignette — a brief red edge pulse (not a full-screen
    ///     wash) whose strength rises as health falls, so a hit at low HP reads as
    ///     more dangerous. Replaces DebugHud's debug full-screen flash.
    ///
    /// Restrained per the juice ceiling (Pillar 1 / Block 2A precedent): short, small,
    /// no camera shake, no hitstop, no audio. It answers "that hurt / I'm in danger"
    /// without the player reading a number. Not a feedback framework — one small
    /// component on the player.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class PlayerDamageFeedback : MonoBehaviour
    {
        [Header("Body flash (world-space)")]
        [Tooltip("Renderer flashed on hit. Auto-resolves to the Renderer on this GameObject if empty (the body mesh, not the muzzle-flash child).")]
        [SerializeField] private Renderer bodyRenderer;

        [Tooltip("Hurt colour flashed on the player body. Red = 'I got hit', distinct from enemies' white 'I hit something' flash.")]
        [SerializeField] private Color bodyFlashColor = new Color(1f, 0.15f, 0.15f, 1f);

        [Tooltip("How long the body flash lasts (seconds).")]
        [SerializeField] private float bodyFlashDuration = 0.1f;

        [Header("Screen-edge vignette")]
        [Tooltip("Colour of the screen-edge danger pulse.")]
        [SerializeField] private Color vignetteColor = new Color(0.8f, 0f, 0f, 1f);

        [Tooltip("How long each vignette pulse takes to fade out (seconds).")]
        [SerializeField] private float vignetteDuration = 0.35f;

        [Tooltip("Vignette opacity for a hit at full health (every hit stays visible).")]
        [Range(0f, 1f)]
        [SerializeField] private float vignetteMinAlpha = 0.25f;

        [Tooltip("Vignette opacity for a hit near death (danger reads stronger the lower your health).")]
        [Range(0f, 1f)]
        [SerializeField] private float vignetteMaxAlpha = 0.7f;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private Health health;
        private Coroutine flashRoutine;
        private bool bodyHasBaseColor;

        // Reused per-renderer override used to paint (and then un-paint) the hurt
        // flash. We never cache a "restore" colour: the flash is a temporary override
        // on top of whatever the material currently shows, and restoring just clears
        // the override so the live material colour (the selected preset, or any later
        // runtime change) shows through again. See HandleDamaged/ClearBodyFlash.
        private MaterialPropertyBlock flashBlock;

        // Vignette state (drawn via OnGUI).
        private Texture2D vignetteTexture;
        private float vignetteTimer;
        private float vignettePeakAlpha;

        private void Awake()
        {
            health = GetComponent<Health>();

            if (bodyRenderer == null)
            {
                bodyRenderer = GetComponent<Renderer>();
            }

            // Whether the body shader supports _BaseColor is a static property of the
            // shader, so it is safe to read from the shared material here — unlike the
            // colour VALUE, which is applied later on a runtime material instance
            // (see CombatBridge) and must never be cached.
            if (bodyRenderer != null && bodyRenderer.sharedMaterial != null &&
                bodyRenderer.sharedMaterial.HasProperty(BaseColorId))
            {
                bodyHasBaseColor = true;
            }

            flashBlock = new MaterialPropertyBlock();
            vignetteTexture = BuildVignetteTexture();
        }

        private void OnEnable()
        {
            health.Damaged += HandleDamaged;
        }

        private void OnDisable()
        {
            health.Damaged -= HandleDamaged;

            // Never leave the body stuck on the hurt colour if disabled mid-flash.
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
                flashRoutine = null;
                ClearBodyFlash();
            }
            vignetteTimer = 0f;
        }

        private void HandleDamaged(float amount)
        {
            // Body flash — restart cleanly if a previous flash is still running.
            if (bodyHasBaseColor)
            {
                if (flashRoutine != null)
                {
                    StopCoroutine(flashRoutine);
                }
                flashRoutine = StartCoroutine(BodyFlash());
            }

            // Vignette pulse — stronger the lower our current health.
            float healthFraction = health.MaxHealth > 0f
                ? Mathf.Clamp01(health.CurrentHealth / health.MaxHealth)
                : 0f;
            vignettePeakAlpha = Mathf.Lerp(vignetteMaxAlpha, vignetteMinAlpha, healthFraction);
            vignetteTimer = Mathf.Max(0.0001f, vignetteDuration);
        }

        private IEnumerator BodyFlash()
        {
            ApplyBodyFlash();
            yield return new WaitForSeconds(Mathf.Max(0.01f, bodyFlashDuration));
            ClearBodyFlash();
            flashRoutine = null;
        }

        // Paint the hurt colour as a per-renderer override on top of the material's
        // current colour. The material instance is never modified, so the underlying
        // (customised) colour is untouched and cannot be corrupted by repeated hits.
        private void ApplyBodyFlash()
        {
            if (bodyRenderer == null)
            {
                return;
            }

            bodyRenderer.GetPropertyBlock(flashBlock);
            flashBlock.SetColor(BaseColorId, bodyFlashColor);
            bodyRenderer.SetPropertyBlock(flashBlock);
        }

        // Remove the hurt-colour override so the renderer falls back to its material's
        // CURRENT colour — the selected preset, or any later runtime colour change.
        // We deliberately restore by clearing rather than by writing a cached colour,
        // so nothing is ever reset to a hardcoded default.
        private void ClearBodyFlash()
        {
            if (bodyRenderer == null)
            {
                return;
            }

            bodyRenderer.GetPropertyBlock(flashBlock);
            flashBlock.Clear();
            bodyRenderer.SetPropertyBlock(flashBlock);
        }

        private void Update()
        {
            if (vignetteTimer > 0f)
            {
                vignetteTimer -= Time.deltaTime;
            }
        }

        private void OnGUI()
        {
            if (vignetteTimer <= 0f || vignetteTexture == null)
            {
                return;
            }

            float fade = Mathf.Clamp01(vignetteTimer / Mathf.Max(0.0001f, vignetteDuration));
            Color previous = GUI.color;
            GUI.color = new Color(vignetteColor.r, vignetteColor.g, vignetteColor.b, vignettePeakAlpha * fade);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), vignetteTexture);
            GUI.color = previous;
        }

        // Radial alpha map: transparent at the centre, opaque toward the edges — an
        // edge vignette, so the play space stays legible while danger reads at the
        // periphery. Generated in code so Block 2C needs no art asset. Stretched to
        // the screen aspect by OnGUI (the ring becomes an ellipse hugging the edges).
        private static Texture2D BuildVignetteTexture()
        {
            const int size = 64;
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear
            };

            Vector2 center = new Vector2((size - 1) * 0.5f, (size - 1) * 0.5f);
            float maxDist = center.magnitude;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), center) / maxDist; // 0 centre -> 1 corner
                    // Nothing until ~55% out from centre, then ramp up toward the edge.
                    float a = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.55f, 1f, dist));
                    tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
                }
            }

            tex.Apply();
            return tex;
        }
    }
}
