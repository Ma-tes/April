using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    enum Direction 
    {
        NONE = 1, 
        NORTH = 0,
        SOUTH = 180,
        WEST = 90,
        EAST = -90 
    }

    internal sealed class MovementHelper : MonoBehaviour
    {
        public GameObject MoveableObject { get; set; }

        public Camera objectCamera { get; set; }

        [SerializeField]
        public float CurrentSpeed;

        [SerializeField]
        public float MaxSpeed;

        public List<AnimationSelecter<float, Animator>> Animations = new List<AnimationSelecter<float, Animator>>();

        public IEnumerable<Direction> CurrentDirection { get; set; }

        private IEnumerable<Direction> lastDirection { get; set; }

        private KeyCode lastKey { get; set; }

        public void Update()
        {
            CurrentDirection = GetDirection(new KeyValuePair<KeyCode, Direction>[]
            {
               new KeyValuePair<KeyCode, Direction>(KeyCode.W, Direction.NORTH),
               new KeyValuePair<KeyCode, Direction>(KeyCode.S, Direction.SOUTH),
               new KeyValuePair<KeyCode, Direction>(KeyCode.A, Direction.WEST),
               new KeyValuePair<KeyCode, Direction>(KeyCode.D, Direction.EAST),
            });
            foreach (Direction direction in CurrentDirection) 
            {
                if (direction != Direction.NONE) 
                {
                    var yAngle = objectCamera.transform.rotation.eulerAngles.y >= 180 ? objectCamera.transform.rotation.eulerAngles.y - 360 : objectCamera.transform.rotation.eulerAngles.y;
                    var cameraRotation = Quaternion.Euler(0, yAngle - (float)direction, 0);
                    Debug.Log($"current: {direction}");
                    if(lastDirection != CurrentDirection)
                        MoveableObject.transform.rotation = Quaternion.Lerp(MoveableObject.gameObject.transform.rotation, cameraRotation, 5f * Time.deltaTime);
                    MoveableObject.transform.position += MoveableObject.transform.forward * (CurrentSpeed * Time.deltaTime);
                }
            }
            lastDirection = CurrentDirection;
        }

        private IEnumerable<Direction> GetDirection(KeyValuePair<KeyCode, Direction>[] keyValues) 
        {
            for (int i = 0; i < keyValues.Length; i++)
            {
                if (Input.GetKey(keyValues[i].Key)) 
                {
                    lastKey = keyValues[i].Key;
                    yield return keyValues[i].Value;
                }
            }
        }
    }
}
