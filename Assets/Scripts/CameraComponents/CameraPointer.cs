using System.Collections.Generic;
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

        public IEnumerable<Vector3> GetValidPoints(GameObject @object, float lineLength, float yAxis = 0) 
        {
            Vector3[] moveCombinations = CalculateCombinations(new Vector3[] { new Vector3(-1, 0, 1)}); // It will be something like [1, 1] [1, -1] [-1, 1] [-1, -1]
            Vector3 lastPoint = new Vector3();
            for (int i = 0; i < moveCombinations.Length; i++)
            {
                int scaler = 25;
                float dotProduct = (lineLength / offset);
                float[] pointAxis = new float[2] {0, 0};
                for (int j = 0; j < dotProduct; j++)
                {
                    pointAxis[0] = lastPoint.x + ((j) * (moveCombinations[i].x * (offset)));
                    pointAxis[1] = lastPoint.z + ((j) * (moveCombinations[i].z * (offset)));
                    yield return new Vector3(((@object.gameObject.transform.position.x) - ((lineLength - (@object.gameObject.transform.localScale.x / 2)))) + pointAxis[0], @object.gameObject.transform.position.y + yAxis, (@object.gameObject.transform.position.z) + pointAxis[1]);
                }
                lastPoint = new Vector3(pointAxis[0], 0, pointAxis[1]);
            }
        }
    }
}
