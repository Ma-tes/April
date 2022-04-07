using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IEntity
    {
        [SerializeField]
        public string Name { get; }

        [SerializeField]
        public uint Health { get; set; }

        public Vector3 Position { get; set; }
    }
}
