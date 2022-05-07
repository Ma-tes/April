using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal class PlayerEntity : MonoBehaviour, IEntity
    {

        [SerializeField]
        private string description;

        [SerializeField]
        private Camera viewCamera;

        [SerializeField]
        private Camera zoomCamera;

        [SerializeField]
        private MovementHelper movementHelper;

        private CameraPointer cameraPointer;

        public string Name { get; }

        public uint Health { get; set; }

        public float Speed { get; set; } = 3.25f;

        private int lastIndexer { get; set; }

        private int pathIndexer { get; set; } = 0;

        private float stepPoint = 0;

        public void Start()
        {
            cameraPointer = new CameraPointer(viewCamera, 0.5f);
            movementHelper.MoveableObject = this.gameObject;
            movementHelper.ObjectCamera = viewCamera;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Update()
        {
             var xAxis = Input.GetAxis("Mouse X");
             cameraPointer.ValidMoveablePoints = cameraPointer.GetValidPoints(this.gameObject, lineLength: 5, yAxis: 2).ToArray();

            Vector3 difference = viewCamera.gameObject.transform.position - cameraPointer.ValidMoveablePoints[pathIndexer];
            Vector3 stepPosition = difference; 
            //Debug.Log($"stepPoint: {stepPoint} xAxis: {xAxis} difference: {difference} magnitude: {difference.sqrMagnitude}");
            float mouseSensivity = (xAxis / 20); //TODO: multiply by real mouse sensivity and virtual in once
            Debug.Log(mouseSensivity);
            if (Math.Abs(difference.x) >= 0.8f && Math.Abs(difference.z) >= 0.8f)
            {
                Debug.Log($"difference: {difference} mouseSensivity: {mouseSensivity}");
                stepPoint += mouseSensivity;
                stepPosition = new Vector3(difference.x + (Math.Abs(mouseSensivity) * CalculateIndexer(difference.x)), 0, difference.z + (Math.Abs(mouseSensivity) * CalculateIndexer(difference.z)));
            }
            else 
            {
                int pointer = mouseSensivity != 0 ? (int)CalculateIndexer(mouseSensivity) : 0;
                int cameraPointsLength = cameraPointer.ValidMoveablePoints.Length;
                pathIndexer = ((pathIndexer + pointer) >= cameraPointsLength || (pathIndexer + pointer) < 0) ? cameraPointsLength - Math.Abs(pathIndexer + pointer) : (pathIndexer + pointer);
                stepPoint = 0.8f;
                lastIndexer = (int)pointer;
                Debug.Log("New pointer");
            }
            //viewCamera.gameObject.transform.position = Vector3.MoveTowards(viewCamera.gameObject.transform.position, viewCamera.gameObject.transform.position - stepPosition, 1f);
            var newNextPosiiton = viewCamera.gameObject.transform.position - (stepPosition * (0.1f + Time.fixedDeltaTime));
            viewCamera.gameObject.transform.position = Vector3.Lerp(viewCamera.gameObject.transform.position, newNextPosiiton, 1f);

             var cameraRotation = Quaternion.LookRotation(this.gameObject.transform.position - viewCamera.gameObject.transform.position);
            viewCamera.gameObject.transform.rotation = cameraRotation;

            if (Input.GetKey(KeyCode.Mouse1))
            {
                viewCamera.enabled = false;
                movementHelper.EntityRotate = false;
                movementHelper.ObjectCamera = zoomCamera;
            }
            else 
            {
                viewCamera.enabled = true;
                movementHelper.EntityRotate = true;
                movementHelper.ObjectCamera = viewCamera;
            }
        }

        public static float CalculateIndexer(float value) => (Mathf.Abs(value) / value);
    }
}
