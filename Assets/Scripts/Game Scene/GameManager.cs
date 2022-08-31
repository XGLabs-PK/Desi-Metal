using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Sentry;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [Header("Pause Mode")]
        public GameObject pauseUI;
        public Animator pauseAnimator;
        
        [Space(5f)]
        
        [Header("Death Mode")]
        public Animator deathAnim;
        
        [Space(5f)]
        
        [Header("Scripts Reference")]
        [Tooltip("Scripts to disable when in Pause Mode or Car Gets Destroyed")]
        public TheCamera camScript;
        [Tooltip("Scripts to disable when in Pause Mode or Car Gets Destroyed")]
        public TheWeapon weaponScript;

        [HideInInspector]
        public bool gamePaused;
        [HideInInspector]
        public bool carDestroyed;
        
        static readonly int IsPaused = Animator.StringToHash("isPaused");
        static readonly int IsResumed = Animator.StringToHash("isResumed");
        static readonly int DeathStart = Animator.StringToHash("DeathStart");

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
            SentrySdk.CaptureMessage("Test event");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused && !carDestroyed)
            {
                PauseGame();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && gamePaused && !carDestroyed)
            {
                ResumeGame(); 
            }
            
            //DEBUG REMOVE IT
            if (Input.GetKeyDown(KeyCode.C))
                TheHealth.Instance.TakeDamage(25);

            if (carDestroyed)
            {
                CarDestroyed();
                
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("Main Menu");
                }
                else if (Input.GetKeyDown(KeyCode.F3))
                {
                    Application.Quit();
                }
            }
        }

        void PauseGame()
        {
            gamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseAnimator.SetTrigger(IsPaused);
            Time.timeScale = 0f;
            camScript.enabled = false;
            weaponScript.enabled = false;
        }

        public void ResumeGame()
        {
            gamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseAnimator.SetTrigger(IsResumed);
            Time.timeScale = 1f;
            camScript.enabled = true;
            weaponScript.enabled = true;
        }

        void CarDestroyed()
        {
            //Car Gets Destroyed
            //Particles
            //Sound
            deathAnim.SetTrigger(DeathStart);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            camScript.enabled = false;
            weaponScript.enabled = false;
        }
    }
}
