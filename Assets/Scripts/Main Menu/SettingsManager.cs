using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace XGStudios
{
    public class SettingsManager : MonoBehaviour
    {
        const string QualityKey = "quality";
        const string ResName = "resolutionOption";
        public RenderPipelineAsset[] qualityLevels;
        public CanvasGroup controlsCanvas;
        public RectTransform controlsRect;

        int _screenInt;
        int _vsyncInt;
        Resolution[] _resolutions;
        public Button controlsBtn;
        public Toggle fullScreenToggle;
        public Toggle vsyncToggle;
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown qualityDropdown;

        void Awake()
        {
            _screenInt = PlayerPrefs.GetInt("ToggleState");
            _vsyncInt = PlayerPrefs.GetInt("VsyncToggleState");
            fullScreenToggle.isOn = _screenInt == 1;
            vsyncToggle.isOn = _vsyncInt == 2;
            
            resolutionDropdown.onValueChanged.AddListener(index =>
            {
                PlayerPrefs.SetInt(ResName, resolutionDropdown.value);
                PlayerPrefs.Save();
            });
            
            qualityDropdown.onValueChanged.AddListener(i =>
            {
                PlayerPrefs.SetInt(QualityKey, qualityDropdown.value);
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

            qualityDropdown.value = QualitySettings.GetQualityLevel();
            qualityDropdown.value = PlayerPrefs.GetInt(QualityKey, 2);

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

        public void SetVSync(bool vSyncToggle)
        {
            PlayerPrefs.SetInt("VsyncToggleState", vsyncToggle ? 2 : 0);
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        
        public void GraphicsQuality(int value)
        {
            QualitySettings.SetQualityLevel(value);
            GraphicsSettings.renderPipelineAsset = qualityLevels[value];
        }
    }
}
