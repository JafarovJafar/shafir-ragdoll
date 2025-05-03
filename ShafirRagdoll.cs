using System.Collections.Generic;
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

        [SerializeField] private RagdollPart pelvis;

        [SerializeField] private RagdollPart lThigh;
        [SerializeField] private RagdollPart lAnkle;
        [SerializeField] private RagdollPart lFoot;

        [SerializeField] private RagdollPart rThigh;
        [SerializeField] private RagdollPart rAnkle;
        [SerializeField] private RagdollPart rFoot;

        [SerializeField] private RagdollPart spine1;
        [SerializeField] private RagdollPart spine2;
        [SerializeField] private RagdollPart spine3;

        [SerializeField] private RagdollPart lArm;
        [SerializeField] private RagdollPart lForearm;
        [SerializeField] private RagdollPart lWrist;

        [SerializeField] private RagdollPart rArm;
        [SerializeField] private RagdollPart rForearm;
        [SerializeField] private RagdollPart rWrist;

        [SerializeField] private RagdollPart head;

        private Dictionary<RagdollPartType, RagdollPart> _partsDict;

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

            _partsDict = new Dictionary<RagdollPartType, RagdollPart>();
            _partsDict.Add(RagdollPartType.Pelvis, pelvis);

            _partsDict.Add(RagdollPartType.ThighLeft, lThigh);
            _partsDict.Add(RagdollPartType.AnkleLeft, lAnkle);
            _partsDict.Add(RagdollPartType.FootLeft, lFoot);

            _partsDict.Add(RagdollPartType.ThighRight, rThigh);
            _partsDict.Add(RagdollPartType.AnkleRight, rAnkle);
            _partsDict.Add(RagdollPartType.FootRight, rFoot);

            _partsDict.Add(RagdollPartType.Spine1, spine1);
            _partsDict.Add(RagdollPartType.Spine2, spine2);
            _partsDict.Add(RagdollPartType.Spine3, spine3);

            _partsDict.Add(RagdollPartType.ArmLeft, lArm);
            _partsDict.Add(RagdollPartType.ForearmLeft, lForearm);
            _partsDict.Add(RagdollPartType.WristLeft, lWrist);

            _partsDict.Add(RagdollPartType.ArmRight, rArm);
            _partsDict.Add(RagdollPartType.ForearmRight, rForearm);
            _partsDict.Add(RagdollPartType.WristRight, rWrist);

            _partsDict.Add(RagdollPartType.Head, head);
        }

        public RagdollPart GetPart(RagdollPartType partType)
        {
            return _partsDict[partType];
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

            if (animator == null)
                return;

            animator.enabled = !isEnabled;
        }
    }
}