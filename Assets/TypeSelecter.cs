using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public sealed class TypeSelecter<T>
    {
        public string TypeName;

        public Type ?objectType { get; }

        public TypeSelecter(Type type) 
        {
            objectType = type;
            TypeName = type.Name;
        }
    }
}
