using UnityEngine;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Enemies
{
    /// <summary>
    /// PROTOTYPE (rapid test — throwaway, not final). Instead of clumping in front
    /// of the player, each Surrounder approaches a point at a fixed angle AROUND the
    /// player, so the group attacks from multiple sides. Standing still gets you
    /// flanked; you have to keep moving. Melee like the chaser. Minimal, no framework.
    /// </summary>
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]
    public class SurrounderAI : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3.6f;
        [SerializeField] private float ringDistance = 2.2f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackInterval = 1f;
        [SerializeField] private float obstacleSkin = 0.2f;
        [SerializeField] private Color bodyColor = new Color(0.2f, 0.7f, 0.4f); // green
        [SerializeField] private Transform player;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static int spawnIndex;

        private Health health;
        private Rigidbody rb;
        private Renderer bodyRenderer;
        private float angle;      // fixed approach angle around the player (degrees)
        private float nextAttack;

        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            bodyRenderer = GetComponentInChildren<Renderer>();
            // Spread approach angles deterministically so they ring the player.
            angle = (spawnIndex++ * 67f) % 360f;
            Tint(bodyColor);
        }

        private void OnEnable() { health.Died += Die; }
        private void OnDisable() { health.Died -= Die; }

        private void FixedUpdate()
        {
            if (player == null) player = PlayerReference.Instance;
            if (player == null || health.IsDead) return;

            // Target a point on the ring around the player at this enemy's angle.
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * ringDistance;
            Vector3 target = player.position + offset;
            Vector3 to = target - rb.position; to.y = 0f;
            float d = to.magnitude;

            Vector3 toPlayer = player.position - rb.position; toPlayer.y = 0f;
            if (toPlayer.sqrMagnitude > 0.0001f) rb.MoveRotation(Quaternion.LookRotation(toPlayer.normalized));

            if (toPlayer.magnitude <= attackRange)
            {
                if (Time.time >= nextAttack) { if (player.TryGetComponent(out Health ph)) ph.TakeDamage(attackDamage); nextAttack = Time.time + attackInterval; }
                return;
            }

            if (d > 0.05f)
            {
                Vector3 dir = to.normalized;
                rb.MovePosition(Block(rb.position + dir * moveSpeed * Time.fixedDeltaTime, dir));
            }
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
                || hit.collider.GetComponentInParent<SurrounderAI>() != null
                || hit.collider.GetComponentInParent<PlayerController>() != null || hit.normal.y > 0.5f) return intended;
            Vector3 blocked = rb.position + dir * Mathf.Max(0f, hit.distance - obstacleSkin);
            blocked.y = rb.position.y;
            return blocked;
        }

        private void Tint(Color c)
        {
            if (bodyRenderer == null) return;
            MaterialPropertyBlock b = new MaterialPropertyBlock();
            bodyRenderer.GetPropertyBlock(b);
            b.SetColor(BaseColorId, c);
            bodyRenderer.SetPropertyBlock(b);
        }

        private void Die() { Destroy(gameObject); }
    }
}
