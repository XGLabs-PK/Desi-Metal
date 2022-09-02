using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XGStudios
{
    public class SettingsMenu : MonoBehaviour
    {
        public TMP_Dropdown resolutionDropdown;
                public Toggle fullScreenToggle;
                
                Resolution[] _resolutions;
                int _screenInt;
                const String _resName = "resolutionOption";
        
                void Awake()
                {
                    _screenInt = PlayerPrefs.GetInt("ToggleState");
        
                    if (_screenInt == 1)
                    {
                        fullScreenToggle.isOn = true;
                    }
                    else
                        fullScreenToggle.isOn = false;
                    
                    resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
                    {
                        PlayerPrefs.SetInt(_resName, resolutionDropdown.value);
                        PlayerPrefs.Save();
                    }));
                }
        
                void Start()
                {
                    _resolutions = Screen.resolutions;
                    resolutionDropdown.ClearOptions();
                    
                    List<String> options = new List<string>();
        
                    int currResIndex = 0;
                    for (int i = 0; i < _resolutions.Length; i++)
                    {
                        string option = _resolutions[i].width + " x " + _resolutions[i].height + " - " + _resolutions[i].refreshRate + " hz";
                        options.Add(option);
        
                        if (_resolutions[i].width == Screen.currentResolution.width &&
                            _resolutions[i].height == Screen.currentResolution.height)
                            currResIndex = i;
                    }
                    
                    resolutionDropdown.AddOptions(options);
                    resolutionDropdown.value = PlayerPrefs.GetInt(_resName, currResIndex);
                    resolutionDropdown.RefreshShownValue();
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
