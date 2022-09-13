using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XG.Studios;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class UIStuff : MonoBehaviour
    {
        const string QualityKey = "quality";
        
        [Header("Misc")]
        public Volume volume;
        public Slider musicVolSlider;
        public Slider sfxVolSlider;
        public Slider motionBlurSlider;
        public Slider filmGrainSlider;
        public TMP_Dropdown qualityDropdown;
        public TextMeshProUGUI motionBlurText;
        public TextMeshProUGUI warningTxt;
        public TextMeshProUGUI filmGrainText;
        public RenderPipelineAsset[] qualityLevels;
        
        [Header("Pause UI Buttons")]
        public Button resumeButton;
        public Button restartButton;
        public Button quitButton;
        
        MotionBlur _motionBlur;
        FilmGrain _filmGrain;

        [Header("Audio Stuff")]
        [SerializeField] TextMeshProUGUI musicVolumeText;
        [SerializeField] TextMeshProUGUI soundEffectsVolumeText;
        
        public static float MusicVolume { get; private set; }
        public static float SoundEffectsVolume { get; private set; }

        void Awake()
        {
            qualityDropdown.onValueChanged.AddListener(i =>
            {
                PlayerPrefs.SetInt(QualityKey, qualityDropdown.value);
                PlayerPrefs.Save();
            });
        }
        
        void Start()
        {
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            volume.profile.TryGet(out _motionBlur);
            volume.profile.TryGet(out _filmGrain);
            
            musicVolSlider.value = PlayerPrefs.GetFloat("musicVol", 0.15f);
            sfxVolSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.15f);
            motionBlurSlider.value = PlayerPrefs.GetFloat("motionBlur", 0.15f);
            filmGrainSlider.value = PlayerPrefs.GetFloat("filmGrain", 0.25f);
            qualityDropdown.value = PlayerPrefs.GetInt(QualityKey, 2);

            if (resumeButton != null)
                resumeButton.onClick.AddListener(() => { GameManager.Instance.ResumeGame(); });

            if (restartButton != null)
                restartButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });

            if (quitButton != null)
                quitButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("Main Menu");
                });
        }

        public void OnMusicSliderValueChange(float value)
        {
            MusicVolume = value;
            musicVolumeText.text = $"{((int)(value * 100))}%";
            AudioManager.Instance.UpdateMixerVolume();
            PlayerPrefs.SetFloat("musicVol", value);
        }
        
        public void OnSoundEffectsSliderValueChange(float value)
        {
            SoundEffectsVolume = value;
            soundEffectsVolumeText.text = $"{((int)(value * 100))}%";
            AudioManager.Instance.UpdateMixerVolume();
            PlayerPrefs.SetFloat("sfxVol", value);
        }

        public void MotionBlurIntensity(float value)
        {
            _motionBlur.intensity.value = value;
            motionBlurText.text = $"{((int)(value * 100))}%";
            PlayerPrefs.SetFloat("motionBlur", value);

            switch (value)
            {
                case >= 0.5f:
                    warningTxt.SetActive(true);
                    break;
                case < 0.5f:
                    warningTxt.SetActive(false);
                    break;
            }
        }
        
        public void FilmGrainIntensity(float value)
        {
            _filmGrain.intensity.value = value;
            filmGrainText.text = $"{((int)(value * 100))}%";
            PlayerPrefs.SetFloat("filmGrain", value);
        }
        
        public void GraphicsQuality(int value)
        {
            QualitySettings.SetQualityLevel(value);
            GraphicsSettings.renderPipelineAsset = qualityLevels[value];
        }
    }
}
