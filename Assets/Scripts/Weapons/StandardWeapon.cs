using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal abstract partial class StandardWeapon : IWeapon 
    {
        public GameObject WeaponModel => weaponModel;

        public uint Stregth => stregth;

        public AnimationSelecter<WeaponAction, AnimationClip>[] CurrentAnimation => currentAnimation;
    }

    internal abstract partial class StandardWeapon : MonoBehaviour, IKeysAction<WeaponAction> 
    {
        [SerializeField]
        private GameObject weaponModel;

        [SerializeField]
        private uint stregth;

        [SerializeField]
        private AnimationSelecter<WeaponAction, AnimationClip>[] currentAnimation;

        [SerializeField]
        public DamagePoint DamagePoint;

        public abstract KeyValuePair<KeyCode, WeaponAction>[] Actions { get; }

        public void Update() 
        {
            var currentAction = GetWeaponAction(Actions);

            if (currentAction != WeaponAction.none) 
            {
                if (CurrentAnimation.Length != 0) 
                {
                    var currentAnimation = AnimationSelecter<WeaponAction, AnimationClip>.GetCurrentAnimation(CurrentAnimation, currentAction);
                    currentAnimation.Output.legacy = true;
                    AnimationSelecter.PlayAnimationClip(currentAnimation.Entity, currentAnimation.Output);
                }

                var attackPoints = ActionMoves(currentAction);
                var newDemagePoints = Instantiate(DamagePoint);
                newDemagePoints.VectorPoints.Vectors = new CustomVector3[attackPoints.Count()];
                newDemagePoints.GameObject = Instantiate(DamagePoint.GameObject);
                newDemagePoints.GameObject.transform.position = WeaponModel.transform.position;

                int i = 0;
                foreach (var attactPoint in attackPoints) 
                {
                    newDemagePoints.VectorPoints.Vectors[i] = attactPoint;
                    Debug.DrawLine(attactPoint, new Vector3(attactPoint.x, attactPoint.y + 1, attactPoint.z), Color.blue, 100);
                    i++;
                }
            }
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
