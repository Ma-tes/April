using UnityEngine;

namespace Assets.Scripts
{
    internal sealed class DamagePoint : MonoBehaviour
    {
        [SerializeField]
        public GameObject GameObject;

        public uint Damage { get; set; } = 0;

        public VectorCounter<CustomVector3> VectorPoints = new VectorCounter<CustomVector3>() { Vectors = new CustomVector3[]{ new CustomVector3() {x = 0, y = 0, z = 0 } }, Indexer = 0 };

        public void Update()
        {
            if (VectorPoints.Vectors.Length > 1) 
            {
                if (GameObject.transform.position != VectorPoints.Vectors[VectorPoints.Indexer]) 
                {
                    var difference = VectorPoints.Vectors[VectorPoints.Indexer] - GameObject.transform.position;
                    GameObject.transform.position += difference * (0.1f * Time.deltaTime);
                }
                else
                    VectorPoints.Indexer = VectorPoints.Indexer + 1 < VectorPoints.Vectors.Length ? VectorPoints.Indexer + 1 : 0;
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            var @objectComponent = collision.gameObject.GetComponent<IEntity>();
            if (@objectComponent != null)
                objectComponent.Health -= Damage;
        }
    }
}
