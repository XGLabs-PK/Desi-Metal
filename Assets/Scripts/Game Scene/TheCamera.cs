using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheCamera : MonoBehaviour
    {
        [Header("Main Stuff")]
        public float sensitivity = 5f;
        public GameObject theCar;
        public GameObject camHolder;
        Camera _cam;

        void Start()
        {
            _cam = Camera.main;
        }

        void Update()
        {
            Vector3 carPos = theCar.transform.position;
            Vector3 camLocalPos = _cam.transform.localPosition;
            Quaternion camHolderRot = camHolder.transform.rotation;

            camHolder.transform.position = new Vector3(
                carPos.x,
                carPos.y,
                carPos.z);
            
            Quaternion rot = Quaternion.Euler(camHolderRot.eulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity / 2,
                camHolderRot.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivity,
                camHolderRot.eulerAngles.z);

            camHolderRot = Quaternion.Slerp(transform.rotation, rot, 2f);

            camHolder.transform.rotation = camHolderRot;

            if (!(_cam.transform.localPosition.z > -1f)) return;
            camLocalPos = new Vector3(camLocalPos.x, camLocalPos.y, -1f);
            _cam.transform.localPosition = camLocalPos;
        }
    }
}
