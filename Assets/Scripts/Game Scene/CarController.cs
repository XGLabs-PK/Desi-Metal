﻿using UnityEngine;
using XGStudios.GameScene;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class CarController : MonoBehaviour
    {
        private CarBody carBody;
        private CarWheels[] wheels;
        private GameObject tempContainer;
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        [Header("Behavior")]
        [Tooltip("How much torque to apply to the wheels. 1 is full speed forward, -1 is full speed backward, 0 is rest.")]
        public float motorDelta = 0;
        [Tooltip("How much steering to apply to the wheels. 1 is right, -1 is left, 0 is straight.")]
        public float steeringDelta = 0;
        [Tooltip("Whether to apply the handbrake to the wheels.")]
        public bool applyHandbrake = false;

        void Awake()
        {
            carBody = GetComponentInChildren<CarBody>();
            carBody.initialize(this);

            wheels = GetComponentsInChildren<CarWheels>();
            foreach (CarWheels wheel in wheels)
            {
                wheel.initialize(this);
            }

            tempContainer = new GameObject("temp " + gameObject.name);
            tempContainer.transform.SetParent(null);
            tempContainer.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            foreach (CarWheels wheel in wheels)
            {
                wheel.setMotor(motorDelta);
                wheel.setSteering(steeringDelta);
                wheel.setHandbrake(applyHandbrake);
            }
        }

        public CarBody getCarBody()
        {
            return carBody;
        }

        public Rigidbody getRigidbody()
        {
            return getCarBody()?.getRigidbody() ?? null;
        }

        public void setMotor(float d)
        {
            motorDelta = d;
        }

        public void setSteering(float d)
        {
            steeringDelta = d;
        }

        public void setHandbrake(bool e)
        {
            applyHandbrake = e;
        }

        public float getMotor()
        {
            return motorDelta;
        }

        public float getSteering()
        {
            return steeringDelta;
        }

        public bool getHandbrake()
        {
            return applyHandbrake;
        }

        public float getWheelsMaxSpin(int direction = 0)
        {
            float maxSpin = 0;
            foreach (CarWheels w in wheels)
            {
                float spin = w.getForwardSpinVelocity();
                if ((direction == 0 && Mathf.Abs(spin) > Mathf.Abs(maxSpin)) || (direction == 1 && spin > maxSpin) || (direction == -1 && spin < maxSpin))
                {
                    maxSpin = spin;
                }
            }
            return maxSpin;
        }

        public float getWheelsMaxSpeed()
        {
            float maxSpeed = 0;
            foreach (CarWheels w in wheels)
            {
                if (w.motorMaxSpeed > maxSpeed) maxSpeed = w.motorMaxSpeed;
            }
            return maxSpeed;
        }

        public float getPitchAngle()
        {
            return getCarBody()?.getPitchAngle() ?? 0;
        }

        public float getRollAngle()
        {
            return getCarBody()?.getRollAngle() ?? 0;
        }

        public float getForwardVelocity()
        {
            return getCarBody()?.getForwardVelocity() ?? 0;
        }

        public float getForwardVelocityDelta()
        {
            if (getWheelsMaxSpeed() == 0) return 0;
            return getForwardVelocity() / getWheelsMaxSpeed();
        }

        public float getLateralVelocity()
        {
            return getCarBody()?.getLateralVelocity() ?? 0;
        }

        public bool isPartiallyGrounded()
        {
            return isGrounded() && !isFullyGrounded();
        }

        public bool isGrounded()
        {
            foreach (CarWheels w in wheels)
            {
                if (w.isTouchingGround()) return true;
            }
            return false;
        }

        public bool isFullyGrounded()
        {
            foreach (CarWheels w in wheels)
            {
                if (!w.isTouchingGround()) return false;
            }
            return true;
        }

        public void immobilize()
        {
            foreach (CarWheels w in wheels)
            {
                w.immobilize();
            }
            carBody.immobilize();
        }

        public void translate(Vector3 position)
        {
            setPosition(getPosition() + position);
        }

        public void rotate(Quaternion rotation)
        {
            setRotation(getRotation() * rotation);
        }

        public void recenter()
        {
            tempContainer.transform.position = carBody.getPosition();
            tempContainer.transform.rotation = carBody.getRotation();

            setParent(tempContainer.transform);

            transform.position = tempContainer.transform.position;
            transform.rotation = tempContainer.transform.rotation;

            setParent(transform);
        }

        public void setPosition(Vector3 position)
        {
            recenter();
            transform.position = position;
        }

        public void setRotation(Quaternion rotation)
        {
            recenter();
            transform.rotation = rotation;
        }

        public Vector3 getPosition()
        {
            return carBody.getPosition();
        }

        public Quaternion getRotation()
        {
            return carBody.getRotation();
        }

        public Vector3 getInitialPosition()
        {
            return initialPosition;
        }

        public Quaternion getInitialRotation()
        {
            return initialRotation;
        }

        public void setParent(Transform parent)
        {
            carBody.setParent(parent);
            foreach (CarWheels w in wheels)
            {
                w.setParent(parent);
            }
        }
    }
}