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

        public void Start()
        {
            cameraPointer = new CameraPointer(ViewCamera, 3);
        }

        public void Update()
        {
            //ViewCamera.transform.position += CalculateCameraPosition(new Vector3(25, 0, 25));
            cameraPointer.ValidMoveablePoints = cameraPointer.GetValidPoints(this.gameObject).ToArray();
            for (int i = 0; i < cameraPointer.ValidMoveablePoints.Length - 1; i++)
            {
                Debug.DrawLine(cameraPointer.ValidMoveablePoints[i], cameraPointer.ValidMoveablePoints[i + 1], Color.red);
                Debug.Log(cameraPointer.ValidMoveablePoints[i].ToString());
            }
            if (ViewCamera.gameObject.transform.position.Equals(cameraPointer.ValidMoveablePoints[pathIndexer]))
                pathIndexer = pathIndexer + 1 == cameraPointer.ValidMoveablePoints.Length ? 0 : pathIndexer + 1;
            else
                ViewCamera.gameObject.transform.position = Vector3.MoveTowards(ViewCamera.gameObject.transform.position, cameraPointer.ValidMoveablePoints[pathIndexer], Speed * Time.deltaTime);
            ViewCamera.gameObject.transform.rotation = Quaternion.LookRotation(-ViewCamera.gameObject.transform.position, this.gameObject.transform.position);
        }
    }
}
