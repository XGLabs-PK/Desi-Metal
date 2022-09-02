using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class game controller.
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    KeyCode NextCarKey = KeyCode.N;
    [SerializeField]
    UnityEngine.UI.Button NextCarButton;
    public static GameController Instance;
    public static CarController PlayerCar => Instance.m_PlayerCar;
    public static bool RaceIsStarted => true;
    public static bool RaceIsEnded => false;

    CarController m_PlayerCar;
    List<CarController> Cars = new List<CarController>();
    int CurrentCarIndex = 0;

    protected virtual void Awake()
    {

        Instance = this;

        //Find all cars in current game.
        Cars.AddRange(FindObjectsOfType<CarController>());
        Cars = Cars.OrderBy(c => c.name).ToList();

        foreach (CarController car in Cars)
        {
            UserControl userControl = car.GetComponent<UserControl>();
            AudioListener audioListener = car.GetComponent<AudioListener>();

            if (userControl == null)
                userControl = car.gameObject.AddComponent<UserControl>();

            if (audioListener == null)
                audioListener = car.gameObject.AddComponent<AudioListener>();

            userControl.enabled = false;
            audioListener.enabled = false;
        }

        m_PlayerCar = Cars[0];
        m_PlayerCar.GetComponent<UserControl>().enabled = true;
        m_PlayerCar.GetComponent<AudioListener>().enabled = true;

        if (NextCarButton)
            NextCarButton.onClick.AddListener(NextCar);
    }

    void Update()
    {
        if (Input.GetKeyDown(NextCarKey))
            NextCar();

    }

    void NextCar()
    {
        m_PlayerCar.GetComponent<UserControl>().enabled = false;
        m_PlayerCar.GetComponent<AudioListener>().enabled = false;

        CurrentCarIndex = MathExtentions.LoopClamp(CurrentCarIndex + 1, 0, Cars.Count);

        m_PlayerCar = Cars[CurrentCarIndex];
        m_PlayerCar.GetComponent<UserControl>().enabled = true;
        m_PlayerCar.GetComponent<AudioListener>().enabled = true;
    }
}
