using System;
using UnityEngine;

namespace Shafir.Ragdoll
{
    [Serializable]
    internal class LimbData
    {
        public Transform Point1;
        public Transform Point2;
        public Transform Point3;
        public float Radius;

        public BoxPart End;
    }
}