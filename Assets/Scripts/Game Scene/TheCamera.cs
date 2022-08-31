using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class TheCamera : MonoBehaviour
    {
        [Header("Main Stuff")]
        public float sensitivity = 5f;
        
        GameObject _theCar;
        GameObject _camHolder;
        Camera _cam;

        void Start()
        {
            _theCar = GameObject.FindGameObjectWithTag("Car");
            _camHolder = GameObject.Find("Cam Holder");
            _cam = Camera.main;
        }

        void Update()
        {
            Vector3 carPos = _theCar.transform.position;
            Vector3 camLocalPos = _cam.transform.localPosition;
            Quaternion camHolderRot = _camHolder.transform.rotation;
            
            _camHolder.transform.position = new Vector3(carPos.x, carPos.y, carPos.z);

            Quaternion rot = Quaternion.Euler(camHolderRot.eulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity / 2,
                camHolderRot.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivity,
                camHolderRot.eulerAngles.z);
            
            camHolderRot = Quaternion.Slerp(transform.rotation, rot, 2f);
            
            _camHolder.transform.rotation = camHolderRot;

            if (!(_cam.transform.localPosition.z > -1f)) return;
            camLocalPos = new Vector3(camLocalPos.x, camLocalPos.y, -1f);
            _cam.transform.localPosition = camLocalPos;
        }
    }
}
