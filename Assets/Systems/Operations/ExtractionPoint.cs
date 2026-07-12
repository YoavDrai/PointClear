using UnityEngine;
using PointClear.Player;

namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.6: the run's finish line. A trigger volume that, while OPEN,
    /// reports the player's entry to the OperationController as a successful
    /// extraction. Closed by default; the controller opens it once the kill
    /// quota is met. This is only the extraction POINT (a win trigger) —
    /// securing rewards at extraction is Sprint 2.7, not here.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ExtractionPoint : MonoBehaviour
    {
        [SerializeField]
        private OperationController operation;

        [Tooltip("Optional visual shown only while the extraction is open.")]
        [SerializeField]
        private GameObject openVisual;

        private Collider triggerCollider;

        private void Awake()
        {
            triggerCollider = GetComponent<Collider>();
            triggerCollider.isTrigger = true;

            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
            }

            if (openVisual == null)
            {
                Transform marker = transform.Find("Marker");
                if (marker != null)
                {
                    openVisual = marker.gameObject;
                }
            }

            SetOpen(false);
        }

        /// <summary>Enable/disable the exit. Closed extraction ignores the player.</summary>
        public void SetOpen(bool open)
        {
            if (triggerCollider != null)
            {
                triggerCollider.enabled = open;
            }

            if (openVisual != null)
            {
                openVisual.SetActive(open);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (operation == null)
            {
                return;
            }

            if (other.CompareTag("Player") || other.GetComponentInParent<PlayerController>() != null)
            {
                operation.NotifyExtractionReached();
            }
        }
    }
}
