using System;
using UnityEngine;

namespace Shafir.Ragdoll
{
    [Serializable]
    internal class BoxPart
    {
        public Transform GoalTransform;
        public Vector3 Offset;
        public Vector3 Rotation;
        public Vector3 Size;
    }
}