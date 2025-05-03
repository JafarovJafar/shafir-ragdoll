using UnityEngine;

namespace Shafir.Ragdoll.Test
{
    internal class DragTestModule : BaseTestModule
    {
        private DragSettings _settings;
        
        private Camera _mainCamera;

        private DragMode _currentMode;

        private RagdollPart _dragGoal;

        private Plane _plane;

        private enum DragMode
        {
            Pending,
            NotFound,
            Dragging,
        }

        public DragTestModule(Camera mainCamera, DragSettings dragSettings)
        {
            _mainCamera = mainCamera;
            _settings = dragSettings;

            _currentMode = DragMode.Pending;
        }

        public override void Update()
        {
            switch (_currentMode)
            {
                case DragMode.Pending:
                    Pend();
                    break;

                case DragMode.NotFound:
                    NotFound();
                    break;

                case DragMode.Dragging:
                    Drag();
                    break;
            }
        }

        private void Pend()
        {
            if (Input.GetMouseButton(0) == false)
                return;

            var ray = GetRay();

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out RagdollPart part))
                {
                    _currentMode = DragMode.Dragging;
                    _dragGoal = part;

                    var point1 = hit.point;
                    var point2 = hit.point + Vector3.up;
                    var point3 = hit.point + _mainCamera.transform.right;

                    _plane = new Plane(point1, point2, point3);
                    return;
                }
            }

            _currentMode = DragMode.NotFound;
        }

        private void NotFound()
        {
            if (Input.GetMouseButton(0) == true)
                return;

            _currentMode = DragMode.Pending;
        }

        private void Drag()
        {
            if (Input.GetMouseButton(0) == false)
            {
                _currentMode = DragMode.Pending;
                return;
            }

            var ray = GetRay();

            if (_plane.Raycast(ray, out var enter))
            {
                var point = ray.GetPoint(enter);
                var dir = point - _dragGoal.transform.position;
                _dragGoal.Rigidbody.AddForce(dir * _settings.Multiplier, _settings.ForceMode);
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