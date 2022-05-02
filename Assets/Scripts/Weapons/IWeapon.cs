using UnityEngine;

namespace Assets.Scripts
{
    enum WeaponAction 
    {
        attack = 0,
        reload = 1,
        none = 2
    }

    internal interface IWeapon
    {
        public GameObject WeaponModel { get; } 

        public uint Streght { get; }

        public AnimationSelecter<WeaponAction, Animator>[] CurrentAnimation { get; } // This would be an Animator
    }
}
