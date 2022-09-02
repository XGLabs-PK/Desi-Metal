using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        TheCamera _camScript;
        TheWeapon _weaponScript;

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
            _camScript = FindObjectOfType<TheCamera>();
            _weaponScript = FindObjectOfType<TheWeapon>();
            if (pauseUI != null)
                pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
                TheHealth.Instance.TakeDamage(5);

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
        }
    }
}
