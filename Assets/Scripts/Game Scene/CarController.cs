using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class CarController : MonoBehaviour
    {
        [Header("MISC")]
        [SerializeField] LayerMask groundLayer;

        [Header("CAR SETUP")]
        [Range(20, 190)]
        public int maxSpeed = 90; //The maximum speed that the car can reach in km/h.

        [Range(10, 120)]
        public int maxReverseSpeed = 45; //The maximum speed that the car can reach while going on reverse in km/h.

        [Range(1, 10)]
        public int accelerationMultiplier = 2; // How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.

        [Space(10)]
        [Range(10, 45)]
        public int maxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.

        [Range(0.1f, 1f)]
        public float steeringSpeed = 0.5f; // How fast the steering wheel turns.

        [Space(10)]
        [Range(100, 600)]
        public int brakeForce = 350; // The strength of the wheel brakes.

        [Range(1, 10)]
        public int decelerationMultiplier = 2; // How fast the car decelerates when the user is not using the throttle.

        [Range(1, 10)]
        public int handbrakeDriftMultiplier = 5; // How much grip the car loses when the user hit the handbrake.

        [Space(10)]
        public Vector3 bodyMassCenter; // This is a vector that contains the center of mass of the car. I recommend to set this value

        [Header("WHEELS")]
        [Space(10)]
        public GameObject frontLeftMesh;

        public WheelCollider frontLeftCollider;

        [Space(10)]
        public GameObject frontRightMesh;

        public WheelCollider frontRightCollider;

        [Space(10)]
        public GameObject rearLeftMesh;

        public WheelCollider rearLeftCollider;

        [Space(10)]
        public GameObject rearRightMesh;

        public WheelCollider rearRightCollider;

        [Header("EFFECTS")]
        [Space(10)]
        public bool useEffects = false;

        // The following particle systems are used as tire smoke when the car drifts.
        public ParticleSystem rlwParticleSystem;
        public ParticleSystem rrwParticleSystem;

        [Header("UI")]
        [Space(10)]
        public bool useUI;

        public TextMeshProUGUI carSpeedText; // Used to store the UI object that is going to show the speed of the car.

        [Header("Sounds")]
        [Space(10)]
        public bool useSounds = false;

        public AudioSource carEngineSound; // This variable stores the sound of the car engine.
        public AudioSource tireScreechSound; // This variable stores the sound of the tire screech (when the car is drifting).
        float _initialCarEngineSoundPitch; // Used to store the initial pitch of the car engine sound.

        //CAR DATA
        [HideInInspector]
        public float carSpeed; // Used to store the speed of the car.

        [HideInInspector]
        public bool isDrifting; // Used to know whether the car is drifting or not.

        [HideInInspector]
        public bool isTractionLocked; // Used to know whether the traction of the car is locked or not.

        //PRIVATE VARIABLES
        Rigidbody _carRigidbody; // Stores the car's rigidbody.

        
        float _steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
        float _throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.
        float _driftingAxis;
        float _localVelocityZ;
        float _localVelocityX;
        bool _deceleratingCar;
        WheelFrictionCurve _flWheelFriction;
         float _flWheelSlip;
        WheelFrictionCurve _frWheelFriction;
        float _frWheelSlip;
        WheelFrictionCurve _rlWheelFriction;
        float _rlWheelSlip;
        WheelFrictionCurve _rrWheelFriction;
        float _rrWheelSlip;

        bool _isGrounded;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _carRigidbody = gameObject.GetComponent<Rigidbody>();
            _carRigidbody.centerOfMass = bodyMassCenter;
            _flWheelFriction = new WheelFrictionCurve();
            WheelFrictionCurve leftSideFriction = frontLeftCollider.sidewaysFriction;
            _flWheelFriction.extremumSlip = leftSideFriction.extremumSlip;
            _flWheelSlip = leftSideFriction.extremumSlip;
            _flWheelFriction.extremumValue = leftSideFriction.extremumValue;
            _flWheelFriction.asymptoteSlip = leftSideFriction.asymptoteSlip;
            _flWheelFriction.asymptoteValue = leftSideFriction.asymptoteValue;
            _flWheelFriction.stiffness = leftSideFriction.stiffness;
            _frWheelFriction = new WheelFrictionCurve();
            WheelFrictionCurve rightSideFriction = frontRightCollider.sidewaysFriction;
            _frWheelFriction.extremumSlip = rightSideFriction.extremumSlip;
            _frWheelSlip = rightSideFriction.extremumSlip;
            _frWheelFriction.extremumValue = rightSideFriction.extremumValue;
            _frWheelFriction.asymptoteSlip = rightSideFriction.asymptoteSlip;
            _frWheelFriction.asymptoteValue = rightSideFriction.asymptoteValue;
            _frWheelFriction.stiffness = rightSideFriction.stiffness;
            _rlWheelFriction = new WheelFrictionCurve();
            WheelFrictionCurve rearLeftFriction = rearLeftCollider.sidewaysFriction;
            _rlWheelFriction.extremumSlip = rearLeftFriction.extremumSlip;
            _rlWheelSlip = rearLeftFriction.extremumSlip;
            _rlWheelFriction.extremumValue = rearLeftFriction.extremumValue;
            _rlWheelFriction.asymptoteSlip = rearLeftFriction.asymptoteSlip;
            _rlWheelFriction.asymptoteValue = rearLeftFriction.asymptoteValue;
            _rlWheelFriction.stiffness = rearLeftFriction.stiffness;
            _rrWheelFriction = new WheelFrictionCurve();
            WheelFrictionCurve rearRightFriction = rearRightCollider.sidewaysFriction;
            _rrWheelFriction.extremumSlip = rearRightFriction.extremumSlip;
            _rrWheelSlip = rearRightFriction.extremumSlip;
            _rrWheelFriction.extremumValue = rearRightFriction.extremumValue;
            _rrWheelFriction.asymptoteSlip = rearRightFriction.asymptoteSlip;
            _rrWheelFriction.asymptoteValue = rearRightFriction.asymptoteValue;
            _rrWheelFriction.stiffness = rearRightFriction.stiffness;

            // We save the initial pitch of the car engine sound.
            if (carEngineSound != null)
            {
                _initialCarEngineSoundPitch = carEngineSound.pitch;
            }
            switch (useUI)
            {
                case true:
                    InvokeRepeating(nameof(CarSpeedUI), 0f, 0.1f);
                    break;
                case false:
                {
                    if (carSpeedText != null)
                        carSpeedText.text = "0";
                    break;
                }
            }

            switch (useSounds)
            {
                case true:
                    InvokeRepeating(nameof(CarSounds), 0f, 0.1f);
                    break;
                case false:
                {
                    if (carEngineSound != null)
                        carEngineSound.Stop();
                    if (tireScreechSound != null)
                        tireScreechSound.Stop();
                    break;
                }
            }

            if (useEffects) return;
            if (rlwParticleSystem != null)
                rlwParticleSystem.Stop();
            if (rrwParticleSystem != null)
                rrwParticleSystem.Stop();
        }

        void Update()
        {
            //CAR DATA
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 3f + 0.1f);
            // We determine the speed of the car.
            carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 80) / 1000;
            // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
            _localVelocityX = transform.InverseTransformDirection(_carRigidbody.velocity).x;
            // Save the local velocity of the car in the z axis. Used to know if the car is going forward or backwards.
            _localVelocityZ = transform.InverseTransformDirection(_carRigidbody.velocity).z;

            if (Input.GetKey(KeyCode.W))
            {
                CancelInvoke(nameof(DecelerateCar));
                _deceleratingCar = false;
                GoForward();
            }
            if (Input.GetKey(KeyCode.S))
            {
                CancelInvoke(nameof(DecelerateCar));
                _deceleratingCar = false;
                GoReverse();
            }

            if (Input.GetKey(KeyCode.A))
                TurnLeft();
            if (Input.GetKey(KeyCode.D))
                TurnRight();
            if (Input.GetKey(KeyCode.Space))
            {
                CancelInvoke(nameof(DecelerateCar));
                _deceleratingCar = false;
                Handbrake();
            }
            if (Input.GetKeyUp(KeyCode.Space))
                RecoverTraction();
            if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)))
                ThrottleOff();
            if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space) && !_deceleratingCar)
            {
                InvokeRepeating(nameof(DecelerateCar), 0f, 0.1f);
                _deceleratingCar = true;
            }
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && _steeringAxis != 0f)
                ResetSteeringAngle();

            if (Physics.Raycast(transform.position, transform.up, out RaycastHit _, 5, groundLayer))
                Invoke(nameof(RestartGame), 1f);

            // We call the method AnimateWheelMeshes() in order to match the wheel collider movements with the 3D meshes of the wheels.
            AnimateWheelMeshes();
            
        }
        
        void RestartGame()
        {
            SceneManager.LoadScene($"Game Scene");
        }

        // This method converts the car speed data from float to string, and then set the text of the UI carSpeedText with this value.
        public void CarSpeedUI()
        {
            if (!useUI)return;
            try
            {
                float absoluteCarSpeed = Mathf.Abs(carSpeed);
                carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }

        public void CarSounds()
        {
            switch (useSounds)
            {
                case true:
                    try
                    {
                        if (carEngineSound != null)
                        {
                            float engineSoundPitch = _initialCarEngineSoundPitch + (Mathf.Abs(_carRigidbody.velocity.magnitude) / 25f);
                            carEngineSound.pitch = engineSoundPitch;
                        }
                        if ((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f))
                        {
                            if (!tireScreechSound.isPlaying)
                                tireScreechSound.Play();
                        }
                        else if ((!isDrifting) && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f))
                            tireScreechSound.Stop();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning(ex);
                    }

                    break;
                case false:
                {
                    if (carEngineSound != null && carEngineSound.isPlaying)
                        carEngineSound.Stop();
                    if (tireScreechSound != null && tireScreechSound.isPlaying)
                        tireScreechSound.Stop();
                    break;
                }
            }
        }

        //The following method turns the front car wheels to the left. The speed of this movement will depend on the steeringSpeed variable.
        void TurnLeft()
        {
            _steeringAxis -= (Time.deltaTime * 7f * steeringSpeed);
            if (_steeringAxis < -1f)
            {
                _steeringAxis = -1f;
            }
            float steeringAngle = _steeringAxis * maxSteeringAngle;
            frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
            frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
        }

        //The following method turns the front car wheels to the right. The speed of this movement will depend on the steeringSpeed variable.
        void TurnRight()
        {
            _steeringAxis += (Time.deltaTime * 7f * steeringSpeed);
            if (_steeringAxis > 1f)
            {
                _steeringAxis = 1f;
            }
            float steeringAngle = _steeringAxis * maxSteeringAngle;
            frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
            frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
        }

        void ResetSteeringAngle()
        {
            _steeringAxis = _steeringAxis switch
            {
                < 0f => _steeringAxis + (Time.deltaTime * 7f * steeringSpeed),
                > 0f => _steeringAxis - (Time.deltaTime * 7f * steeringSpeed),
                _ => _steeringAxis
            };

            if (Mathf.Abs(frontLeftCollider.steerAngle) < 1f)
            {
                _steeringAxis = 0f;
            }
            float steeringAngle = _steeringAxis * maxSteeringAngle;
            frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
            frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
        }

        // This method matches both the position and rotation of the WheelColliders with the WheelMeshes.
        private void AnimateWheelMeshes()
        {
            try
            {
                Quaternion flwRotation;
                Vector3 flwPosition;
                frontLeftCollider.GetWorldPose(out flwPosition, out flwRotation);
                frontLeftMesh.transform.position = flwPosition;
                frontLeftMesh.transform.rotation = flwRotation;

                Quaternion frwRotation;
                Vector3 frwPosition;
                frontRightCollider.GetWorldPose(out frwPosition, out frwRotation);
                frontRightMesh.transform.position = frwPosition;
                frontRightMesh.transform.rotation = frwRotation;

                Quaternion rlwRotation;
                Vector3 rlwPosition;
                rearLeftCollider.GetWorldPose(out rlwPosition, out rlwRotation);
                rearLeftMesh.transform.position = rlwPosition;
                rearLeftMesh.transform.rotation = rlwRotation;

                Quaternion rrwRotation;
                Vector3 rrwPosition;
                rearRightCollider.GetWorldPose(out rrwPosition, out rrwRotation);
                rearRightMesh.transform.position = rrwPosition;
                rearRightMesh.transform.rotation = rrwRotation;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }

        void GoForward()
        {
            if (Mathf.Abs(_localVelocityX) > 2.5f)
            {
                isDrifting = true;
                DriftCarPS();
            }
            else
            {
                isDrifting = false;
                DriftCarPS();
            }
            // The following part sets the throttle power to 1 smoothly.
            _throttleAxis += (Time.deltaTime * 3f);
            if (_throttleAxis > 1f)
                _throttleAxis = 1f;
            if (_localVelocityZ < -1f)
                Brakes();
            else
            {
                if (Mathf.RoundToInt(carSpeed) < maxSpeed)
                {
                    //Apply positive torque in all wheels to go forward if maxSpeed has not been reached.
                    frontLeftCollider.brakeTorque = 0;
                    frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * _throttleAxis;
                    frontRightCollider.brakeTorque = 0;
                    frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * _throttleAxis;
                    rearLeftCollider.brakeTorque = 0;
                    rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * _throttleAxis;
                    rearRightCollider.brakeTorque = 0;
                    rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * _throttleAxis;
                }
                else
                {
                    frontLeftCollider.motorTorque = 0;
                    frontRightCollider.motorTorque = 0;
                    rearLeftCollider.motorTorque = 0;
                    rearRightCollider.motorTorque = 0;
                }
            }
        }

        // This method apply negative torque to the wheels in order to go backwards.
        void GoReverse()
        {
            if (Mathf.Abs(_localVelocityX) > 3f)
            {
                isDrifting = true;
                DriftCarPS();
            }
            else
            {
                isDrifting = false;
                DriftCarPS();
            }
            // The following part sets the throttle power to -1 smoothly.
            _throttleAxis -= (Time.deltaTime * 3f);
            if (_throttleAxis < -1f)
            {
                _throttleAxis = -1f;
            }
            if (_localVelocityZ > 1f)
            {
                Brakes();
            }
            else
            {
                if (Mathf.Abs(Mathf.RoundToInt(carSpeed)) < maxReverseSpeed)
                {
                    //Apply negative torque in all wheels to go in reverse if maxReverseSpeed has not been reached.
                    frontLeftCollider.brakeTorque = 0;
                    frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * _throttleAxis;
                    frontRightCollider.brakeTorque = 0;
                    frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * _throttleAxis;
                    rearLeftCollider.brakeTorque = 0;
                    rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * _throttleAxis;
                    rearRightCollider.brakeTorque = 0;
                    rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * _throttleAxis;
                }
                else
                {
                    frontLeftCollider.motorTorque = 0;
                    frontRightCollider.motorTorque = 0;
                    rearLeftCollider.motorTorque = 0;
                    rearRightCollider.motorTorque = 0;
                }
            }
        }

        //The following function set the motor torque to 0 (in case the user is not pressing either W or S).
        void ThrottleOff()
        {
            frontLeftCollider.motorTorque = 0;
            frontRightCollider.motorTorque = 0;
            rearLeftCollider.motorTorque = 0;
            rearRightCollider.motorTorque = 0;
        }

        public void DecelerateCar()
        {
            if (Mathf.Abs(_localVelocityX) > 2.5f)
            {
                isDrifting = true;
                DriftCarPS();
            }
            else
            {
                isDrifting = false;
                DriftCarPS();
            }
            // The following part resets the throttle power to 0 smoothly.
            if (_throttleAxis != 0f)
            {
                _throttleAxis = _throttleAxis switch
                {
                    > 0f => _throttleAxis - (Time.deltaTime * 10f),
                    < 0f => _throttleAxis + (Time.deltaTime * 10f),
                    _ => _throttleAxis
                };

                if (Mathf.Abs(_throttleAxis) < 0.15f)
                {
                    _throttleAxis = 0f;
                }
            }
            _carRigidbody.velocity *= (1f / (1f + (0.025f * decelerationMultiplier)));
            // Since we want to decelerate the car, we are going to remove the torque from the wheels of the car.
            frontLeftCollider.motorTorque = 0;
            frontRightCollider.motorTorque = 0;
            rearLeftCollider.motorTorque = 0;
            rearRightCollider.motorTorque = 0;
            if (_carRigidbody.velocity.magnitude < 0.25f)
            {
                _carRigidbody.velocity = Vector3.zero;
                CancelInvoke(nameof(DecelerateCar));
            }
        }

        // This function applies brake torque to the wheels according to the brake force given by the user.
        void Brakes()
        {
            frontLeftCollider.brakeTorque = brakeForce;
            frontRightCollider.brakeTorque = brakeForce;
            rearLeftCollider.brakeTorque = brakeForce;
            rearRightCollider.brakeTorque = brakeForce;
        }

        void Handbrake()
        {
            CancelInvoke(nameof(RecoverTraction));
            _driftingAxis += (Time.deltaTime);
            float secureStartingPoint = _driftingAxis * _flWheelSlip * handbrakeDriftMultiplier;

            if (secureStartingPoint < _flWheelSlip)
            {
                _driftingAxis = _flWheelSlip / (_flWheelSlip * handbrakeDriftMultiplier);
            }
            if (_driftingAxis > 1f)
            {
                _driftingAxis = 1f;
            }
            if (Mathf.Abs(_localVelocityX) > 2.5f)
            {
                isDrifting = true;
            }
            else
            {
                isDrifting = false;
            }

            if (_driftingAxis < 1f)
            {
                _flWheelFriction.extremumSlip = _flWheelSlip * handbrakeDriftMultiplier * _driftingAxis;
                frontLeftCollider.sidewaysFriction = _flWheelFriction;

                _frWheelFriction.extremumSlip = _frWheelSlip * handbrakeDriftMultiplier * _driftingAxis;
                frontRightCollider.sidewaysFriction = _frWheelFriction;

                _rlWheelFriction.extremumSlip = _rlWheelSlip * handbrakeDriftMultiplier * _driftingAxis;
                rearLeftCollider.sidewaysFriction = _rlWheelFriction;

                _rrWheelFriction.extremumSlip = _rrWheelSlip * handbrakeDriftMultiplier * _driftingAxis;
                rearRightCollider.sidewaysFriction = _rrWheelFriction;
            }

            isTractionLocked = true;
            DriftCarPS();
        }

        void DriftCarPS()
        {
            switch (useEffects)
            {
                case true:
                    try
                    {
                        switch (isDrifting)
                        {
                            case true:
                                rlwParticleSystem.Play();
                                rrwParticleSystem.Play();
                                break;
                            case false:
                                rlwParticleSystem.Stop();
                                rrwParticleSystem.Stop();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning(ex);
                    }
                    

                    break;
                case false:
                {
                    if (rlwParticleSystem != null)
                        rlwParticleSystem.Stop();
                    if (rrwParticleSystem != null)
                        rrwParticleSystem.Stop();

                    break;
                }
            }
        }

        // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
        public void RecoverTraction()
        {
            isTractionLocked = false;
            _driftingAxis -= (Time.deltaTime / 1.5f);
            if (_driftingAxis < 0f)
            {
                _driftingAxis = 0f;
            }

            if (_flWheelFriction.extremumSlip > _flWheelSlip)
            {
                _flWheelFriction.extremumSlip = _flWheelSlip * handbrakeDriftMultiplier * _driftingAxis;
                frontLeftCollider.sidewaysFriction = _flWheelFriction;

                _frWheelFriction.extremumSlip = _frWheelSlip * handbrakeDriftMultiplier * _driftingAxis;
                frontRightCollider.sidewaysFriction = _frWheelFriction;

                _rlWheelFriction.extremumSlip = _rlWheelSlip * handbrakeDriftMultiplier * _driftingAxis;
                rearLeftCollider.sidewaysFriction = _rlWheelFriction;

                _rrWheelFriction.extremumSlip = _rrWheelSlip * handbrakeDriftMultiplier * _driftingAxis;
                rearRightCollider.sidewaysFriction = _rrWheelFriction;

                Invoke(nameof(RecoverTraction), Time.deltaTime);
            }
            else if (_flWheelFriction.extremumSlip < _flWheelSlip)
            {
                _flWheelFriction.extremumSlip = _flWheelSlip;
                frontLeftCollider.sidewaysFriction = _flWheelFriction;

                _frWheelFriction.extremumSlip = _frWheelSlip;
                frontRightCollider.sidewaysFriction = _frWheelFriction;

                _rlWheelFriction.extremumSlip = _rlWheelSlip;
                rearLeftCollider.sidewaysFriction = _rlWheelFriction;

                _rrWheelFriction.extremumSlip = _rrWheelSlip;
                rearRightCollider.sidewaysFriction = _rrWheelFriction;

                _driftingAxis = 0f;
            }
        }
    }
}