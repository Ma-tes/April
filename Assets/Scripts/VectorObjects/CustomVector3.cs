using UnityEngine;

namespace Assets.Scripts
{
    internal sealed class CustomVector3 : Vector<Vector3, CustomVector3>
    {
        protected override Vector3 GetT() => new Vector3(x, y, z);

        public static implicit operator CustomVector3(Vector3 vector) { return (CustomVector3)new CustomVector3().SetT(vector); }
    }
}
