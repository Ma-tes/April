using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal sealed class DemagePoint : MonoBehaviour
    {
        [SerializeField]
        public GameObject GameObject;

        public uint Demage { get; set; } = 0;

        public VectorCounter<CustomVector3> vectorPoints = new VectorCounter<CustomVector3>() { Vectors = new CustomVector3[]{ new CustomVector3() {x = 0, y = 0, z = 0 } }, Indexer = 0 };

        public void Update()
        {
            if (vectorPoints.Vectors.Length > 1) 
            {
                if (GameObject.transform.position != vectorPoints.Vectors[vectorPoints.Indexer]) 
                {
                    var difference = vectorPoints.Vectors[vectorPoints.Indexer] - GameObject.transform.position;
                    GameObject.transform.position += difference * (0.1f * Time.deltaTime);
                }
                else
                    vectorPoints.Indexer = vectorPoints.Indexer + 1 < vectorPoints.Vectors.Length ? vectorPoints.Indexer + 1 : 0;
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            var @objectComponent = collision.gameObject.GetComponent<IEntity>();
            if (@objectComponent != null)
                objectComponent.Health -= Demage;
        }
    }
}
