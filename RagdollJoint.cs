using UnityEngine;

namespace Shafir.Ragdoll
{
    /// <summary>
    /// Сустав ragdoll
    /// </summary>
    internal class RagdollJoint : MonoBehaviour
    {
        [SerializeField] private Joint joint;

        private Rigidbody _cachedConnectedBody;

        public void Initialize()
        {
            _cachedConnectedBody = joint.connectedBody;
        }

        public void SetEnabled(bool isEnabled)
        {
            joint.connectedBody = isEnabled ? _cachedConnectedBody : null;
        }

        private void Reset()
        {
            joint = GetComponent<Joint>();
        }
    }
}