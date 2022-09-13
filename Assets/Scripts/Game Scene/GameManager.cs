using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        static readonly int IsPaused = Animator.StringToHash("isPaused");
        static readonly int IsResumed = Animator.StringToHash("isResumed");
        static readonly int DeathStart = Animator.StringToHash("DeathStart");

        [Header("Pause Mode")]
        public GameObject pauseUI;
        public Animator pauseAnimator;
        
        [Header("Audio Sources")]
        public AudioSource carEngine;
        public AudioSource carDestructionSound;

        [Space(5f)]
        [Header("Death Mode")]
        public TextMeshProUGUI inGameKillCountText;
        public TextMeshProUGUI killCountText;
        public Animator deathAnim;

        [HideInInspector]
        public bool gamePaused;
        [HideInInspector]
        public bool carDestroyed;

        TheCamera _camScript;
        TheWeapon _weaponScript;
        int _killCounter;
        int _inGameKillCounter;

        public static int _score;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            _score = 0;
            _camScript = FindObjectOfType<TheCamera>();
            _weaponScript = FindObjectOfType<TheWeapon>();

            if (pauseUI != null)
                pauseUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                TheHealth.Instance.TakeDamage(15);
            
            if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused && !carDestroyed)
                PauseGame();
            else if (Input.GetKeyDown(KeyCode.Escape) && gamePaused && !carDestroyed)
                ResumeGame();

            if (!carDestroyed) return;
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
                Application.Quit();

            int.TryParse(killCountText.text, out _killCounter);
            _killCounter = PlayerPrefs.GetInt("KillCounter");
            int.TryParse(inGameKillCountText.text, out _inGameKillCounter);
            
            if (killCountText != null)
                killCountText.text = _inGameKillCounter.ToString();

        }

        void PauseGame()
        {
            gamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            carEngine.Pause();

            if (pauseAnimator != null)
                pauseAnimator.SetTrigger(IsPaused);

            Time.timeScale = 0f;
            _camScript.enabled = false;
            _weaponScript.enabled = false;
        }

        public void ResumeGame()
        {
            gamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            carEngine.Play();

            if (pauseAnimator != null)
                pauseAnimator.SetTrigger(IsResumed);

            Time.timeScale = 1f;
            _camScript.enabled = true;
            _weaponScript.enabled = true;
        }

        void CarDestroyed()
        {
            if (deathAnim != null)
                deathAnim.SetTrigger(DeathStart);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            _camScript.enabled = false;
            _weaponScript.enabled = false;
            carDestructionSound.Play();
        }
    }
}
