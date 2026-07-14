using System.Collections.Generic;
using UnityEngine;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Enemies
{
    /// <summary>
    /// PROTOTYPE (greybox — disposable vehicle for the "identify &amp; kill the
    /// priority target" question; its final identity may become a shaman /
    /// commander / ritualist). Hangs back and empowers nearby chasers (EnemyAI):
    /// faster + tinted while in its aura; reverts when they leave or when it dies.
    /// The visible buff is the whole lesson — kill the empowerer first.
    ///
    /// Block 1 (PC-017): the buff now goes through EnemyAI.SetMoveSpeed (a public
    /// surface), replacing the earlier reflection hack. Still not a buff system —
    /// just an honest speed set/restore.
    /// </summary>
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]
    public class EmpowererAI : MonoBehaviour
    {
        [SerializeField] private float standoff = 8.5f;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float auraRadius = 6f;
        [SerializeField] private float baseChaserSpeed = 3.5f;
        [SerializeField] private float empoweredChaserSpeed = 6.5f;
        [SerializeField] private Color bodyColor = new Color(1f, 0.8f, 0.1f); // gold
        [SerializeField] private Color empoweredTint = new Color(1f, 0.55f, 0.1f);
        [SerializeField] private Transform player;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly Collider[] Buffer = new Collider[64];

        private Health health;
        private Rigidbody rb;
        private Renderer bodyRenderer;
        private GameObject aura;
        private readonly HashSet<EnemyAI> buffed = new HashSet<EnemyAI>();

        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            bodyRenderer = GetComponentInChildren<Renderer>();
            Tint(bodyRenderer, bodyColor);
            BuildAura();
        }

        private void OnEnable() { health.Died += Die; }
        private void OnDisable() { health.Died -= Die; RevertAll(); }

        private void FixedUpdate()
        {
            if (player == null) player = PlayerReference.Instance;
            if (player == null || health.IsDead) return;

            // Hang back at standoff.
            Vector3 to = player.position - rb.position; to.y = 0f;
            float d = to.magnitude;
            if (to.sqrMagnitude > 0.0001f) rb.MoveRotation(Quaternion.LookRotation(to.normalized));
            if (d < standoff - 1f) rb.MovePosition(rb.position - to.normalized * moveSpeed * Time.fixedDeltaTime);
            else if (d > standoff + 1f) rb.MovePosition(rb.position + to.normalized * moveSpeed * Time.fixedDeltaTime);

            UpdateAura();
        }

        private void UpdateAura()
        {
            int count = Physics.OverlapSphereNonAlloc(rb.position, auraRadius, Buffer, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            HashSet<EnemyAI> inRange = new HashSet<EnemyAI>();
            for (int i = 0; i < count; i++)
            {
                if (Buffer[i] == null) continue;
                if (Buffer[i].TryGetComponent(out EnemyAI e))
                {
                    inRange.Add(e);
                    if (buffed.Add(e)) { SetSpeed(e, empoweredChaserSpeed); Tint(e.GetComponentInChildren<Renderer>(), empoweredTint); }
                }
            }
            // Revert any that left the aura.
            List<EnemyAI> left = new List<EnemyAI>();
            foreach (var e in buffed) if (e == null || !inRange.Contains(e)) left.Add(e);
            foreach (var e in left) { if (e != null) { SetSpeed(e, baseChaserSpeed); Tint(e.GetComponentInChildren<Renderer>(), new Color(0.85f, 0.15f, 0.15f)); } buffed.Remove(e); }
        }

        private void RevertAll()
        {
            foreach (var e in buffed) if (e != null) SetSpeed(e, baseChaserSpeed);
            buffed.Clear();
            if (aura != null) Destroy(aura);
        }

        private void SetSpeed(EnemyAI e, float s) { if (e != null) e.SetMoveSpeed(s); }

        private void Tint(Renderer r, Color c)
        {
            if (r == null) return;
            MaterialPropertyBlock b = new MaterialPropertyBlock();
            r.GetPropertyBlock(b);
            b.SetColor(BaseColorId, c);
            r.SetPropertyBlock(b);
        }

        private void BuildAura()
        {
            aura = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            aura.name = "EmpowerAura";
            Collider c = aura.GetComponent<Collider>(); if (c != null) Destroy(c);
            aura.transform.SetParent(transform, false);
            aura.transform.localScale = new Vector3(auraRadius * 2f, 0.05f, auraRadius * 2f);
            var r = aura.GetComponent<Renderer>();
            MaterialPropertyBlock b = new MaterialPropertyBlock();
            b.SetColor(BaseColorId, new Color(1f, 0.7f, 0.1f, 0.25f));
            r.SetPropertyBlock(b);
        }

        private void Die() { Destroy(gameObject); }
    }
}
