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
        public GameObject DefaultObject;

        [SerializeField]
        public MovementHelper movementHelper;

        public string Name { get; }

        public uint Health { get; set; }

        public Vector3 Position { get; set; }

        public float Speed { get; set; } = 3.25f;

        private CameraPointer cameraPointer;

        private float lastRotaionIndex = 1;

        private KeyCode lastKey { get; set; }

        private int lastIndexer { get; set; }

        private int pathIndexer { get; set; } = 0;

        private float stepPoint = 0;

        public void Start()
        {
            cameraPointer = new CameraPointer(ViewCamera, 0.5f);
            movementHelper.MoveableObject = this.gameObject;
            movementHelper.objectCamera = ViewCamera;
        }

        public async void LateUpdate()
        {
             if (Input.GetKeyDown(lastKey) == false) 
             {
             }

             var xAxis = Input.GetAxis("Mouse X");
             var yAxis = Input.GetAxis("Mouse Y");
             Cursor.lockState = CursorLockMode.Locked;
             Cursor.visible = false;
             cameraPointer.ValidMoveablePoints = cameraPointer.GetValidPoints(this.gameObject, 5).ToArray();
             Debug.Log(cameraPointer.ValidMoveablePoints.Length);
             for (int i = 0; i < cameraPointer.ValidMoveablePoints.Length - 1; i++)
             {
                 Debug.DrawLine(cameraPointer.ValidMoveablePoints[i], cameraPointer.ValidMoveablePoints[i + 1], Color.red);
                 //Debug.Log(cameraPointer.ValidMoveablePoints[i].ToString());
             }
            if (((int)CalculateIndexer(xAxis) == lastIndexer && Mathf.Abs(stepPoint) < 1))
            {
                float mouseSensivity = xAxis; //TODO: multiply by real mouse sensivity and virtual in once
                int[] pointIndexers = new int[]
                {
                    (int)CalculateIndexer(cameraPointer.ValidMoveablePoints[pathIndexer].x),
                    (int)CalculateIndexer(cameraPointer.ValidMoveablePoints[pathIndexer].z),
                };
                Vector3 difference = ViewCamera.gameObject.transform.position - cameraPointer.ValidMoveablePoints[pathIndexer];
                Vector3 stepPosition = new Vector3(difference.x * (Math.Abs(mouseSensivity) * -1), 0, difference.z * (Math.Abs(mouseSensivity) * -1));
                stepPoint += mouseSensivity;
                ViewCamera.gameObject.transform.position = Vector3.MoveTowards(ViewCamera.gameObject.transform.position, ViewCamera.gameObject.transform.position + stepPosition, 1f);
                Debug.Log($"mouseSensivity: {mouseSensivity} point[0]: {pointIndexers[0]} point[1]: {pointIndexers[1]} stepPoint: {stepPoint} stepPosition: {stepPosition} pathIndexer: {pathIndexer}");
            }
            else 
            {
                int pointer = xAxis != 0 ? (int)CalculateIndexer(xAxis) : 0;
                int cameraPointsLength = cameraPointer.ValidMoveablePoints.Length;
                pathIndexer = (pathIndexer + pointer) >= cameraPointsLength || (pathIndexer + pointer) < 0 ? cameraPointsLength - Math.Abs(pathIndexer + pointer) : (pathIndexer + pointer);
                stepPoint = 0;
                lastIndexer = (int)pointer;
            }
             //Camera rotation
             var cameraRotation = Quaternion.LookRotation(this.gameObject.transform.position - ViewCamera.gameObject.transform.position);
             float rotationIndex = (1f * Time.deltaTime);
             ViewCamera.gameObject.transform.rotation = Quaternion.Lerp(cameraRotation, this.gameObject.transform.rotation, (1f * Time.deltaTime) * lastRotaionIndex);
             lastRotaionIndex = rotationIndex;
        }
        public float CalculateIndexer(float value) => (Mathf.Abs(value) / value);
    }
}
