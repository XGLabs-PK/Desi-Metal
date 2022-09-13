using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XGStudios
{
    public class SettingsMenu : MonoBehaviour
    {
        public Button controlsBtn;
        public CanvasGroup controlsCanvas;
        public RectTransform controlsRect;
        const string ResName = "resolutionOption";
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullScreenToggle;

        Resolution[] _resolutions;
        int _screenInt;

        void Awake()
        {
            _screenInt = PlayerPrefs.GetInt("ToggleState");

            if (_screenInt == 1)
                fullScreenToggle.isOn = true;
            else
                fullScreenToggle.isOn = false;

            resolutionDropdown.onValueChanged.AddListener(index =>
            {
                PlayerPrefs.SetInt(ResName, resolutionDropdown.value);
                PlayerPrefs.Save();
            });
        }

        void Start()
        {
            _resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            var options = new List<string>();

            int currResIndex = 0;

            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height + " - " +
                                _resolutions[i].refreshRate + " hz";

                options.Add(option);

                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                    currResIndex = i;
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt(ResName, currResIndex);
            resolutionDropdown.RefreshShownValue();
            
            //Control Btn
            if (controlsBtn != null)
                controlsBtn.onClick.AddListener(() =>
                {
                    controlsCanvas.alpha = 0;
                    controlsCanvas.interactable = true;
                    controlsCanvas.blocksRaycasts = true;
                    controlsRect.transform.localPosition = new Vector3(0, -1000f, 0);
                    controlsRect.DOAnchorPos(new Vector2(0, 0), 0.2f).SetEase(Ease.OutExpo);
                    controlsCanvas.DOFade(1, 0.2f);
                });
        }

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;

            if (controlsCanvas.alpha != 0)
            {
                controlsCanvas.alpha = 1;
                controlsCanvas.interactable = false;
                controlsCanvas.blocksRaycasts = false;
                controlsRect.transform.localPosition = new Vector3(0, 0, 0);
                controlsRect.DOAnchorPos(new Vector2(0, -1000f), 0.2f).SetEase(Ease.OutExpo);
                controlsCanvas.DOFade(0, 0.2f);
            }
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;

            if (isFullScreen == false)
                PlayerPrefs.SetInt("ToggleState", 0);
            else
            {
                isFullScreen = true;
                PlayerPrefs.SetInt("ToggleState", 1);
            }
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}
