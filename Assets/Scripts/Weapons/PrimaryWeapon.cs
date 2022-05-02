using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    struct BulletsManager 
    {
        [SerializeField]
        public uint MaxBullets;

        [SerializeField]
        public uint CurrentBullets;

        public static BulletsManager operator -(BulletsManager bulletsManager, uint index = 1) 
        {
            uint newCurrentBullets = (bulletsManager.CurrentBullets - index) >= 0 ? bulletsManager.CurrentBullets - index : bulletsManager.MaxBullets;
            return new BulletsManager() { CurrentBullets = newCurrentBullets, MaxBullets = bulletsManager.MaxBullets };
        }
    }

    internal sealed class PrimaryWeapon : StandardWeapon
    {
        [SerializeField]
        public BulletsManager bulletsManager = new BulletsManager();

        [SerializeField]
        public float MaxShootDistance;

        public sealed override KeyValuePair<KeyCode, WeaponAction>[] Actions => new KeyValuePair<KeyCode, WeaponAction>[]
        {
            new KeyValuePair<KeyCode, WeaponAction>(KeyCode.Mouse0, WeaponAction.attack),
            new KeyValuePair<KeyCode, WeaponAction>(KeyCode.R, WeaponAction.reload)
        };

        public override IEnumerable<Vector3> ActionMoves(WeaponAction action)
        {
            if (action == WeaponAction.attack)
            {
                bulletsManager = bulletsManager - 1;
                foreach (var point in GetRelativePoints(WeaponModel.transform.forward, (int)MaxShootDistance, 1)) { yield return WeaponModel.transform.position + point; }
            }
        }

        private IEnumerable<Vector3> GetRelativePoints(Vector3 @objectForward, int length, int devider) 
        {
            int timer = length / devider;
            for (int i = 0; i < timer; i++)
            {
                yield return @objectForward * (timer - i);
            }
        }
    }
}
