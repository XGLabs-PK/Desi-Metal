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
        public RenderPipelineAsset[] qualityLevels;
        FilmGrain _filmGrain;

        MotionBlur _motionBlur;
        public Slider filmGrainSlider;
        public TextMeshProUGUI filmGrainText;
        public Slider motionBlurSlider;
        public TextMeshProUGUI motionBlurText;
        public Slider musicVolSlider;

        [Header("Audio Stuff")]
        [SerializeField]
        TextMeshProUGUI musicVolumeText;
        public TMP_Dropdown qualityDropdown;
        public Button quitButton;
        public Button restartButton;

        [Header("Pause UI Buttons")]
        public Button resumeButton;
        public Slider sfxVolSlider;
        [SerializeField]
        TextMeshProUGUI soundEffectsVolumeText;

        [Header("Misc")]
        public Volume volume;
        public TextMeshProUGUI warningTxt;

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
            sfxVolSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.25f);
            motionBlurSlider.value = PlayerPrefs.GetFloat("motionBlur", 0.1f);
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
                    SceneManager.LoadScene("MainMenu");
                });
        }

        public void OnMusicSliderValueChange(float value)
        {
            MusicVolume = value;
            musicVolumeText.text = $"{(int)(value * 100)}%";
            AudioManager.Instance.UpdateMixerVolume();
            PlayerPrefs.SetFloat("musicVol", value);
        }

        public void OnSoundEffectsSliderValueChange(float value)
        {
            SoundEffectsVolume = value;
            soundEffectsVolumeText.text = $"{(int)(value * 100)}%";
            AudioManager.Instance.UpdateMixerVolume();
            PlayerPrefs.SetFloat("sfxVol", value);
        }

        public void MotionBlurIntensity(float value)
        {
            _motionBlur.intensity.value = value;
            motionBlurText.text = $"{(int)(value * 100)}%";
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
            filmGrainText.text = $"{(int)(value * 100)}%";
            PlayerPrefs.SetFloat("filmGrain", value);
        }

        public void GraphicsQuality(int value)
        {
            QualitySettings.SetQualityLevel(value);
            GraphicsSettings.renderPipelineAsset = qualityLevels[value];
        }
    }
}
