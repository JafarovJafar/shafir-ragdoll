using EasyButtons;
using UnityEngine;

namespace Shafir.Ragdoll
{
    internal class RagdollInstaller : MonoBehaviour
    {
        [SerializeField] private BoxPart pelvis;
        [SerializeField] private BoxPart spine_1;
        [SerializeField] private BoxPart spine_2;
        [SerializeField] private BoxPart spine_3;
        [SerializeField] private BoxPart head;
        [SerializeField] private LimbData leftLeg;
        [SerializeField] private LimbData rightLeg;
        [SerializeField] private LimbData leftArm;
        [SerializeField] private LimbData rightArm;

        private readonly Color pelvisColor = Color.cyan;
        private readonly Color spineColor = Color.green;
        private readonly Color headColor = Color.magenta;
        private readonly Color legsColor = Color.blue;
        private readonly Color armsColor = Color.red;
        private readonly Color jointsColor = Color.yellow;

        private readonly float jointsRadius = 0.05f;

        [Button]
        private void Install()
        {
            
        }

        [Button]
        private void Clear()
        {
            
        }
        
        private void OnDrawGizmos()
        {
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
    }
}