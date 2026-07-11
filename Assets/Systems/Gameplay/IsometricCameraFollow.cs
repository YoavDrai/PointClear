using UnityEngine;

namespace PointClear.Gameplay
{
    /// <summary>
    /// Fixed-angle isometric camera that smoothly follows a target position.
    /// Never rotates from player input — rotation is set once in the editor.
    /// </summary>
    public class IsometricCameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private Vector3 offset = new Vector3(0f, 12f, -8f);

        [SerializeField]
        private float smoothTime = 0.2f;

        private Vector3 velocity;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
    }
}
