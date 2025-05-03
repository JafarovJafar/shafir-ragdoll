using EasyButtons;
using UnityEngine;

namespace Shafir.Ragdoll
{
    internal class RagdollInstaller : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos;
        [SerializeField] private bool destroyAfterInstall;

        [SerializeField] private BoxPart pelvis;
        [SerializeField] private BoxPart spine_1;
        [SerializeField] private BoxPart spine_2;
        [SerializeField] private BoxPart spine_3;
        [SerializeField] private BoxPart head;
        [SerializeField] private LimbData leftLeg;
        [SerializeField] private LimbData rightLeg;
        [SerializeField] private LimbData leftArm;
        [SerializeField] private LimbData rightArm;

        private const float GIZMOS_TRANSPARENCY = 0.25f;
        private readonly Color pelvisColor = new Color(0, 1, 1, GIZMOS_TRANSPARENCY);
        private readonly Color spineColor = new Color(0, 1, 0, GIZMOS_TRANSPARENCY);
        private readonly Color headColor = new Color(1, 0, 1, GIZMOS_TRANSPARENCY);
        private readonly Color legsColor = new Color(0, 0, 1, GIZMOS_TRANSPARENCY);
        private readonly Color armsColor = new Color(1, 0, 0, GIZMOS_TRANSPARENCY);
        private readonly Color jointsColor = new Color(1, 1, 0, GIZMOS_TRANSPARENCY);

        private readonly float jointsRadius = 0.05f;

        private const string COLLIDER_NAME = "ShafirRagdollCollider";

        [Button]
        private void Install()
        {
            InstallSpine();
            InstallLegs();
            InstallArms();

            if (destroyAfterInstall)
            {
                gameObject.AddComponent<ShafirRagdoll>();
                DestroyImmediate(this);
            }
        }
        
        private void InstallLimb(LimbData limb)
        {
            InstallCapsule(limb.Point1, limb.Point2, limb.Radius);
            InstallCapsule(limb.Point2, limb.Point3, limb.Radius);
            InstallBox(limb.End);

            InstallJoint(limb.Point3.gameObject, limb.Point2.gameObject);
            InstallJoint(limb.Point2.gameObject, limb.Point1.gameObject);
        }

        private void ClearLimb(LimbData limb)
        {
            ClearCapsule(limb.Point1);
            ClearCapsule(limb.Point2);
            ClearBox(limb.End);
        }

        private void InstallSpine()
        {
            InstallBox(pelvis);

            InstallBox(spine_1);
            InstallBox(spine_2);
            InstallBox(spine_3);

            InstallBox(head);

            InstallJoint(spine_1.GoalTransform.gameObject, pelvis.GoalTransform.gameObject);
            InstallJoint(spine_2.GoalTransform.gameObject, spine_1.GoalTransform.gameObject);
            InstallJoint(spine_3.GoalTransform.gameObject, spine_2.GoalTransform.gameObject);

            InstallJoint(head.GoalTransform.gameObject, spine_3.GoalTransform.gameObject);
        }

        private void InstallLegs()
        {
            InstallLimb(leftLeg);
            InstallLimb(rightLeg);

            InstallJoint(leftLeg.Point1.gameObject, pelvis.GoalTransform.gameObject);
            InstallJoint(rightLeg.Point1.gameObject, pelvis.GoalTransform.gameObject);
        }

        private void InstallArms()
        {
            InstallLimb(leftArm);
            InstallLimb(rightArm);

            InstallJoint(leftArm.Point1.gameObject, spine_1.GoalTransform.gameObject);
            InstallJoint(rightArm.Point1.gameObject, spine_1.GoalTransform.gameObject);
        }

        private void InstallJoint(GameObject from, GameObject to)
        {
            var joint = from.AddComponent<ConfigurableJoint>();
            joint.connectedBody = to.GetComponent<Rigidbody>();
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
        }
        
        private void InstallBox(BoxPart part)
        {
            GameObject colliderGO;

            if (part.GoalTransform.Find(COLLIDER_NAME) == null)
                colliderGO = new GameObject(COLLIDER_NAME);
            else
                colliderGO = part.GoalTransform.Find(COLLIDER_NAME).gameObject;

            var colliderTransform = colliderGO.transform;
            colliderTransform.SetParent(part.GoalTransform);
            colliderTransform.position = part.GoalTransform.position + part.Offset;
            var eulers = part.GoalTransform.eulerAngles;
            eulers += part.Rotation;
            colliderTransform.eulerAngles = eulers;

            var collider = colliderGO.AddComponent<BoxCollider>();
            collider.size = part.Size;

            part.GoalTransform.gameObject.AddComponent<Rigidbody>();
        }

        private void ClearBox(BoxPart part)
        {
            var colliderTransform = part.GoalTransform.Find(COLLIDER_NAME);
            if (colliderTransform == null)
                return;

            var colliderGO = colliderTransform.gameObject;
            DestroyImmediate(colliderGO);
        }

        private void InstallCapsule(Transform start, Transform end, float radius)
        {
            if (start == null)
                return;

            if (end == null)
                return;

            var length = Vector3.Distance(start.position, end.position);

            BoxCollider collider;

            if (start.gameObject.TryGetComponent(out collider) == false)
            {
                collider = start.gameObject.AddComponent<BoxCollider>();
            }

            collider.size = new Vector3(radius * 2f, length, radius * 2f);
            collider.center = (length / 2f) * Vector3.up;

            start.gameObject.AddComponent<Rigidbody>();
        }

        private void ClearCapsule(Transform start)
        {
            if (start == null)
                return;

            if (start.TryGetComponent(out BoxCollider collider) == false)
                return;

            DestroyImmediate(collider);

            var rigidbody = start.GetComponent<Rigidbody>();
            if (rigidbody != null)
                DestroyImmediate(rigidbody);
        }

        [Button]
        private void Clear()
        {
            var js = GetComponentsInChildren<Joint>();
            for (var idx = js.Length - 1; idx >= 0; idx--)
            {
                var j = js[idx];
                DestroyImmediate(j);
            }

            var rs = GetComponentsInChildren<Rigidbody>();
            for (var idx = rs.Length - 1; idx >= 0; idx--)
            {
                var r = rs[idx];
                DestroyImmediate(r);
            }

            ClearBox(pelvis);

            ClearBox(spine_1);
            ClearBox(spine_2);
            ClearBox(spine_3);

            ClearBox(head);

            ClearLimb(leftLeg);
            ClearLimb(rightLeg);

            ClearLimb(leftArm);
            ClearLimb(rightArm);

            var joints = GetComponentsInChildren<RagdollJoint>();

            for (var idx = joints.Length - 1; idx >= 0; idx--)
            {
                var component = joints[idx];
                DestroyImmediate(component);
            }

            var parts = GetComponentsInChildren<RagdollPart>();

            for (var idx = parts.Length - 1; idx >= 0; idx--)
            {
                var component = parts[idx];
                DestroyImmediate(component);
            }

            var ragdoll = GetComponentInChildren<ShafirRagdoll>();
            if (ragdoll != null)
                DestroyImmediate(ragdoll);
        }

        #region Gizmos

        private void OnDrawGizmos()
        {
            if (drawGizmos == false)
                return;

            DrawCube(pelvis, pelvisColor);
            DrawCube(spine_1, spineColor);
            DrawCube(spine_2, spineColor);
            DrawCube(spine_3, spineColor);
            DrawCube(head, headColor);

            DrawJoint(spine_1.GoalTransform);
            DrawJoint(spine_2.GoalTransform);
            DrawJoint(spine_3.GoalTransform);
            DrawJoint(head.GoalTransform);

            DrawLimb(leftLeg, legsColor);
            DrawLimb(rightLeg, legsColor);

            DrawLimb(leftArm, armsColor);
            DrawLimb(rightArm, armsColor);
        }

        private void DrawLimb(LimbData data, Color color)
        {
            if (data.Point1 == null)
                return;

            if (data.Point2 == null)
                return;

            if (data.Point3 == null)
                return;

            if (data.End == null)
                return;

            DrawCapsule(data.Point1, data.Point2, data.Radius, color);
            DrawCapsule(data.Point2, data.Point3, data.Radius, color);
            DrawCube(data.End, color);

            DrawJoint(data.Point1);
            DrawJoint(data.Point2);
            DrawJoint(data.Point3);
        }

        private void DrawJoint(Transform goalTransform)
        {
            if (goalTransform == null)
                return;

            DrawSphere(goalTransform.position, jointsRadius, jointsColor);
        }

        private void DrawSphere(Vector3 pos, float radius, Color color)
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = color;
            Gizmos.DrawSphere(pos, radius);
            Gizmos.DrawWireSphere(pos, radius);
        }

        private void DrawCapsule(Transform start, Transform end, float radius, Color color)
        {
            var pos = (start.position + end.position) / 2f;
            var rot = Quaternion.LookRotation(end.position - start.position);
            var length = Vector3.Distance(start.position, end.position);
            var doubleRadius = radius * 2;
            var scale = new Vector3(doubleRadius, doubleRadius, length);
            var matrix = Matrix4x4.TRS(pos, rot, scale);
            Gizmos.matrix = matrix;
            Gizmos.color = color;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

        private void DrawCube(BoxPart box, Color color)
        {
            if (box.GoalTransform == null)
                return;

            var pos = box.GoalTransform.position;
            pos += box.Offset;
            var eulers = box.GoalTransform.eulerAngles;
            eulers += box.Rotation;
            var rot = Quaternion.Euler(eulers);
            var scale = box.Size;
            var matrix = Matrix4x4.TRS(pos, rot, scale);

            Gizmos.matrix = matrix;
            Gizmos.color = color;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

        #endregion
    }
}