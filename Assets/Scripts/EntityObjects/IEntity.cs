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
        public string Name { get; }

        public uint Health { get; set; }

        public float Speed { get; set; }
    }
}
