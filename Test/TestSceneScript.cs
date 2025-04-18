using System.Collections;
using EasyButtons;
using UnityEngine;

namespace Shafir.Ragdoll.Test
{
    public class TestSceneScript : MonoBehaviour
    {
        public enum Mode
        {
            Push,
            Drag,
            Permanent,
        }

        [SerializeField] private Camera mainCamera;
        [SerializeField] private ShafirRagdoll ragdoll;

        private PushTestModule _pushTestModule;
        private DragTestModule _dragTestModule;
        private PermanentTestModule _permanentTestModule;
        private Coroutine _updateCoroutine;

        private void Awake()
        {
            _pushTestModule = new PushTestModule(mainCamera, ragdoll);
            _dragTestModule = new DragTestModule(mainCamera, ragdoll);
            _permanentTestModule = new PermanentTestModule();
        }

        [Button]
        private void SwitchMode(Mode mode)
        {
            BaseTestModule module;

            switch (mode)
            {
                case Mode.Push:
                    module = _pushTestModule;
                    break;

                case Mode.Drag:
                    module = _dragTestModule;
                    break;

                case Mode.Permanent:
                    module = _permanentTestModule;
                    break;

                default:
                    if (_updateCoroutine != null)
                    {
                        StopCoroutine(_updateCoroutine);
                        _updateCoroutine = null;
                    }

                    return;
            }

            StartModule(module);
        }

        private void StartModule(BaseTestModule module)
        {
            if (_updateCoroutine != null)
            {
                StopCoroutine(_updateCoroutine);
            }

            _updateCoroutine = StartCoroutine(UpdateCoroutine(module));
        }

        private IEnumerator UpdateCoroutine(BaseTestModule module)
        {
            while (true)
            {
                module.Update();

                yield return null;
            }
        }
    }
}