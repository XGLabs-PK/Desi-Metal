using TMPro;
using UnityEngine;
using XGStudios;

/// <summary>
///     Only visual on UI logic.
///     Speedometer, tachometer and current gear information.
/// </summary>
public class CarStateUI : MonoBehaviour
{
    [SerializeField]
    int UpdateFrameCount = 3;

    [SerializeField]
    RectTransform TahometerArrow;
    [SerializeField]
    float MinArrowAngle;
    [SerializeField]
    float MaxArrowAngle = -315f;

    int CurrentFrame;
    [SerializeField]
    TextMeshProUGUI CurrentGearText;
    [SerializeField]
    TextMeshProUGUI SpeedText;
    CarController SelectedCar => GameController.PlayerCar;

    void Update()
    {

        if (CurrentFrame >= UpdateFrameCount)
        {
            UpdateGamePanel();
            CurrentFrame = 0;
        }
        else
            CurrentFrame++;

        UpdateArrow();
    }

    void UpdateArrow()
    {
        float procent = SelectedCar.EngineRpm / SelectedCar.GetMaxRpm;
        float angle = (MaxArrowAngle - MinArrowAngle) * procent + MinArrowAngle;
        TahometerArrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void UpdateGamePanel()
    {
        SpeedText.text = SelectedCar.SpeedInHour.ToString("000");
        CurrentGearText.text = SelectedCar.CurrentGear.ToString();
    }
}
