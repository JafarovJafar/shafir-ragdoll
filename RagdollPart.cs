using UnityEngine;

namespace Shafir.Ragdoll
{
    /// <summary>
    /// Часть тела (или конечность)
    /// </summary>
    internal class RagdollPart : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private Rigidbody rigidbody;

        public void SetEnabled(bool isEnabled)
        {
            rigidbody.isKinematic = !isEnabled;
            collider.enabled = isEnabled;
        }

        private void Reset()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }
    }
}