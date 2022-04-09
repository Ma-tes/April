using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal sealed class CameraPointer
    {
        public Camera DefaultCamera { get; private set; }

        private float offset { get; set; }

        public Vector3[] ValidMoveablePoints { get; set; }

        private const int CIRCLE_FACE = 4;

        public CameraPointer(Camera camera, float offset) 
        {
            DefaultCamera = camera;
            this.offset = offset;
        }

        private Vector3[] CalculateCombinations(Vector3[] nodes) 
        {
            var combinations = new Vector3[nodes.Length * CIRCLE_FACE];
            for (int i = 0; i < nodes.Length; i++)
            {
                float[] numbers = { nodes[i].x, nodes[i].z };
                for (int j = 0; j < CIRCLE_FACE; j++)
                {
                    int indexer = j >= numbers.Length ? j - numbers.Length : j;
                    int combinationIndexer = (i * nodes.Length) + j;
                    numbers[indexer] = numbers[indexer] * -1;
                    combinations[combinationIndexer] = new Vector3(numbers[0], nodes[i].y, numbers[1]);
                }
            }
            return combinations;
        }

        public IEnumerable<Vector3> GetValidPoints(GameObject @object) 
        {
            Vector3[] moveCombinations = CalculateCombinations(new Vector3[] { new Vector3(-1, 0, 1) }); // I would be something like [1, 1] [1, -1] [-1, 1] [-1, -1]
            float[] pointAxis = new float[2] {0, offset };
            for (int i = 0; i < moveCombinations.Length; i++)
            {
                int scaler = 3;
                float dotProduct = ((offset * scaler) / offset) * scaler;
                for (int j = 0; j < offset * scaler; j++)
                {
                    pointAxis[0] += ((j) * (moveCombinations[i].x)) / dotProduct;
                    pointAxis[1] += ((j) * (moveCombinations[i].z)) / dotProduct;
                    yield return new Vector3(((@object.gameObject.transform.position.x - (offset)) - @object.gameObject.transform.localScale.x) + pointAxis[0], @object.gameObject.transform.position.y, (@object.gameObject.transform.position.z - (offset)) + pointAxis[1]);
                }
            }
        }
    }
}
