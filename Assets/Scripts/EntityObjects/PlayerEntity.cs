using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField]
        public Camera ZoomCamera;

        [SerializeField]
        public GameObject DefaultObject;

        [SerializeField]
        public MovementHelper movementHelper;

        public string Name { get; }

        public uint Health { get; set; }

        public Vector3 Position { get; set; }

        public float Speed { get; set; } = 3.25f;

        private CameraPointer cameraPointer;

        private float lastRotaionIndex = 1;


        private int lastIndexer { get; set; }

        private int pathIndexer { get; set; } = 0;

        private float stepPoint = 0;

        private float xRotation { get; set; }
        private float yRotation { get; set; }

        public void Start()
        {
            cameraPointer = new CameraPointer(ViewCamera, 0.5f);
            movementHelper.MoveableObject = this.gameObject;
            movementHelper.objectCamera = ViewCamera;
        }

        public void Update()
        {
             var xAxis = Input.GetAxis("Mouse X");
             var yAxis = Input.GetAxis("Mouse Y");
             Cursor.lockState = CursorLockMode.Locked;
             Cursor.visible = false;

             cameraPointer.ValidMoveablePoints = cameraPointer.GetValidPoints(this.gameObject, lineLength: 5, yAxis: 2).ToArray();

            Vector3 difference = ViewCamera.gameObject.transform.position - cameraPointer.ValidMoveablePoints[pathIndexer];
            Vector3 stepPosition = -difference;
            if (((int)CalculateIndexer(xAxis) == lastIndexer && Mathf.Abs(stepPoint) < 1))
            {
                float mouseSensivity = xAxis; //TODO: multiply by real mouse sensivity and virtual in once
                int[] pointIndexers = new int[]
                {
                    (int)CalculateIndexer(cameraPointer.ValidMoveablePoints[pathIndexer].x),
                    (int)CalculateIndexer(cameraPointer.ValidMoveablePoints[pathIndexer].z),
                };
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
                ViewCamera.gameObject.transform.position = Vector3.MoveTowards(ViewCamera.gameObject.transform.position, ViewCamera.gameObject.transform.position + stepPosition, 1f);
             //Camera rotation
             var cameraRotation = Quaternion.LookRotation(this.gameObject.transform.position - ViewCamera.gameObject.transform.position);
            ViewCamera.gameObject.transform.rotation = cameraRotation;

            if (Input.GetKey(KeyCode.Mouse1))
            {
                ViewCamera.enabled = false;
                movementHelper.entityRotate = false;
                movementHelper.objectCamera = ZoomCamera;
            }
            else 
            {
                ViewCamera.enabled = true;
                movementHelper.entityRotate = true;
                movementHelper.objectCamera = ViewCamera;
            }
            Debug.Log(this.gameObject.transform.rotation);
        }
        public static float CalculateIndexer(float value) => (Mathf.Abs(value) / value);
    }
}
