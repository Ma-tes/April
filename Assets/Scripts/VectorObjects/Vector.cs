using System;
using UnityEngine;

namespace Assets.Scripts
{
    internal abstract class Vector<T, TOutPut> : IVector where T : IEquatable<T>, IFormattable where TOutPut : Vector<T, TOutPut>, new() // The orignal vector
    {
        public float x { get; set; }

        public float y { get; set; }

        public float z { get; set; }

        protected abstract T GetT();

        protected TOutPut SetT(T vector) 
        {
            var newVector = (Vector3)Convert.ChangeType(vector, typeof(T));
            return new TOutPut() {x = newVector.x, y = newVector.y, z = newVector.z };
        }

        public static implicit operator T(Vector<T, TOutPut> vector) { return vector.GetT(); }
    }
}
