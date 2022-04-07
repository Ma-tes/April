using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class PlayerEntity : MonoBehaviour, IEntity
    {
        [SerializeField]
        public string Name { get; }

        [SerializeField]
        public string Description;

        public uint Health { get; set; }

        public Vector3 Position { get; set; }

        public void Update()
        {

        }
    }
}
