using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    struct MouseSensivity 
    {
        [SerializeField]
        public uint xAxis;

        [SerializeField]
        public uint yAxis;
    }

    internal sealed class AimCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera aimCamera;

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private GameObject target;

        [SerializeField]
        private KeyCode aimKey;

        public MouseSensivity MouseSensivity;

        public float XRotation { get; private set; } 

        public float YRotation { get; private set; }

        private int holdIndex = 0;

        public bool isActive => Input.GetKey(aimKey);

        private float lastRotationIndex = 1;

        public void Update()
        {
            GameObject objectTransform = mainCamera.gameObject;
            if (isActive)
            {
                InterpolateCamera(target, mainCamera);

                XRotation = XRotation - (MouseSensivity.yAxis * Input.GetAxis("Mouse Y"));
                YRotation = YRotation + (MouseSensivity.xAxis * Input.GetAxis("Mouse X"));
                XRotation = Mathf.Clamp(XRotation, -80, 80);
                target.transform.rotation = Quaternion.Euler(XRotation, YRotation, target.transform.rotation.eulerAngles.z);
                objectTransform = target;
                Debug.Log(XRotation);
                holdIndex++;
            }
            else
                holdIndex = 0;
             objectTransform.transform.rotation = Quaternion.Lerp(objectTransform.transform.rotation, target.transform.rotation, (1f * Time.deltaTime) * lastRotationIndex);
             lastRotationIndex = Time.deltaTime;
        }

        private void InterpolateCamera(GameObject @object, Camera mainCamera) 
        {
            if (holdIndex == 0) 
            {
                var modifyRotation = Quaternion.Euler(@object.transform.rotation.eulerAngles.x, mainCamera.gameObject.gameObject.transform.eulerAngles.y - @object.transform.position.x, mainCamera.gameObject.transform.eulerAngles.z);
                @object.transform.rotation = modifyRotation;
                XRotation = 0;
                YRotation = modifyRotation.eulerAngles.y;
            }
        }

    }
}
