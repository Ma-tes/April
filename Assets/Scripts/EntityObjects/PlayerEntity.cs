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
            Vector3 stepPosition = -difference;
            if (((int)CalculateIndexer(xAxis) == lastIndexer && Mathf.Abs(stepPoint) < 1))
            {
                float mouseSensivity = xAxis; //TODO: multiply by real mouse sensivity and virtual in once
                stepPosition = new Vector3(difference.x * (Math.Abs(mouseSensivity) * -1), -difference.y, difference.z * (Math.Abs(mouseSensivity) * -1));
                stepPoint += mouseSensivity;
            }
            else 
            {
                int pointer = xAxis != 0 ? (int)CalculateIndexer(xAxis) : 0;
                int cameraPointsLength = cameraPointer.ValidMoveablePoints.Length;
                pathIndexer = (pathIndexer + pointer) >= cameraPointsLength || (pathIndexer + pointer) < 0 ? cameraPointsLength - Math.Abs(pathIndexer + pointer) : (pathIndexer + pointer);
                stepPoint = 0;
                lastIndexer = (int)pointer;
            }
                viewCamera.gameObject.transform.position = Vector3.MoveTowards(viewCamera.gameObject.transform.position, viewCamera.gameObject.transform.position + stepPosition, 1f);

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
            Debug.Log(this.gameObject.transform.rotation);
        }

        public static float CalculateIndexer(float value) => (Mathf.Abs(value) / value);
    }
}
