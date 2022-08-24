using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace XGStudios.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Strings")]
        public string gameScene;
        
        [Header("Menu Buttons")]
        public Button playButton;
        public Button settingsButton;
        public Button creditsButton;
        public Button aboutUsButton;
        public Button quitButton;
        
        [Space(5f)]
        [Header("UI Panels")]
        public GameObject settingsPanel;
        public GameObject creditsPanel;
        public GameObject aboutUsPanel;

        void Start()
        {
            if (playButton != null)
            {
                playButton.onClick.AddListener(() => {
                    SceneManager.LoadScene(gameScene);
                });
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.AddListener(() => {
                    settingsPanel.SetActive(true);
                });
            }
            
            if (creditsButton != null)
            {
                creditsButton.onClick.AddListener(() => {
                    creditsPanel.SetActive(true);
                });
            }
            
            if (aboutUsButton != null)
            {
                aboutUsButton.onClick.AddListener(() => {
                    aboutUsPanel.SetActive(true);
                });
            }
            
            if (quitButton != null)
                quitButton.onClick.AddListener(Application.Quit);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (settingsPanel.activeSelf)
                    settingsPanel.SetActive(false);
                if (creditsPanel.activeSelf)
                    creditsPanel.SetActive(false);
                if (aboutUsPanel.activeSelf)
                    aboutUsPanel.SetActive(false);
            }

        }
    }
}
