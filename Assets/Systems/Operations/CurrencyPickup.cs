using System.Collections.Generic;
using UnityEngine;
using PointClear.Player;

namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.7: a simple physical currency drop. A trigger volume the player
    /// walks over to collect a fixed value into the CurrencyWallet's Unsecured
    /// pool. Trigger-based so it never blocks movement; collected only by the
    /// player, at most once; self-destroys on collection. Uncollected pickups are
    /// destroyed by ClearAll() at run end (and are never banked). Not a general
    /// pickup framework — one currency, fixed value.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class CurrencyPickup : MonoBehaviour
    {
        private static readonly List<CurrencyPickup> Active = new List<CurrencyPickup>();
        private static readonly Color CoinColor = new Color(1f, 0.85f, 0.1f); // gold = "value"
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int LegacyColorId = Shader.PropertyToID("_Color");

        [SerializeField]
        private int value = 5;

        [Tooltip("Sprint 2.7 game-feel: once the player is within this radius the pickup is drawn in and collected — a small convenience so you needn't step exactly on it. NOT a long-range vacuum. Tune after playtest.")]
        [Min(0f)]
        [SerializeField]
        private float pickupRadius = 1.75f;

        [Tooltip("Speed the pickup moves toward the player once inside the radius.")]
        [SerializeField]
        private float attractSpeed = 8f;

        private const float CollectDistance = 0.5f;

        private Transform playerTransform;
        private CurrencyWallet playerWallet;
        private Collider trigger;
        private bool collected;

        private void Awake()
        {
            trigger = GetComponent<Collider>();
            trigger.isTrigger = true;
            Active.Add(this);
            TintGold();
        }

        // Prototype visual: tint gold via a MaterialPropertyBlock (same
        // asset-free approach DetonationMark uses) so the drop reads as value.
        private void TintGold()
        {
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer == null)
            {
                return;
            }

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor(BaseColorId, CoinColor);
            block.SetColor(LegacyColorId, CoinColor);
            renderer.SetPropertyBlock(block);
        }

        private void OnDestroy()
        {
            Active.Remove(this);
        }

        /// <summary>Set the value this pickup grants (the dropping enemy decides it).</summary>
        public void SetValue(int amount)
        {
            value = amount;
        }

        private void Update()
        {
            if (collected)
            {
                return;
            }

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
                // Small convenience pull toward the player — bounded by pickupRadius,
                // not a global loot vacuum.
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

        // Single guarded collection path shared by the radius pull and the
        // trigger — the `collected` flag makes double collection impossible.
        private void Collect()
        {
            if (collected)
            {
                return;
            }

            ResolveWallet();
            if (playerWallet == null)
            {
                return;
            }

            collected = true;
            playerWallet.AddUnsecured(value);
            Destroy(gameObject);
        }

        private void ResolvePlayer()
        {
            if (playerTransform == null && PlayerReference.Instance != null)
            {
                playerTransform = PlayerReference.Instance;
            }
        }

        private void ResolveWallet()
        {
            if (playerWallet != null)
            {
                return;
            }

            ResolvePlayer();
            if (playerTransform != null)
            {
                playerWallet = playerTransform.GetComponentInParent<CurrencyWallet>();
            }
        }

        /// <summary>Destroys all live pickups without collecting them (run-end
        /// cleanup — uncollected currency is never banked).</summary>
        public static void ClearAll()
        {
            CurrencyPickup[] snapshot = Active.ToArray();
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
