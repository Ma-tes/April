using System;
using UnityEngine;

namespace Assets.Scripts
{
    internal abstract class Vector<T, TOutPut> : IVector where T : IEquatable<T>, IFormattable where TOutPut : Vector<T, TOutPut>, new() // The orignal vector
    {
        public float x { get; set; } = 0;

        public float y { get; set; } = 0;

        public float z { get; set; } = 0;

        protected abstract T GetT();

        protected TOutPut SetT(T vector) 
        {
            var newVector = (Vector3)Convert.ChangeType(vector, typeof(T));
            return new TOutPut() {x = newVector.x, y = newVector.y, z = newVector.z };
        }

        public static implicit operator T(Vector<T, TOutPut> vector) { return vector.GetT(); }
    }
}
