using System;
using TMPro;
using UnityEngine;

namespace XGStudios
{
    [RequireComponent(typeof(Rigidbody))]
    public class TheCar : MonoBehaviour
    {
        #region VARIABLES

        #region CAR SETUP VARIABLES

        [Space(5)]
        [Header("CAR SETUP")]
        [Space(10)]
        [Tooltip("The center of mass of the car. This is used to make the car more stable.")]
        public Vector3 bodyMassCenter;
        [Range(20, 250)]
        [Tooltip("The maximum speed that the car can reach in km/h.")]
        public int maxSpeed = 220;
        [Range(10, 150)]
        [Tooltip("The maximum speed that the car can reach while going on reverse in km/h.")]
        public int maxReverseSpeed = 45;
        [Range(1, 30)]
        [Tooltip("How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.")]
        public int accelerationMultiplier = 30;
        [Space(10)]
        [Range(10, 45)]
        [Tooltip("The maximum angle that the tires can reach while rotating the steering wheel.")]
        public int maxSteeringAngle = 25;
        [Range(0.1f, 1f)]
        [Tooltip("How fast the steering wheel turns.")]
        public float steeringSpeed = 0.5f;
        [Space(10)]
        [Range(100, 600)]
        [Tooltip("The strength of the wheel brakes.")]
        public int brakeForce = 450;
        [Range(1, 10)]
        [Tooltip("How fast the car decelerates when the user is not using the throttle.")]
        public int decelerationMultiplier = 3;
        [Range(1, 10)]
        [Tooltip("How much grip the car loses when the user hit the handbrake.")]
        public int handbrakeDriftMultiplier = 3;

        #endregion

        #region WHEELS VARIABLES

        [Space(15)]
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

        #endregion

        #region EFFECTS VARIABLES

        [Space(15)]
        [Header("EFFECTS")]
        [Space(10)]
        [Tooltip("The following variable lets you to set up particle systems in your car")]
        public bool useEffects = false;
        [Space(5)]
        [Tooltip("Left Rear Wheel Smoke Particle System")]
        public ParticleSystem rlwParticleSystem;
        [Tooltip("Right Rear Wheel Smoke Particle System")]
        public ParticleSystem rrwParticleSystem;
        [Space(5)]
        [Tooltip("Left Rear Wheel Skid Trail Renderer")]
        public TrailRenderer rlwTireSkid;
        [Tooltip("Right Rear Wheel Skid Trail Renderer")]
        public TrailRenderer rrwTireSkid;

        #endregion

        #region UI VARIABLES

        [Space(15)]
        [Header("UI")]
        [Space(10)]
        [Tooltip("The following variable lets you to set up a UI text to display the speed of your car.")]
        public bool useUI = false;
        [Tooltip("Used to store the UI object that is going to show the speed of the car.")]
        public TextMeshProUGUI carSpeedText;

        #endregion

        #region SOUND VARIABLES

        [Space(15)]
        [Header("Sounds")]
        [Space(10)]
        [Tooltip(
            "The following variable lets you to set up sounds for your car such as the car engine or tire screech sounds.")]
        public bool useSounds = false;
        [Tooltip("This variable stores the sound of the car engine.")]
        public AudioSource carEngineSound;
        [Tooltip("This variable stores the sound of the tire screech (when the car is drifting).")]
        public AudioSource tireScreechSound;
        [Tooltip("Used to store the initial pitch of the car engine sound.")]
        float _initialCarEngineSoundPitch;

        #endregion

        //CAR DATA
        [HideInInspector]
        public float carSpeed; // Used to store the speed of the car.
        [HideInInspector]
        public bool isDrifting; // Used to know whether the car is drifting or not.
        [HideInInspector]
        public bool isTractionLocked; // Used to know whether the traction of the car is locked or not.

        #endregion

        #region PRIVATE VARIABLES

        Rigidbody _carRigidbody;
        float
            _steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
        float _throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.
        float _driftingAxis;
        float _localVelocityZ;
        float _localVelocityX;
        bool _deceleratingCar;
        WheelFrictionCurve _flWheelFriction;
        float _flwExtremumSlip;
        WheelFrictionCurve _frWheelFriction;
        float _frwExtremumSlip;
        WheelFrictionCurve _rlWheelFriction;
        float _rlwExtremumSlip;
        WheelFrictionCurve _rrWheelFriction;
        float _rrwExtremumSlip;

        #endregion

        void Start()
        {
            _carRigidbody = gameObject.GetComponent<Rigidbody>();
            _carRigidbody.centerOfMass = bodyMassCenter;
            _flWheelFriction = new WheelFrictionCurve();
            WheelFrictionCurve sidewaysFriction = frontLeftCollider.sidewaysFriction;
            _flWheelFriction.extremumSlip = sidewaysFriction.extremumSlip;
            _flwExtremumSlip = sidewaysFriction.extremumSlip;
            _flWheelFriction.extremumValue = sidewaysFriction.extremumValue;
            _flWheelFriction.asymptoteSlip = sidewaysFriction.asymptoteSlip;
            _flWheelFriction.asymptoteValue = sidewaysFriction.asymptoteValue;
            _flWheelFriction.stiffness = sidewaysFriction.stiffness;
            _frWheelFriction = new WheelFrictionCurve();
            WheelFrictionCurve friction = frontRightCollider.sidewaysFriction;
            _frWheelFriction.extremumSlip = friction.extremumSlip;
            _frwExtremumSlip = friction.extremumSlip;
            _frWheelFriction.extremumValue = friction.extremumValue;
            _frWheelFriction.asymptoteSlip = friction.asymptoteSlip;
            _frWheelFriction.asymptoteValue = friction.asymptoteValue;
            _frWheelFriction.stiffness = friction.stiffness;
            _rlWheelFriction = new WheelFrictionCurve();
            WheelFrictionCurve sidewaysFriction1 = rearLeftCollider.sidewaysFriction;
            _rlWheelFriction.extremumSlip = sidewaysFriction1.extremumSlip;
            _rlwExtremumSlip = sidewaysFriction1.extremumSlip;
            _rlWheelFriction.extremumValue = sidewaysFriction1.extremumValue;
            _rlWheelFriction.asymptoteSlip = sidewaysFriction1.asymptoteSlip;
            _rlWheelFriction.asymptoteValue = sidewaysFriction1.asymptoteValue;
            _rlWheelFriction.stiffness = sidewaysFriction1.stiffness;
            _rrWheelFriction = new WheelFrictionCurve();
            WheelFrictionCurve friction1 = rearRightCollider.sidewaysFriction;
            _rrWheelFriction.extremumSlip = friction1.extremumSlip;
            _rrwExtremumSlip = friction1.extremumSlip;
            _rrWheelFriction.extremumValue = friction1.extremumValue;
            _rrWheelFriction.asymptoteSlip = friction1.asymptoteSlip;
            _rrWheelFriction.asymptoteValue = friction1.asymptoteValue;
            _rrWheelFriction.stiffness = friction1.stiffness;

            // We save the initial pitch of the car engine sound.
            if (carEngineSound != null)
                _initialCarEngineSoundPitch = carEngineSound.pitch;

            switch (useUI)
            {
                case true:
                    InvokeRepeating("CarSpeedUI", 0f, 0.1f);
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
                    InvokeRepeating("CarSounds", 0f, 0.1f);
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

            if (!useEffects)
            {
                if (rlwParticleSystem != null)
                    rlwParticleSystem.Stop();

                if (rrwParticleSystem != null)
                    rrwParticleSystem.Stop();

                if (rlwTireSkid != null)
                    rlwTireSkid.emitting = false;

                if (rrwTireSkid != null)
                    rrwTireSkid.emitting = false;
            }
        }

        void Update()
        {
            //CAR DATA
            // We determine the speed of the car.
            carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
            // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
            _localVelocityX = transform.InverseTransformDirection(_carRigidbody.velocity).x;
            // Save the local velocity of the car in the z axis. Used to know if the car is going forward or backwards.
            _localVelocityZ = transform.InverseTransformDirection(_carRigidbody.velocity).z;

            //CAR PHYSICS
            if (Input.GetKey(KeyCode.W))
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoForward();
            }

            if (Input.GetKey(KeyCode.S))
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoReverse();
            }

            if (Input.GetKey(KeyCode.A))
                TurnLeft();

            if (Input.GetKey(KeyCode.D))
                TurnRight();

            if (Input.GetKey(KeyCode.Space))
            {
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                Handbrake();
            }

            if (Input.GetKeyUp(KeyCode.Space))
                RecoverTraction();

            if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)))
                ThrottleOff();

            if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space) &&
                !_deceleratingCar)
            {
                InvokeRepeating("DecelerateCar", 0f, 0.1f);
                _deceleratingCar = true;
            }

            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && _steeringAxis != 0f)
                ResetSteeringAngle();
            
            // We call the method AnimateWheelMeshes() in order to match the wheel collider movements with the 3D meshes of the wheels.
            AnimateWheelMeshes();
        }
        
        public void CarSpeedUI()
        {
            if (!useUI) return;
            try{
                float absoluteCarSpeed = Mathf.Abs(carSpeed);
                carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
            }catch(Exception ex){
                Debug.LogWarning(ex);
            }

        }
        
        public void CarSounds()
        {

            switch (useSounds)
            {
                case true:
                    try{
                        if(carEngineSound != null){
                            float engineSoundPitch = _initialCarEngineSoundPitch + (Mathf.Abs(_carRigidbody.velocity.magnitude) / 25f);
                            carEngineSound.pitch = engineSoundPitch;
                        }
                        if((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f)){
                            if(!tireScreechSound.isPlaying){
                                tireScreechSound.Play();
                            }
                        }else if((!isDrifting) && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f)){
                            tireScreechSound.Stop();
                        }
                    }catch(Exception ex){
                        Debug.LogWarning(ex);
                    }
                    break;
                case false:
                {
                    if(carEngineSound != null && carEngineSound.isPlaying){
                        carEngineSound.Stop();
                    }
                    if(tireScreechSound != null && tireScreechSound.isPlaying){
                        tireScreechSound.Stop();
                    }
                    break;
                }
            }

        }
        
        //STEERING METHODS
        void TurnLeft(){
            _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
            if(_steeringAxis < -1f){
                _steeringAxis = -1f;
            }
            float steeringAngle = _steeringAxis * maxSteeringAngle;
            frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
            frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
        }

        void TurnRight(){
            _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
            if(_steeringAxis > 1f){
                _steeringAxis = 1f;
            }
            float steeringAngle = _steeringAxis * maxSteeringAngle;
            frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
            frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
        }

        void ResetSteeringAngle(){
            _steeringAxis = _steeringAxis switch
            {
                < 0f => _steeringAxis + (Time.deltaTime * 10f * steeringSpeed),
                > 0f => _steeringAxis - (Time.deltaTime * 10f * steeringSpeed),
                _ => _steeringAxis
            };

            if(Mathf.Abs(frontLeftCollider.steerAngle) < 1f){
                _steeringAxis = 0f;
            }
            float steeringAngle = _steeringAxis * maxSteeringAngle;
            frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
            frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
        }

        void AnimateWheelMeshes(){
            try{
                Quaternion FLWRotation;
                Vector3 FLWPosition;
                frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
                frontLeftMesh.transform.position = FLWPosition;
                frontLeftMesh.transform.rotation = FLWRotation;

                Quaternion FRWRotation;
                Vector3 FRWPosition;
                frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
                frontRightMesh.transform.position = FRWPosition;
                frontRightMesh.transform.rotation = FRWRotation;

                Quaternion RLWRotation;
                Vector3 RLWPosition;
                rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
                rearLeftMesh.transform.position = RLWPosition;
                rearLeftMesh.transform.rotation = RLWRotation;

                Quaternion RRWRotation;
                Vector3 RRWPosition;
                rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
                rearRightMesh.transform.position = RRWPosition;
                rearRightMesh.transform.rotation = RRWRotation;
            }catch(Exception ex){
                Debug.LogWarning(ex);
            }
        }

        //ENGINE AND BRAKING METHODS

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

            _throttleAxis = _throttleAxis + (Time.deltaTime * 3f);

            if (_throttleAxis > 1f)
                _throttleAxis = 1f;

            if (_localVelocityZ < -1f)
                Brakes();
            else
            {
                if (Mathf.RoundToInt(carSpeed) < maxSpeed)
                {
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

        void GoReverse()
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
            _throttleAxis = _throttleAxis - (Time.deltaTime * 3f);

            if (_throttleAxis < -1f)
                _throttleAxis = -1f;

            if (_localVelocityZ > 1f)
                Brakes();
            else
            {
                if (Mathf.Abs(Mathf.RoundToInt(carSpeed)) < maxReverseSpeed)
                {
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

        void ThrottleOff()
        {
            frontLeftCollider.motorTorque = 0;
            frontRightCollider.motorTorque = 0;
            rearLeftCollider.motorTorque = 0;
            rearRightCollider.motorTorque = 0;
        }
        
        public void DecelerateCar(){
            if(Mathf.Abs(_localVelocityX) > 2.5f){
                isDrifting = true;
                DriftCarPS();
            }else{
                isDrifting = false;
                DriftCarPS();
            }
            
            if(_throttleAxis != 0f)
            {
                _throttleAxis = _throttleAxis switch
                {
                    > 0f => _throttleAxis - (Time.deltaTime * 10f),
                    < 0f => _throttleAxis + (Time.deltaTime * 10f),
                    _ => _throttleAxis
                };

                if(Mathf.Abs(_throttleAxis) < 0.15f){
                    _throttleAxis = 0f;
                }
            }
            _carRigidbody.velocity = _carRigidbody.velocity * (1f / (1f + (0.025f * decelerationMultiplier)));
            frontLeftCollider.motorTorque = 0;
            frontRightCollider.motorTorque = 0;
            rearLeftCollider.motorTorque = 0;
            rearRightCollider.motorTorque = 0;
            if(_carRigidbody.velocity.magnitude < 0.25f){
                _carRigidbody.velocity = Vector3.zero;
                CancelInvoke("DecelerateCar");
            }
        }
        
        void Brakes(){
            frontLeftCollider.brakeTorque = brakeForce;
            frontRightCollider.brakeTorque = brakeForce;
            rearLeftCollider.brakeTorque = brakeForce;
            rearRightCollider.brakeTorque = brakeForce;
        }

        void Handbrake()
        {
            CancelInvoke("RecoverTraction");
            _driftingAxis = _driftingAxis + (Time.deltaTime);
            float secureStartingPoint = _driftingAxis * _flwExtremumSlip * handbrakeDriftMultiplier;

            if (secureStartingPoint < _flwExtremumSlip)
                _driftingAxis = _flwExtremumSlip / (_flwExtremumSlip * handbrakeDriftMultiplier);

            if (_driftingAxis > 1f)
                _driftingAxis = 1f;
            
            isDrifting = Mathf.Abs(_localVelocityX) > 2.5f;
            if (_driftingAxis < 1f)
            {
                _flWheelFriction.extremumSlip = _flwExtremumSlip * handbrakeDriftMultiplier * _driftingAxis;
                frontLeftCollider.sidewaysFriction = _flWheelFriction;

                _frWheelFriction.extremumSlip = _frwExtremumSlip * handbrakeDriftMultiplier * _driftingAxis;
                frontRightCollider.sidewaysFriction = _frWheelFriction;

                _rlWheelFriction.extremumSlip = _rlwExtremumSlip * handbrakeDriftMultiplier * _driftingAxis;
                rearLeftCollider.sidewaysFriction = _rlWheelFriction;

                _rrWheelFriction.extremumSlip = _rrwExtremumSlip * handbrakeDriftMultiplier * _driftingAxis;
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

                    try
                    {
                        if ((isTractionLocked || Mathf.Abs(_localVelocityX) > 5f) && Mathf.Abs(carSpeed) > 12f)
                        {
                            rlwTireSkid.emitting = true;
                            rrwTireSkid.emitting = true;
                        }
                        else
                        {
                            rlwTireSkid.emitting = false;
                            rrwTireSkid.emitting = false;
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

                    if (rlwTireSkid != null)
                        rlwTireSkid.emitting = false;

                    if (rrwTireSkid != null)
                        rrwTireSkid.emitting = false;

                    break;
                }
            }

        }

        public void RecoverTraction()
        {
            isTractionLocked = false;
            _driftingAxis = _driftingAxis - (Time.deltaTime / 1.5f);

            if (_driftingAxis < 0f)
            {
                _driftingAxis = 0f;
            }

            //If the 'driftingAxis' value is not 0f, it means that the wheels have not recovered their traction.
            //We are going to continue decreasing the sideways friction of the wheels until we reach the initial
            // car's grip.
            if (_flWheelFriction.extremumSlip > _flwExtremumSlip)
            {
                _flWheelFriction.extremumSlip = _flwExtremumSlip * handbrakeDriftMultiplier * _driftingAxis;
                frontLeftCollider.sidewaysFriction = _flWheelFriction;

                _frWheelFriction.extremumSlip = _frwExtremumSlip * handbrakeDriftMultiplier * _driftingAxis;
                frontRightCollider.sidewaysFriction = _frWheelFriction;

                _rlWheelFriction.extremumSlip = _rlwExtremumSlip * handbrakeDriftMultiplier * _driftingAxis;
                rearLeftCollider.sidewaysFriction = _rlWheelFriction;

                _rrWheelFriction.extremumSlip = _rrwExtremumSlip * handbrakeDriftMultiplier * _driftingAxis;
                rearRightCollider.sidewaysFriction = _rrWheelFriction;

                Invoke("RecoverTraction", Time.deltaTime);

            }
            else if (_flWheelFriction.extremumSlip < _flwExtremumSlip)
            {
                _flWheelFriction.extremumSlip = _flwExtremumSlip;
                frontLeftCollider.sidewaysFriction = _flWheelFriction;

                _frWheelFriction.extremumSlip = _frwExtremumSlip;
                frontRightCollider.sidewaysFriction = _frWheelFriction;

                _rlWheelFriction.extremumSlip = _rlwExtremumSlip;
                rearLeftCollider.sidewaysFriction = _rlWheelFriction;

                _rrWheelFriction.extremumSlip = _rrwExtremumSlip;
                rearRightCollider.sidewaysFriction = _rrWheelFriction;

                _driftingAxis = 0f;
            }
        }
        
    }
}
