using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class UIStuff : MonoBehaviour
    {
        [Header("Pause UI Buttons")]
        public Button resumeButton;
        public Button restartButton;
        public Button quitButton;


        void Start()
        {
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
    }
}
