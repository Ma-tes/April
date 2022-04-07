using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts 
{
    public class EntityManager : MonoBehaviour
    {
        public TypeSelecter<IEntity>[] typeSelecter = new TypeSelecter<IEntity>[] {new TypeSelecter<IEntity>(typeof(PlayerEntity) )};
    }
} 
