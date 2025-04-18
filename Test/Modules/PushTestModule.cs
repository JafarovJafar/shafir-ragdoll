using UnityEngine;

namespace Shafir.Ragdoll.Test
{
    internal class PushTestModule : BaseTestModule
    {
        private Camera _mainCamera;
        private ShafirRagdoll _ragdoll;

        public PushTestModule(Camera mainCamera, ShafirRagdoll ragdoll)
        {
            _mainCamera = mainCamera;
            _ragdoll = ragdoll;
        }

        public override void Update()
        {

        }
    }
}