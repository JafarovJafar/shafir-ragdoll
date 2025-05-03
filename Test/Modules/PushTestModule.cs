using System;
using UnityEngine;

namespace Shafir.Ragdoll.Test
{
    [Serializable]
    internal class PushSettings
    {
        public float Strength;
        public ForceMode ForceMode;
    }

    internal class PushTestModule : BaseTestModule
    {
        private Camera _mainCamera;
        private ShafirRagdoll _ragdoll;
        private PushSettings _pushSettings;

        private bool _pushed;

        public PushTestModule(Camera mainCamera, ShafirRagdoll ragdoll, PushSettings pushSettings)
        {
            _mainCamera = mainCamera;
            _ragdoll = ragdoll;
            _pushSettings = pushSettings;
        }

        public override void Update()
        {
            if (Input.GetMouseButton(0) == false)
            {
                _pushed = false;
                return;
            }

            if (_pushed == true)
                return;

            _pushed = true;

            var ray = GetRay();

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out RagdollPart part))
                {
                    _ragdoll.Enable();
                    part.AddForce(Vector3.forward, ForceMode.VelocityChange);
                }
            }
        }

        private Ray GetRay()
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 1f;

            var ray = _mainCamera.ScreenPointToRay(mousePos);

            return ray;
        }
    }
}