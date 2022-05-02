using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal abstract partial class StandardWeapon : IWeapon 
    {
        public GameObject WeaponModel => weaponModel;

        public uint Streght => streght;

        public AnimationSelecter<WeaponAction, Animator>[] CurrentAnimation => currentAnimation;
    }
    internal abstract partial class StandardWeapon : MonoBehaviour, IKeysAction<WeaponAction> 
    {
        [SerializeField]
        private GameObject weaponModel;

        [SerializeField]
        private uint streght;

        [SerializeField]
        private AnimationSelecter<WeaponAction, Animator>[] currentAnimation;

        [SerializeField]
        public DemagePoint DemagePoint;

        public abstract KeyValuePair<KeyCode, WeaponAction>[] Actions { get; }

        public void Update() 
        {
            var currentAction = GetWeaponAction(Actions);
            //var currentAnimation = AnimationSelecter<WeaponAction, Animator>.GetAnimationOutput(CurrentAnimation, currentAction); //TODO: connect it to the entity 

            if (currentAction != WeaponAction.none) 
            {
                var attackPoints = ActionMoves(currentAction);
                var newDemagePoints = Instantiate(DemagePoint); //TODO: create better point system
                newDemagePoints.vectorPoints.Vectors = new CustomVector3[attackPoints.Count()];
                newDemagePoints.GameObject = Instantiate(DemagePoint.GameObject);
                newDemagePoints.GameObject.transform.position = WeaponModel.transform.position;

                int i = 0;
                foreach (var attactPoint in attackPoints) 
                {
                    newDemagePoints.vectorPoints.Vectors[i] = attactPoint;
                    Debug.DrawLine(attactPoint, new Vector3(attactPoint.x, attactPoint.y + 1, attactPoint.z), Color.blue, 100);
                    Debug.Log(attactPoint);
                    i++;
                }
            }
            //TODO: play animation to specify gameObject
        }

        public abstract IEnumerable<Vector3> ActionMoves(WeaponAction action);

        protected WeaponAction GetWeaponAction(KeyValuePair<KeyCode, WeaponAction>[] keyPairs) 
        {
            for (int i = 0; i < keyPairs.Length; i++)
            {
                if (Input.GetKeyDown(keyPairs[i].Key))
                    return keyPairs[i].Value;
            }
            return WeaponAction.none;
        }
    }
}
