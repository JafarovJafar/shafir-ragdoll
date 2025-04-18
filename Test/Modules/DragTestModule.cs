using UnityEngine;

namespace Shafir.Ragdoll.Test
{
    internal class DragTestModule : BaseTestModule
    {
        private Camera _mainCamera;
        private ShafirRagdoll _ragdoll;

        public DragTestModule(Camera mainCamera, ShafirRagdoll ragdoll)
        {
            _mainCamera = mainCamera;
            _ragdoll = ragdoll;
        }

        public override void Update()
        {

        }
    }
}