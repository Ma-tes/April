using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string Name { get; }

        public uint Health { get; set; }

        public Vector3 Position { get; set; }

        public float Speed { get; set; } = 3.25f;

        private CameraPointer cameraPointer;

        private int pathIndexer = 0;

        private int futureStep { get; set; } = 0;

        private float lastRotaionIndex = 1;

        private float mouseAcceleration { get; set; } = 1;

        public void Start()
        {
            cameraPointer = new CameraPointer(ViewCamera, 3);
        }

        public void Update()
        {
            var xAxis = Input.GetAxis("Mouse X");
            var yAxis = Input.GetAxis("Mouse Y");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //ViewCamera.transform.position += CalculateCameraPosition(new Vector3(25, 0, 25));
            cameraPointer.ValidMoveablePoints = cameraPointer.GetValidPoints(this.gameObject).ToArray();
            for (int i = 0; i < cameraPointer.ValidMoveablePoints.Length - 1; i++)
            {
                Debug.DrawLine(cameraPointer.ValidMoveablePoints[i], cameraPointer.ValidMoveablePoints[i + 1], Color.red);
                Debug.Log(cameraPointer.ValidMoveablePoints[i].ToString());
            }
            if (futureStep == 0)
            {
                if (xAxis != 0) 
                {
                    float difference = xAxis;
                    float differencePointer = CalculateIndexer(difference);
                    mouseAcceleration = difference;
                    futureStep = (((((int)differencePointer) * ((int)Mathf.Abs((1 * differencePointer) + mouseAcceleration)))));
                }
            }
            else 
            {
                float pointer = CalculateIndexer(futureStep);
                int futureIndexSquared = pathIndexer + (int)pointer;
                pathIndexer = futureIndexSquared >= cameraPointer.ValidMoveablePoints.Length || futureIndexSquared <= -1 ? (cameraPointer.ValidMoveablePoints.Length - Math.Abs(futureIndexSquared)) : futureIndexSquared;
                ViewCamera.gameObject.transform.position = Vector3.MoveTowards(ViewCamera.gameObject.transform.position, cameraPointer.ValidMoveablePoints[pathIndexer], 0.33f + (((Math.Abs(xAxis * Time.deltaTime)) + Math.Abs(mouseAcceleration))));
                futureStep = futureStep + ((int)pointer * -1);
            }
            Debug.Log($"xDifference: {xAxis} mouseAcceleration: {mouseAcceleration} futureStep: {futureStep} pathIndexer: {pathIndexer}");
            var cameraRotation = Quaternion.LookRotation(this.gameObject.transform.position - ViewCamera.gameObject.transform.position);
            float rotationIndex = (1f * Time.deltaTime);
            ViewCamera.gameObject.transform.rotation = Quaternion.Lerp(cameraRotation, this.gameObject.transform.rotation, (1f * Time.deltaTime) * lastRotaionIndex);
            lastRotaionIndex = rotationIndex;
            //ViewCamera.gameObject.transform.rotation = Quaternion.FromToRotation(ViewCamera.gameObject.transform.position, this.gameObject.transform.position);
        }
        public float CalculateIndexer(float value) => (Mathf.Abs(value) / value);

    }
}
