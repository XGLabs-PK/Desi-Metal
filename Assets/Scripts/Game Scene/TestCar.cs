using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class TestCar : MonoBehaviour
    {
        public Rigidbody theRb;
        public float forwardAcceleration = 8;
        public float reverseAcceleration = 4;
        public float maxSpeed = 50;
        public float turnStrength = 180;
        float _speedInput;
        float _turnInput;
        public float gravityForce = 10;
        public LayerMask groundLayer;
        public float groundLayLength = 0.5f;
        public Transform groundRayPoint;
        public float dragOnGround = 3;

        bool _isGrounded;

        void Start()
        {
            theRb.transform.parent = null;
        }

        void Update()
        {
            _speedInput = 0f;

            if (Input.GetAxis("Vertical") > 0)
                _speedInput = Input.GetAxis("Vertical") * forwardAcceleration * 1000f;
            else if (Input.GetAxis("Vertical") < 0)
                _speedInput = Input.GetAxis("Vertical") * reverseAcceleration * 1000f;

            _turnInput = Input.GetAxis("Horizontal");
            
            if (_isGrounded)
                transform.rotation =
                    Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));

            transform.position = theRb.transform.position;
        }

        void FixedUpdate()
        {
            _isGrounded = false;

            if (Physics.Raycast(groundRayPoint.position, -transform.up, out RaycastHit _, groundLayLength, groundLayer))
                _isGrounded = true;

            if (_isGrounded)
            {
                theRb.drag = dragOnGround;

                if (Mathf.Abs(_speedInput) > 0)
                {
                    theRb.AddForce(transform.forward * _speedInput);
                }
                else
                {
                    theRb.drag = 0.1f;
                    theRb.AddForce(Vector3.up * (-gravityForce * 100f));
                }
            }
        }
    }
}
