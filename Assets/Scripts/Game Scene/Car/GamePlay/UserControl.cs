using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGStudios;

/// <summary>
/// For user multiplatform control.
/// </summary>
[RequireComponent(typeof(CarController))]
public class UserControl : MonoBehaviour
{
    CarController ControlledCar;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool Brake { get; private set; }

    void Awake()
    {
        ControlledCar = GetComponent<CarController>();
    }

    void Update()
    {
        //Standart input control (Keyboard or gamepad).
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
            Brake = Input.GetButton("Jump");

            //Apply control for controlled car.
        ControlledCar.UpdateControls(Horizontal, Vertical, Brake);
    }
}
