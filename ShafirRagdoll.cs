using UnityEngine;

namespace Shafir.Ragdoll
{
    /// <summary>
    /// Рэгдолл
    /// </summary>
    public class ShafirRagdoll : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private RagdollPart[] _parts;
        private RagdollJoint[] _joints;

        public void Initialize()
        {
            _parts = GetComponentsInChildren<RagdollPart>();
            _joints = GetComponentsInChildren<RagdollJoint>();

            foreach (var part in _parts)
            {
                part.SetEnabled(false);
            }

            foreach (var joint in _joints)
            {
                joint.Initialize();
                joint.SetEnabled(false);
            }
        }

        public void Enable()
        {
            SetInternal(true);
        }

        public void Disable()
        {
            SetInternal(false);
        }

        private void SetInternal(bool isEnabled)
        {
            foreach (var part in _parts)
            {
                part.SetEnabled(isEnabled);
            }

            foreach (var joint in _joints)
            {
                joint.SetEnabled(isEnabled);
            }

            animator.enabled = !isEnabled;
        }
    }
}