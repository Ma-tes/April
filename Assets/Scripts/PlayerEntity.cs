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
        public string Description;

        [SerializeField]
        public Camera ViewCamera;

        public string Name { get; }

        public uint Health { get; set; }

        public Vector3 Position { get; set; }

        public float Speed { get; set; } = 1.25f;

        public void Start()
        {
        }

        public void Update()
        {

            //ViewCamera.transform.position += CalculateCameraPosition(new Vector3(25, 0, 25));
        }

        private Vector3 CalculateCameraPosition(Vector3 nextPosition) 
        {
            Vector3 vectorDifference = AbsVector3Difference(gameObject.transform.position, nextPosition);
            return vectorDifference + (gameObject.transform.position - nextPosition);
        }
        private Vector3 AbsVector3Difference(Vector3 first, Vector3 second) 
        {
            first -= second;
            var difference = new Vector3(Math.Abs(first.x), Math.Abs(first.y), Math.Abs(first.z));
            return difference;
        }
    }
}
