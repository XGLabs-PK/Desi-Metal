using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Mobile input arrows.
/// </summary>
public class MobileControlUI : MonoBehaviour
{
    [SerializeField]
    CustomButton TurnLeftButton;
    [SerializeField]
    CustomButton TurnRigthButton;
    [SerializeField]
    CustomButton AccelerationButton;
    [SerializeField]
    CustomButton DecelerationButton;

    bool LeftPressed => TurnLeftButton.ButtonIsPressed;
    bool RightPressed => TurnRigthButton.ButtonIsPressed;
    bool AccelerationPressed => AccelerationButton.ButtonIsPressed;
    bool DecelerationPressed => DecelerationButton.ButtonIsPressed;
    public bool ControlInUse => LeftPressed || RightPressed || AccelerationPressed || DecelerationPressed;

    public float GetHorizontalAxis
    {
        get
        {
            if (LeftPressed)
                return -1;
            else if (RightPressed)
                return 1;

            return 0;
        }
    }

    public float GetVerticalAxis
    {
        get
        {
            if (AccelerationPressed)
                return 1;
            else if (DecelerationPressed)
                return -1;

            return 0;
        }
    }
}
