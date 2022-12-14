using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using XG.Studios;
using Random = UnityEngine.Random;

namespace XGStudios
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        [SerializeField]
        LayerMask groundLayer;
        [SerializeField]
        GameObject groundCheck;
        
        [Space(5f)]
        [Header("Multiplier Stuff")]
        [SerializeField]
        GameObject airMultiplierPopup;
        [SerializeField]
        TextMeshProUGUI airMultiplierScore;
        [SerializeField]
        Slider airMultiplierSlider;
        [SerializeField]
        GameObject scoreMultiplierPopup;
        [SerializeField]
        GameObject abilityText;
        
        [Space(5f)]
        [Header("Effects")]
        public static bool AbilityUsed;
        [SerializeField]
        GameObject headlights;
        [SerializeField]
        GameObject backlights;
        [SerializeField]
        GameObject smokeEffect;
        [SerializeField]
        ScriptableRendererFeature blitRender;
        
        [Space(5f)]
        [Header("Car Flipping")]
        [SerializeField]
        GameObject topCheck;
        [SerializeField]
        float flipRadius;
        [SerializeField]
        float angle;
        [SerializeField]
        float rotationSpeed;
        
        [Space(5f)]
        [Header("Wheels")]
        [SerializeField]
        Wheel FrontLeftWheel;
        [SerializeField]
        Wheel FrontRightWheel;
        [SerializeField]
        Wheel RearLeftWheel;
        [SerializeField]
        Wheel RearRightWheel;
        
        [Space(5f)]
        [Header("idk")]
        [SerializeField]
        Transform COM;
        [SerializeField]
        CarConfig CarConfig;

        float _airMultiplier;
        bool _airMultiplierFilled;
        float[] _allGearsRatio;
        bool _backLights;
        float _currentAcceleration;
        float _currentBrake;
        float _currentSteerAngle;
        float _desiredZ;
        int _firstDriveWheel;
        bool _headLights;
        bool _inHandBrake;
        int _lastDriveWheel;
        Rigidbody _rb;

        CarConfig GetCarConfig => CarConfig;
        Wheel[] Wheels { get; set; }
        public Rigidbody Rb
        {
            get
            {
                if (!_rb)
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    _rb = GetComponent<Rigidbody>();

                return _rb;
            }
        }

        public float CurrentMaxSlip { get; private set; }
        int CurrentMaxSlipWheelIndex { get; set; }
        float CurrentSpeed { get; set; }
        public float SpeedInHour => CurrentSpeed * C.KPHMult;
        public int CarDirection => CurrentSpeed < 1 ? 0 : VelocityAngle < 90 && VelocityAngle > -90 ? 1 : -1;

        void Awake()
        {
            BlitEffect(false);
            airMultiplierPopup.SetActive(false);
            scoreMultiplierPopup.SetActive(false);
            smokeEffect.SetActive(false);
            headlights.SetActive(false);
            backlights.SetActive(false);
            Rb.centerOfMass = COM.localPosition;
            _desiredZ = transform.eulerAngles.z;

            Wheels = new[]
            {
                FrontLeftWheel,
                FrontRightWheel,
                RearLeftWheel,
                RearRightWheel
            };

            //Set drive wheel.
            switch (DriveType)
            {
                case DriveType.AWD:
                    _firstDriveWheel = 0;
                    _lastDriveWheel = 3;
                    break;
                case DriveType.FWD:
                    _firstDriveWheel = 0;
                    _lastDriveWheel = 1;
                    break;
                case DriveType.RWD:
                    _firstDriveWheel = 2;
                    _lastDriveWheel = 3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Divide the motor torque by the count of driving wheels
            _maxMotorTorque = CarConfig.MaxMotorTorque / (_lastDriveWheel - _firstDriveWheel + 1);


            //Calculated gears ratio with main ratio
            _allGearsRatio = new float[GearsRatio.Length + 2];
            _allGearsRatio[0] = ReversGearRatio * MainRatio;
            _allGearsRatio[1] = 0;

            for (int i = 0; i < GearsRatio.Length; i++)
                _allGearsRatio[i + 2] = GearsRatio[i] * MainRatio;
        }

        void Update()
        {
            headlights.SetActive(_headLights);
            backlights.SetActive(_backLights);

            AirMultiplier();

            if (Input.GetButtonDown("HeadLights") && !_headLights)
                _headLights = true;
            else if (Input.GetButtonDown("HeadLights") && _headLights)
                _headLights = false;

            for (int i = 0; i < Wheels.Length; i++)
                Wheels[i].UpdateVisual();

            smokeEffect.SetActive(_rb.velocity.magnitude > 5 && IsGrounded());
        }

        void FixedUpdate()
        {
            if (Flipped())
                StartCoroutine(Flip());

            CurrentSpeed = Rb.velocity.magnitude;
            UpdateSteerAngleLogic();
            UpdateRpmAndTorqueLogic();

            //Find max slip and update braking ground logic.
            CurrentMaxSlip = Wheels[0].CurrentMaxSlip;
            CurrentMaxSlipWheelIndex = 0;

            switch (_inHandBrake)
            {
                case true:
                    RearLeftWheel.WheelCollider.brakeTorque = MaxBrakeTorque;
                    RearRightWheel.WheelCollider.brakeTorque = MaxBrakeTorque;
                    FrontLeftWheel.WheelCollider.brakeTorque = 0;
                    FrontRightWheel.WheelCollider.brakeTorque = 0;
                    _backLights = true;
                    break;
                case false:
                    _backLights = false;
                    break;
            }

            for (int i = 0; i < Wheels.Length; i++)
            {
                if (!_inHandBrake)
                    Wheels[i].WheelCollider.brakeTorque = _currentBrake;

                Wheels[i].FixedUpdate();

                if (!(CurrentMaxSlip < Wheels[i].CurrentMaxSlip)) continue;
                CurrentMaxSlip = Wheels[i].CurrentMaxSlip;
                CurrentMaxSlipWheelIndex = i;
            }
        }

        public void UpdateControls(float horizontal, float vertical, bool handBrake)
        {
            float targetSteerAngle = horizontal * MaxSteerAngle;

            if (EnableSteerAngleMultiplier)
                targetSteerAngle *= Mathf.Clamp(1 - SpeedInHour / MaxSpeedForMinAngleMultiplier,
                    MinSteerAngleMultiplier,
                    MaxSteerAngleMultiplier);
            
            _currentSteerAngle =
                Mathf.MoveTowards(_currentSteerAngle, targetSteerAngle, Time.deltaTime * SteerAngleChangeSpeed);
            _currentAcceleration = vertical;
            _inHandBrake = handBrake;
        }

        bool IsGrounded()
        {
            float height = .008f;
            BoxCollider boxCollider = GetComponent<BoxCollider>();

            Physics.Raycast(groundCheck.transform.position, Vector3.down, boxCollider.bounds.extents.y + height,
                groundLayer);

            Debug.DrawRay(groundCheck.transform.position, Vector3.down * (boxCollider.bounds.extents.y + height),
                Color.green);

            return Physics.Raycast(groundCheck.transform.position, Vector3.down, boxCollider.bounds.extents.y + height,
                groundLayer);
        }
        
        #region Air Multiplier

                void AirMultiplier()
        {
            if (GameManager.Instance.gamePaused) return;
            int floor = Mathf.FloorToInt(_airMultiplier);
            airMultiplierScore.SetText(floor.ToString());

            if (!IsGrounded())
                Invoke(nameof(ShowPopup), 0.75f);
            else if (IsGrounded())
            {
                airMultiplierPopup.SetActive(false);
                CancelInvoke(nameof(ShowPopup));

                switch (_airMultiplierFilled)
                {
                    case false:
                        Invoke(nameof(ResetAirMultiplier), 1f);
                        break;
                }
            }
            
            switch (airMultiplierSlider.value)
                {
                    case >= 100:
                        abilityText.SetActive(true);
                        _airMultiplierFilled = true;
                        BlitEffect(true);

                        if (Input.GetButtonDown("Fire2"))
                            Ability();
                        break;
                    case <= 100:
                        _airMultiplierFilled = true;
                        break;
                }
        }

        void Ability()
        {
            abilityText.SetActive(false);
            BlitEffect(false);
            airMultiplierSlider.value = 0;
            _airMultiplier = 0;
            AbilityUsed = true;
            scoreMultiplierPopup.SetActive(true);
            Invoke(nameof(AbilityFalse), 20f);
            if (AudioManager.Instance != null)
                AudioManager.Instance.Play("LaserBeam");
        }

        void AbilityFalse()
        {
            AbilityUsed = false;
            scoreMultiplierPopup.SetActive(false);
        }

        void ShowPopup()
        {
            _airMultiplier += 1 * 0.15f;
            airMultiplierSlider.value = _airMultiplier;
            airMultiplierPopup.SetActive(true);
        }

        void ResetAirMultiplier()
        {
            airMultiplierSlider.value = 0;
            _airMultiplier = 0;
        }
        
        void BlitEffect(bool boolean)
        {
            blitRender.SetActive(boolean);
        }

        #endregion

        #region Car Flipping

        bool Flipped()
        {
            return Physics.CheckSphere(topCheck.transform.position, flipRadius, groundLayer);
        }

        IEnumerator Flip()
        {
            yield return new WaitForSeconds(0.5f);
            Quaternion desired = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, desired, rotationSpeed * Time.deltaTime);
        }

        #endregion

        #region Properties of car parameters

        float _maxMotorTorque;
        float MaxSteerAngle => CarConfig.MaxSteerAngle;
        DriveType DriveType => CarConfig.DriveType;
        bool AutomaticGearBox => CarConfig.AutomaticGearBox;
        AnimationCurve MotorTorqueFromRpmCurve => CarConfig.MotorTorqueFromRpmCurve;
        float MaxRpm => CarConfig.MaxRPM;
        float MinRpm => CarConfig.MinRPM;
        float CutOffRpm => CarConfig.CutOffRPM;
        float CutOffOffsetRpm => CarConfig.CutOffOffsetRPM;
        float RpmToNextGear => CarConfig.RpmToNextGear;
        float RpmToPrevGear => CarConfig.RpmToPrevGear;
        float MaxForwardSlipToBlockChangeGear => CarConfig.MaxForwardSlipToBlockChangeGear;
        float RpmEngineToRpmWheelsLerpSpeed => CarConfig.RpmEngineToRpmWheelsLerpSpeed;
        float[] GearsRatio => CarConfig.GearsRatio;
        float MainRatio => CarConfig.MainRatio;
        float ReversGearRatio => CarConfig.ReversGearRatio;
        float MaxBrakeTorque => CarConfig.MaxBrakeTorque;

        #endregion

        #region Properties of drift Settings

        bool EnableSteerAngleMultiplier => CarConfig.EnableSteerAngleMultiplier;
        float MinSteerAngleMultiplier => CarConfig.MinSteerAngleMultiplier;
        float MaxSteerAngleMultiplier => CarConfig.MaxSteerAngleMultiplier;
        float MaxSpeedForMinAngleMultiplier => CarConfig.MaxSpeedForMinAngleMultiplier;
        float SteerAngleChangeSpeed => CarConfig.SteerAngleChangeSpeed;
        float MinSpeedForSteerHelp => CarConfig.MinSpeedForSteerHelp;
        float HelpSteerPower => CarConfig.HelpSteerPower;
        float OppositeAngularVelocityHelpPower => CarConfig.OppositeAngularVelocityHelpPower;
        float PositiveAngularVelocityHelpPower => CarConfig.PositiveAngularVelocityHelpPower;
        float MaxAngularVelocityHelpAngle => CarConfig.MaxAngularVelocityHelpAngle;
        float AngularVelocityInMaxAngle => CarConfig.AngularVelucityInMaxAngle;
        float AngularVelocityInMinAngle => CarConfig.AngularVelucityInMinAngle;

        #endregion

        #region Steer help logic

        //Angle between forward point and velocity point.
        public float VelocityAngle { get; private set; }

        void UpdateSteerAngleLogic()
        {
            bool needHelp = SpeedInHour > MinSpeedForSteerHelp && CarDirection > 0;
            float targetAngle = 0;

            VelocityAngle =
                -Vector3.SignedAngle(Rb.velocity, transform.TransformDirection(Vector3.forward), Vector3.up);

            if (needHelp)
                //Wheel turning helper.
                targetAngle = Mathf.Clamp(VelocityAngle * HelpSteerPower, -MaxSteerAngle, MaxSteerAngle);

            //Wheel turn limitation.
            targetAngle = Mathf.Clamp(targetAngle + _currentSteerAngle, -(MaxSteerAngle + 10), MaxSteerAngle + 10);

            //Front wheel turn.
            Wheels[0].WheelCollider.steerAngle = targetAngle;
            Wheels[1].WheelCollider.steerAngle = targetAngle;

            if (!needHelp) return;
            //Angular velocity helper.
            float absAngle = Mathf.Abs(VelocityAngle);

            //Get current percent help angle.
            float currentAngularPercent = absAngle / MaxAngularVelocityHelpAngle;

            Vector3 currAngle = Rb.angularVelocity;

            if (VelocityAngle * _currentSteerAngle > 0)
            {
                //Turn to the side opposite to the angle. To change the angular velocity.
                float angularVelocityMagnitudeHelp =
                    OppositeAngularVelocityHelpPower * _currentSteerAngle * Time.fixedDeltaTime;

                currAngle.y += angularVelocityMagnitudeHelp * currentAngularPercent;
            }
            else if (!Mathf.Approximately(_currentSteerAngle, 0))
            {
                //Turn to the side positive to the angle. To change the angular velocity.
                float angularVelocityMagnitudeHelp =
                    PositiveAngularVelocityHelpPower * _currentSteerAngle * Time.fixedDeltaTime;

                currAngle.y += angularVelocityMagnitudeHelp * (1 - currentAngularPercent);
            }

            //Clamp and apply of angular velocity.
            float maxMagnitude = (AngularVelocityInMaxAngle - AngularVelocityInMinAngle) * currentAngularPercent +
                                 AngularVelocityInMinAngle;

            currAngle.y = Mathf.Clamp(currAngle.y, -maxMagnitude, maxMagnitude);
            Rb.angularVelocity = currAngle;
        }

        #endregion

        #region Rpm and torque logic

        public int CurrentGear { get; private set; }
        int CurrentGearIndex => CurrentGear + 1;
        public float EngineRpm { get; private set; }
        public float GetMaxRpm => MaxRpm;
        float GetInCutOffRpm => CutOffRpm - CutOffOffsetRpm;

        float _cutOffTimer;
        bool _inCutOff;

        void UpdateRpmAndTorqueLogic()
        {
            if (_inCutOff)
            {
                if (_cutOffTimer > 0)
                {
                    _cutOffTimer -= Time.fixedDeltaTime;

                    EngineRpm = Mathf.Lerp(EngineRpm, GetInCutOffRpm,
                        RpmEngineToRpmWheelsLerpSpeed * Time.fixedDeltaTime);
                }
                else
                    _inCutOff = false;
            }

            if (!GameController.RaceIsStarted)
            {
                if (_inCutOff)
                    return;

                float rpm = _currentAcceleration > 0 ? MaxRpm : MinRpm;

                float speed = _currentAcceleration > 0
                    ? RpmEngineToRpmWheelsLerpSpeed
                    : RpmEngineToRpmWheelsLerpSpeed * 0.2f;

                EngineRpm = Mathf.Lerp(EngineRpm, rpm, speed * Time.fixedDeltaTime);

                if (EngineRpm >= CutOffRpm)
                {
                    _inCutOff = true;
                    _cutOffTimer = CarConfig.CutOffTime;
                }

                return;
            }

            //Get drive wheel with MinRPM.
            float minRpm = 0;

            for (int i = _firstDriveWheel + 1; i <= _lastDriveWheel; i++)
                minRpm += Wheels[i].WheelCollider.rpm;

            minRpm /= _lastDriveWheel - _firstDriveWheel + 1;

            if (!_inCutOff)
            {
                //Calculate the rpm based on rpm of the wheel and current gear ratio.
                float targetRpm =
                    ((minRpm + 20) * _allGearsRatio[CurrentGearIndex]).Abs(); //+20 for normal work CutOffRPM

                targetRpm = Mathf.Clamp(targetRpm, MinRpm, MaxRpm);
                EngineRpm = Mathf.Lerp(EngineRpm, targetRpm, RpmEngineToRpmWheelsLerpSpeed * Time.fixedDeltaTime);
            }

            if (EngineRpm >= CutOffRpm)
            {
                _inCutOff = true;
                _cutOffTimer = CarConfig.CutOffTime;
                return;
            }

            if (!Mathf.Approximately(_currentAcceleration, 0))
            {
                //If the direction of the car is the same as Current Acceleration.
                if (CarDirection * _currentAcceleration >= 0)
                {
                    _currentBrake = 0;

                    float motorTorqueFromRpm = MotorTorqueFromRpmCurve.Evaluate(EngineRpm * 0.001f);

                    float motorTorque = _currentAcceleration *
                                        (motorTorqueFromRpm * (_maxMotorTorque * _allGearsRatio[CurrentGearIndex]));

                    if (Mathf.Abs(minRpm) * _allGearsRatio[CurrentGearIndex] > MaxRpm)
                        motorTorque = 0;

                    //If the rpm of the wheel is less than the max rpm engine * current ratio, then apply the current torque for wheel, else not torque for wheel.
                    float maxWheelRpm = _allGearsRatio[CurrentGearIndex] * EngineRpm;

                    for (int i = _firstDriveWheel; i <= _lastDriveWheel; i++)
                        if (Wheels[i].WheelCollider.rpm <= maxWheelRpm)
                            Wheels[i].WheelCollider.motorTorque = motorTorque;
                        else
                            Wheels[i].WheelCollider.motorTorque = 0;
                }
                else
                    _currentBrake = MaxBrakeTorque;
            }
            else
            {
                _currentBrake = 0;

                for (int i = _firstDriveWheel; i <= _lastDriveWheel; i++)
                    Wheels[i].WheelCollider.motorTorque = 0;
            }

            //Automatic gearbox logic. 
            if (!AutomaticGearBox) return;

            {
                bool forwardIsSlip = false;

                for (int i = _firstDriveWheel; i <= _lastDriveWheel; i++)
                {
                    if (!(Wheels[i].CurrentForwardSleep > MaxForwardSlipToBlockChangeGear)) continue;
                    forwardIsSlip = true;
                    break;
                }

                float prevRatio = 0;
                float newRatio = 0;

                if (!forwardIsSlip && EngineRpm > RpmToNextGear && CurrentGear >= 0 &&
                    CurrentGear < _allGearsRatio.Length - 2)
                {
                    prevRatio = _allGearsRatio[CurrentGearIndex];
                    CurrentGear++;
                    newRatio = _allGearsRatio[CurrentGearIndex];
                }
                else if (EngineRpm < RpmToPrevGear && CurrentGear > 0 && (EngineRpm <= MinRpm || CurrentGear != 1))
                {
                    prevRatio = _allGearsRatio[CurrentGearIndex];
                    CurrentGear--;
                    newRatio = _allGearsRatio[CurrentGearIndex];
                }

                if (!Mathf.Approximately(prevRatio, 0) && !Mathf.Approximately(newRatio, 0))
                    EngineRpm = Mathf.Lerp(EngineRpm, EngineRpm * (newRatio / prevRatio),
                        RpmEngineToRpmWheelsLerpSpeed * Time.fixedDeltaTime); //EngineRPM * (prevRatio / newRatio);// 

                if (CarDirection <= 0 && _currentAcceleration < 0)
                    CurrentGear = -1;
                else if (CurrentGear <= 0 && CarDirection >= 0 && _currentAcceleration > 0)
                    CurrentGear = 1;
                else if (CarDirection == 0 && _currentAcceleration == 0)
                    CurrentGear = 0;
            }
        }

        #endregion
    }


    [Serializable]
    public class CarConfig
    {
        [Header("Steer Settings")]
        public float MaxSteerAngle = 25;

        [Header("Engine and power settings")]
        public DriveType DriveType = DriveType.RWD;
        public bool AutomaticGearBox = true;
        public float MaxMotorTorque = 150;
        public AnimationCurve MotorTorqueFromRpmCurve;
        public float MaxRPM = 7000;
        public float MinRPM = 700;
        public float CutOffRPM = 6800;
        public float CutOffOffsetRPM = 500;
        public float CutOffTime = 0.1f;
        [Range(0, 1)]
        public float ProbabilityBackfire = 0.2f;
        public float RpmToNextGear = 6500;
        public float RpmToPrevGear = 4500;
        public float MaxForwardSlipToBlockChangeGear = 0.5f;
        public float RpmEngineToRpmWheelsLerpSpeed = 15;
        public float[] GearsRatio;
        public float MainRatio;
        public float ReversGearRatio;

        [Header("Braking settings")]
        public float MaxBrakeTorque = 1000;

        [Header("Helper settings")]
        public bool EnableSteerAngleMultiplier = true;
        public float MinSteerAngleMultiplier = 0.05f;
        public float MaxSteerAngleMultiplier = 1f;
        public float MaxSpeedForMinAngleMultiplier = 250;
        [Space(10)]
        public float SteerAngleChangeSpeed;
        public float MinSpeedForSteerHelp;
        [Range(0f, 1f)]
        public float HelpSteerPower;
        public float OppositeAngularVelocityHelpPower = 0.1f;
        public float PositiveAngularVelocityHelpPower = 0.1f;
        public float MaxAngularVelocityHelpAngle;
        public float AngularVelucityInMaxAngle;
        public float AngularVelucityInMinAngle;
    }
}
