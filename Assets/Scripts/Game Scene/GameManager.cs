using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public static GameManager Instance;
        
        [Header("Pause Mode")]
        public GameObject pauseUI;
        public GameObject pauseScreen;
        public GameObject advSettingsScreen;
        public Animator pauseAnimator;
        
        [Space(5f)]
        
        [Header("Scripts Reference")]
        [Tooltip("Scripts to disable when in Pause Mode or Car Gets Destroyed")]
        public TheCamera camScript;
        [Tooltip("Scripts to disable when in Pause Mode or Car Gets Destroyed")]
        public TheWeapon weaponScript;

        bool _gamePaused;
        bool _carDestroyed;
        static readonly int IsPaused = Animator.StringToHash("isPaused");
        static readonly int IsResumed = Animator.StringToHash("isResumed");

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_gamePaused)
            {
                PauseGame();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && _gamePaused)
            {
                ResumeGame(); 
            }
                
        }

        void PauseGame()
        {
            _gamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseAnimator.SetTrigger(IsPaused);
            Time.timeScale = 0f;
            camScript.enabled = false;
            weaponScript.enabled = false;
        }

        public void ResumeGame()
        {
            _gamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseAnimator.SetTrigger(IsResumed);
            Time.timeScale = 1f;
            camScript.enabled = true;
            weaponScript.enabled = true;
        }

        public void AdvSettings()
        {
            pauseScreen.gameObject.SetActive(false);
            advSettingsScreen.gameObject.SetActive(true);
        }
        
        public void GoBack()
        {
            pauseScreen.gameObject.SetActive(true);
            advSettingsScreen.gameObject.SetActive(false);
        }
    }
}
