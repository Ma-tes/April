using System;
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

    enum MovementType 
    {
        WALK = 0,
        SPRINT = 1,
        NONE = 2
    }

    internal sealed class MovementHelper : MonoBehaviour
    {
        public GameObject MoveableObject { get; set; }

        public Camera ObjectCamera { get; set; }

        [SerializeField]
        public float CurrentSpeed;

        [SerializeField]
        public float RotationSpeed;

        [SerializeField]
        public float MaxSpeed;

        public bool EntityRotate { get; set; } = true;

        public List<AnimationSelecter<MovementType, AnimationClip>> Animations = new List<AnimationSelecter<MovementType, AnimationClip>>();

        public IEnumerable<Direction> CurrentDirections { get; set; }

        private IEnumerable<Direction> lastDirections { get; set; }

        private MovementType lastMovementType { get; set; } = MovementType.NONE;

        public void Update()
        {
            CurrentDirections = GetDirection(new KeyValuePair<KeyCode, Direction>[]
            {
               new KeyValuePair<KeyCode, Direction>(KeyCode.W, Direction.NORTH),
               new KeyValuePair<KeyCode, Direction>(KeyCode.S, Direction.SOUTH),
               new KeyValuePair<KeyCode, Direction>(KeyCode.A, Direction.WEST),
               new KeyValuePair<KeyCode, Direction>(KeyCode.D, Direction.EAST),
            });

            CurrentSpeed = CurrentDirections.Count() != 0 ? CurrentSpeed + (((MaxSpeed - CurrentSpeed) / 10) * Time.deltaTime) : ((CurrentSpeed - ((CurrentSpeed /  2)) * (Time.deltaTime * 10)));
            var movementType = CurrentSpeed > 4 ? MovementType.SPRINT : MovementType.WALK;
            //Debug.Log(movementType);

            Quaternion entityRotation = MoveableObject.transform.rotation;
            Quaternion cameraQuaternion = ObjectCamera.transform.rotation;

            foreach (Direction direction in CurrentDirections) 
            {
                if (direction != Direction.NONE && CurrentDirections != lastDirections)
                {

                    if (movementType != lastMovementType) 
                    {
                        var animationClip = AnimationSelecter<MovementType, AnimationClip>.GetCurrentAnimation(Animations, movementType);
                        animationClip.Output.legacy = true;
                        AnimationSelecter.PlayAnimationClip(animationClip.Entity, animationClip.Output, WrapMode.PingPong);
                    }

                    var yAngle = ObjectCamera.transform.rotation.eulerAngles.y >= 180 ? ObjectCamera.transform.rotation.eulerAngles.y - 360 : ObjectCamera.transform.rotation.eulerAngles.y;
                    var cameraRotation = Quaternion.Euler(0, yAngle - (float)direction, 0);
                    float xAngle = entityRotation.eulerAngles.x;
                    entityRotation = Quaternion.Lerp(entityRotation, cameraRotation, RotationSpeed * Time.deltaTime);
                    entityRotation = Quaternion.Euler(xAngle, entityRotation.eulerAngles.y, entityRotation.eulerAngles.z);
                    if (EntityRotate)
                        MoveableObject.transform.rotation = entityRotation;
                    cameraQuaternion = cameraRotation;
                }
                lastMovementType = movementType;
            }

            entityRotation = EntityRotate ? entityRotation : cameraQuaternion;
            MoveableObject.transform.position += ((entityRotation * Vector3.forward)) * (CurrentSpeed * Time.deltaTime);
            lastDirections = CurrentDirections;
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
