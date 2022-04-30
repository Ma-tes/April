using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal abstract class ObjectAction : MonoBehaviour, IKeyAction
    {
        [SerializeField]
        public GameObject Target;

        [SerializeField]
        public GameObject Entity;

        public abstract KeyCode Key { get; }

        protected Vector3 Difference { get; set; }

        protected virtual Vector3 offset => new Vector3(3, 3, 3);

        protected bool isVisible = true;

        private Vector3[] moveSteps { get; set; } = new Vector3[0];

        private uint indexer = 0;

        protected bool isCollide = false;

        public virtual void Update() 
        {
            Difference = Entity.transform.position - Target.transform.position;

            if (Input.GetKeyDown(Key) && (Math.Abs(Difference.x) < offset.x) && (Math.Abs(Difference.z) < offset.z) && isCollide == false) 
            {
                moveSteps = ActionMoves().ToArray();
                indexer = 0;
            }

            if (moveSteps.Count() > 0 && indexer < moveSteps.Count()) 
            {
                var stepMove = moveSteps[indexer];
                Vector3 difference = Entity.transform.position - stepMove;
                if ((int)difference.magnitude == 0)
                    indexer++;
                else
                {
                    var entityRigidBody = Entity.GetComponent<Rigidbody>();
                    float yIndexer = PlayerEntity.CalculateIndexer(difference.y);
                    if (difference.y != 0)
                        entityRigidBody.AddForceAtPosition(new Vector3(0, (yIndexer) * -1, 0) * 5, new Vector3(0, stepMove.y, 0));
                    Entity.transform.position = Vector3.MoveTowards(Entity.transform.position, stepMove, Time.deltaTime * 5);
                }
            }
        }

        public virtual IEnumerable<Vector3> ActionMoves() 
        {
            isVisible = (Math.Abs(Difference.z) <= offset.z) && (Math.Abs(Difference.x) <= offset.x && Difference.y <= offset.y);
            yield break;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<PlayerEntity>() != null)
                isCollide = true;
            else
                isCollide = false;
        }

        private void OnCollisionExit(Collision collision) => isCollide = false;
    }
}
