using UnityEngine;

namespace Shafir.Ragdoll
{
    /// <summary>
    /// Часть тела (или конечность)
    /// </summary>
    public class RagdollPart : MonoBehaviour
    {
        public Rigidbody Rigidbody => rigidbody;

        [SerializeField] private Collider collider;
        [SerializeField] private Rigidbody rigidbody;

        public void SetEnabled(bool isEnabled)
        {
            rigidbody.isKinematic = !isEnabled;
            collider.enabled = isEnabled;
        }

        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            rigidbody.AddForce(force, forceMode);
        }

        private void Reset()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }
    }
}