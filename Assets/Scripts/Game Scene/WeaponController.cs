using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGStudios.GameScene
{
    public class WeaponController : MonoBehaviour
    {
        private Camera cam;

        void Start()
        {
            cam = Camera.main;
        }

        void Update()
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}
