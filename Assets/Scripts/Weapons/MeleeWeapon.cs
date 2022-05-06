using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal sealed class MeleeWeapon : StandardWeapon
    {
        [Range(0, 100)]
        [SerializeField]
        private float range;

        private const int CURVE_SCALER = 2;

        public sealed override KeyValuePair<KeyCode, WeaponAction>[] Actions => new KeyValuePair<KeyCode, WeaponAction>[]
        {
            new KeyValuePair<KeyCode, WeaponAction>(KeyCode.E, WeaponAction.attack)
        };

        public override IEnumerable<Vector3> ActionMoves(WeaponAction action)
        {
            if (action == WeaponAction.attack) 
            {
                for (int i = -(int)range; i < (int)range; i++)
                {
                    float curveValue = CalculateCurve(i, range);
                    var currentPoint = WeaponModel.transform.position + (WeaponModel.transform.forward * curveValue);
                    currentPoint = currentPoint + (WeaponModel.transform.right * i);
                    currentPoint.y = 0;
                    yield return currentPoint;
                }
            }
        }

        private float CalculateCurve(int index, float range) 
        {
            float exponent = ((index) * (index)) * -1;
            return Mathf.Pow(CURVE_SCALER, exponent) + exponent + range;
        }
    }
}
