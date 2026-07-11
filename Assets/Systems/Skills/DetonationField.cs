using UnityEngine;
using UnityEngine.InputSystem;
using PointClear.Combat;
using PointClear.Enemies;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.3 Active Skill (Area). On activation, marks every enemy within
    /// markRadius of the player. A marked enemy that dies within markDuration
    /// detonates (see DetonationMark) — a "set up, then chain" payoff, distinct
    /// from Fracture Bolt's direct ranged damage.
    ///
    /// Self-centered: no separate targeting/reticle this sprint. Owns its own
    /// cooldown gate (not a shared abstraction — same rationale as FractureBolt).
    ///
    /// The marking query uses its OWN buffer, never EnemyAI.SeparationBuffer
    /// (a shared static array reused every EnemyAI.FixedUpdate — borrowing it
    /// would corrupt separation calculations mid-frame).
    /// </summary>
    public class DetonationField : MonoBehaviour
    {
        [SerializeField]
        private float markRadius = 5f;

        [SerializeField]
        private float markDuration = 4f;

        [SerializeField]
        private float explosionRadius = 3f;

        [SerializeField]
        private float explosionDamage = 25f;

        [SerializeField]
        private float cooldown = 6f;

        [SerializeField]
        private GameObject fieldVfxPrefab;

        [SerializeField]
        private float fieldVfxDuration = 0.4f;

        public float Cooldown => cooldown;
        public bool IsReady => Time.time >= nextReadyTime;
        public float CooldownRemaining => Mathf.Max(0f, nextReadyTime - Time.time);

        private static readonly Collider[] MarkBuffer = new Collider[64];

        private InputAction activateAction;
        private float nextReadyTime;

        private void OnEnable()
        {
            activateAction = InputSystem.actions.FindAction("DetonationField");
        }

        private void Update()
        {
            if (activateAction != null && activateAction.WasPressedThisFrame() && IsReady)
            {
                Activate();
                nextReadyTime = Time.time + cooldown;
            }
        }

        private void Activate()
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                markRadius,
                MarkBuffer,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Collider other = MarkBuffer[i];
                if (other == null || !other.TryGetComponent(out EnemyAI _))
                {
                    continue;
                }

                if (!other.TryGetComponent(out Health health) || health.IsDead)
                {
                    continue;
                }

                DetonationMark mark = other.GetComponent<DetonationMark>();
                if (mark == null)
                {
                    mark = other.gameObject.AddComponent<DetonationMark>();
                }

                mark.Apply(markDuration, explosionRadius, explosionDamage);
            }

            ShowFieldVfx();
        }

        private void ShowFieldVfx()
        {
            if (fieldVfxPrefab == null)
            {
                return;
            }

            GameObject vfx = Instantiate(fieldVfxPrefab, transform.position, Quaternion.identity);
            vfx.transform.localScale = new Vector3(markRadius * 2f, 0.1f, markRadius * 2f);
            Destroy(vfx, fieldVfxDuration);
        }
    }
}
