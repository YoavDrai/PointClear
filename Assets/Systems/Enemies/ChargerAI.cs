using UnityEngine;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Enemies
{
    /// <summary>
    /// PROTOTYPE (rapid test — throwaway, not final). Telegraphed charge enemy:
    /// chase → wind-up (flare + a line showing the charge path) → dash in a locked
    /// straight line → recover (vulnerable). The player dodges the line and punishes
    /// the recovery. Minimal on purpose; no shared framework.
    /// </summary>
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]
    public class ChargerAI : MonoBehaviour
    {
        private enum State { Chasing, Telegraph, Dashing, Recovering }

        [SerializeField] private float chaseSpeed = 3.5f;
        [SerializeField] private float dashSpeed = 16f;
        [SerializeField] private float triggerRange = 11f;
        [SerializeField] private float telegraphDuration = 0.8f;
        [SerializeField] private float dashDuration = 0.35f;
        [SerializeField] private float recoverDuration = 1.1f;
        [SerializeField] private float chargeLength = 9f;
        [SerializeField] private float contactRange = 1.7f;
        [SerializeField] private float contactDamage = 20f;
        [SerializeField] private float obstacleSkin = 0.2f;
        [SerializeField] private Color bodyColor = new Color(1f, 0.55f, 0.1f); // orange
        [SerializeField] private Transform player;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private Health health;
        private Rigidbody rb;
        private Renderer bodyRenderer;
        private Vector3 baseScale;
        private State state = State.Chasing;
        private float timer;
        private Vector3 dashDir;
        private bool hitThisDash;
        private GameObject indicator;

        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            bodyRenderer = GetComponentInChildren<Renderer>();
            baseScale = transform.localScale;
            Tint(bodyColor);
            SetupIndicator();
        }

        private void OnEnable() { health.Died += Die; }
        private void OnDisable() { health.Died -= Die; }

        private void FixedUpdate()
        {
            if (player == null) player = PlayerReference.Instance;
            if (player == null || health.IsDead) return;
            timer -= Time.fixedDeltaTime;

            switch (state)
            {
                case State.Chasing: Chase(); break;
                case State.Telegraph: Telegraph(); break;
                case State.Dashing: Dash(); break;
                case State.Recovering: if (timer <= 0f) { state = State.Chasing; Tint(bodyColor); transform.localScale = baseScale; } break;
            }
        }

        private void Chase()
        {
            Vector3 to = player.position - rb.position; to.y = 0f;
            float d = to.magnitude;
            Face(to);
            if (d <= triggerRange) { EnterTelegraph(); return; }
            Vector3 dir = to.normalized;
            rb.MovePosition(Block(rb.position + dir * chaseSpeed * Time.fixedDeltaTime, dir));
        }

        private void EnterTelegraph()
        {
            state = State.Telegraph; timer = telegraphDuration;
            Vector3 to = player.position - rb.position; to.y = 0f;
            dashDir = to.sqrMagnitude > 0.0001f ? to.normalized : transform.forward;
            Face(dashDir);
            Tint(Color.red);
            transform.localScale = baseScale * 1.3f;
            if (indicator != null) indicator.SetActive(true);
            UpdateIndicator();
        }

        private void Telegraph()
        {
            UpdateIndicator();
            if (timer <= 0f) { state = State.Dashing; timer = dashDuration; hitThisDash = false; if (indicator != null) indicator.SetActive(false); Tint(new Color(1f, 0.9f, 0.3f)); }
        }

        private void Dash()
        {
            rb.MovePosition(Block(rb.position + dashDir * dashSpeed * Time.fixedDeltaTime, dashDir));
            if (!hitThisDash)
            {
                Vector3 pp = player.position - rb.position; pp.y = 0f;
                if (pp.magnitude <= contactRange)
                {
                    if (player.TryGetComponent(out Health ph)) ph.TakeDamage(contactDamage);
                    hitThisDash = true;
                }
            }
            if (timer <= 0f) { state = State.Recovering; timer = recoverDuration; Tint(new Color(0.45f, 0.3f, 0.3f)); }
        }

        private Vector3 Block(Vector3 intended, Vector3 dir)
        {
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) return intended;
            dir.Normalize();
            float dist = Vector3.Distance(rb.position, intended);
            if (dist <= 0.0001f) return intended;
            if (!rb.SweepTest(dir, out RaycastHit hit, dist, QueryTriggerInteraction.Ignore)) return intended;
            if (hit.collider == null || hit.collider.GetComponentInParent<EnemyAI>() != null
                || hit.collider.GetComponentInParent<ChargerAI>() != null
                || hit.collider.GetComponentInParent<PlayerController>() != null || hit.normal.y > 0.5f) return intended;
            Vector3 blocked = rb.position + dir * Mathf.Max(0f, hit.distance - obstacleSkin);
            blocked.y = rb.position.y;
            return blocked;
        }

        private void Face(Vector3 dir) { dir.y = 0f; if (dir.sqrMagnitude > 0.0001f) rb.MoveRotation(Quaternion.LookRotation(dir)); }

        private void Tint(Color c)
        {
            if (bodyRenderer == null) return;
            MaterialPropertyBlock b = new MaterialPropertyBlock();
            bodyRenderer.GetPropertyBlock(b);
            b.SetColor(BaseColorId, c);
            bodyRenderer.SetPropertyBlock(b);
        }

        // Robust telegraph tell: a thin flat cube laid along the charge path,
        // tinted via a MaterialPropertyBlock on the primitive's default material
        // (no Shader.Find — the LineRenderer approach nulled its material in URP).
        // Unparented so the charger's telegraph scale-up doesn't distort it.
        private void SetupIndicator()
        {
            indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            indicator.name = "ChargeTell";
            Collider col = indicator.GetComponent<Collider>(); if (col != null) Destroy(col);
            Renderer r = indicator.GetComponent<Renderer>();
            MaterialPropertyBlock b = new MaterialPropertyBlock();
            b.SetColor(BaseColorId, new Color(1f, 0.15f, 0.1f));
            r.SetPropertyBlock(b);
            indicator.SetActive(false);
        }

        private void UpdateIndicator()
        {
            if (indicator == null) return;
            Vector3 origin = rb.position + Vector3.up * 0.15f;
            indicator.transform.position = origin + dashDir * (chargeLength * 0.5f);
            indicator.transform.rotation = Quaternion.LookRotation(dashDir.sqrMagnitude > 0.0001f ? dashDir : Vector3.forward);
            indicator.transform.localScale = new Vector3(0.5f, 0.1f, chargeLength);
        }

        private void OnDestroy() { if (indicator != null) Destroy(indicator); }

        private void Die() { Destroy(gameObject); }
    }
}
