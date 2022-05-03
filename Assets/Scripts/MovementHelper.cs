using System.Collections.Generic;
using System.Linq;
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

        private GameObject CopyMoveableObject;

        public Camera objectCamera { get; set; }

        [SerializeField]
        public float CurrentSpeed;

        [SerializeField]
        public float RotationSpeed;

        [SerializeField]
        public float MaxSpeed;

        public bool entityRotate { get; set; } = true;

        public List<AnimationSelecter<float, Animator>> Animations = new List<AnimationSelecter<float, Animator>>();

        public IEnumerable<Direction> CurrentDirection { get; set; }

        private IEnumerable<Direction> lastDirection { get; set; }

        public void Start()
        {
            CopyMoveableObject = new GameObject();
        }

        public void Update()
        {
            CurrentDirection = GetDirection(new KeyValuePair<KeyCode, Direction>[]
            {
               new KeyValuePair<KeyCode, Direction>(KeyCode.W, Direction.NORTH),
               new KeyValuePair<KeyCode, Direction>(KeyCode.S, Direction.SOUTH),
               new KeyValuePair<KeyCode, Direction>(KeyCode.A, Direction.WEST),
               new KeyValuePair<KeyCode, Direction>(KeyCode.D, Direction.EAST),
            });
            CurrentSpeed = CurrentDirection.Count() != 0 ? CurrentSpeed + (((MaxSpeed - CurrentSpeed) / 10) * Time.deltaTime) : ((CurrentSpeed - ((CurrentSpeed /  2)) * (Time.deltaTime * 10)));
            Quaternion entityRotation = MoveableObject.transform.rotation;
            Quaternion cameraQuaternion = objectCamera.transform.rotation;
            foreach (Direction direction in CurrentDirection) 
            {
                if (direction != Direction.NONE && CurrentDirection != lastDirection) //For now it's redudant
                {
                    var yAngle = objectCamera.transform.rotation.eulerAngles.y >= 180 ? objectCamera.transform.rotation.eulerAngles.y - 360 : objectCamera.transform.rotation.eulerAngles.y;
                    var cameraRotation = Quaternion.Euler(0, yAngle - (float)direction, 0);
                    //Debug.Log($"cameraRotation: {cameraRotation}");
                    float xAngle = entityRotation.eulerAngles.x;
                    entityRotation = Quaternion.Lerp(entityRotation, cameraRotation, RotationSpeed * Time.deltaTime);
                    entityRotation = Quaternion.Euler(xAngle, entityRotation.eulerAngles.y, entityRotation.eulerAngles.z);
                    if (entityRotate)
                        MoveableObject.transform.rotation = entityRotation;
                    cameraQuaternion = cameraRotation;
                }
            }
            entityRotation = entityRotate ? entityRotation : cameraQuaternion;
            //Debug.Log($"entityRotation: {entityRotation * Vector3.forward} forward: {MoveableObject.transform.forward}");
            MoveableObject.transform.position += ((entityRotation * Vector3.forward)) * (CurrentSpeed * Time.deltaTime);
            lastDirection = CurrentDirection;
        }

        private IEnumerable<Direction> GetDirection(KeyValuePair<KeyCode, Direction>[] keyValues) 
        {
            for (int i = 0; i < keyValues.Length; i++)
            {
                if (Input.GetKey(keyValues[i].Key)) 
                {
                    yield return keyValues[i].Value;
                }
            }
        }
    }
}
