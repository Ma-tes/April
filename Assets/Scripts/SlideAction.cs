using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal sealed class SlideAction : ObjectAction
    {
        public override KeyCode Key => KeyCode.LeftControl;

        protected override Vector3 offset => new Vector3(3, 1, 3);

        private Vector3 target { get; set; }

        public override void Update()
        {
            var entityMovementComponent = Entity.GetComponent<MovementHelper>();
            base.Update();
            if (vectorCounter.Indexer >= vectorCounter.Vectors.Length)
            {
                entityMovementComponent.enabled = true;
                Entity.transform.rotation = Quaternion.Euler(0, Entity.transform.eulerAngles.y, Entity.transform.eulerAngles.z);
            }
            else
                entityMovementComponent.enabled = false;
        }

        public override IEnumerable<CustomVector3> ActionMoves()
        {
            IEnumerator<CustomVector3> enumerator = base.ActionMoves().GetEnumerator();
            enumerator.MoveNext();

            var entityHeight = Entity.GetComponent<Collider>();
            var targetHeight = Target.GetComponent<Collider>();

            float yAngle = entityHeight.bounds.size.y + Target.transform.position.y;
            float distance = (entityHeight.bounds.size.y + targetHeight.bounds.size.z);
            if (isVisible) 
            {
                var newRotation = Quaternion.Euler((yAngle * -1.25f) * (Mathf.PI * Mathf.PI), Entity.transform.eulerAngles.y, Entity.transform.eulerAngles.z); //TODO: It is not a correct way to calculated angles
                Entity.transform.rotation = Quaternion.SlerpUnclamped(Entity.transform.rotation, newRotation, 1f);
                target = new Vector3(Entity.transform.position.x, Entity.transform.position.y, ((Entity.transform.position.z) - (PlayerEntity.CalculateIndexer(Difference.z) * (distance * 2))));
                yield return target;
            }
        }
    }
}
