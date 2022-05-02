using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal sealed class JumpAction : ObjectAction
    {
        public override KeyCode Key => KeyCode.Space;

        public sealed override IEnumerable<CustomVector3> ActionMoves()
        {
            IEnumerator<CustomVector3> enumerator = base.ActionMoves().GetEnumerator();
            enumerator.MoveNext();

            var targetCollider = Target.GetComponent<Collider>();

            for (int i = 0; i < 3; i++)
            {
                Vector3 forwardDirection = Math.Abs(Difference.z) > Math.Abs(Difference.x) ? new Vector3(0, 0, 1) : new Vector3(1, 0, 0);
                float zAxisStep = CalculateDifferenceLength(Difference.z, targetCollider.bounds.size.z, (3 - (i - 1))) * forwardDirection.z;
                float xAxisStep = CalculateDifferenceLength(Difference.x, targetCollider.bounds.size.x, (3 - (i - 1))) * forwardDirection.x;
                if (isVisible == false)
                    break;
                yield return new Vector3((Entity.transform.position.x - xAxisStep), ((targetCollider.bounds.size.y * 3) / (3 - i)), (Entity.transform.position.z - zAxisStep));
            }
        }

        private float CalculateDifferenceLength(float difference, float targetSize, float devider = 1) 
        {
            if (devider == 0)
                throw new DivideByZeroException();
            float properSize = (PlayerEntity.CalculateIndexer(difference) * targetSize);
            return (difference + properSize) / devider;
        }
    }
}
