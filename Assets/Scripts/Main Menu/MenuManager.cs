using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace XGStudios
{
    public class MenuManager : MonoBehaviour
    {
        static readonly int StartGame = Animator.StringToHash("StartGame");
        static readonly int CreditsMoving = Animator.StringToHash("CreditsMoving");
        [Header("Strings")]
        public string nextScene;

        [Space(5f)]
        [Header("UI Panels")]
        public CanvasGroup menuCanvas;
        public RectTransform menuRect;
        public CanvasGroup settingsCanvas;
        public RectTransform settingsRect;
        public CanvasGroup creditsCanvas;
        public RectTransform creditsRect;
        public CanvasGroup leaderBoardCanvas;
        public RectTransform leaderBoardRect;

        [Space(5f)]
        [Header("Misc")]
        public Animator carAnim;
        public Animator creditsAnim;

        [Header("Texts")]
        public TextMeshProUGUI leaderBoardTxt;

        [Header("Menu Buttons")]
        public Button playButton;
        public Button settingsButton;
        public Button creditsButton;
        public Button leaderBoardBtn;
        public Button quitButton;

        void Start()
        {
            leaderBoardTxt.SetText("LEADERBOARD");
            creditsAnim.enabled = false;

            if (playButton != null)
                playButton.onClick.AddListener(() =>
                {
                    carAnim.SetTrigger(StartGame);
                    Invoke(nameof(StartTheGame), 1f);
                });

            if (settingsButton != null)
                settingsButton.onClick.AddListener(() =>
                {
                    settingsCanvas.alpha = 0;
                    settingsCanvas.interactable = true;
                    settingsCanvas.blocksRaycasts = true;
                    settingsRect.transform.localPosition = new Vector3(0, -1000f, 0);
                    settingsRect.DOAnchorPos(new Vector2(0, 0), 0.2f).SetEase(Ease.OutExpo);
                    settingsCanvas.DOFade(1, 0.2f);
                });

            if (creditsButton != null)
                creditsButton.onClick.AddListener(() =>
                {
                    creditsAnim.enabled = true;
                    creditsAnim.SetTrigger(CreditsMoving);
                    creditsCanvas.alpha = 0;
                    creditsCanvas.interactable = true;
                    creditsCanvas.blocksRaycasts = true;
                    creditsRect.transform.localPosition = new Vector3(0, -1000f, 0);
                    creditsRect.DOAnchorPos(new Vector2(0, 0), 0.2f).SetEase(Ease.OutExpo);
                    creditsCanvas.DOFade(1, 0.2f);
                });

            if (leaderBoardBtn != null)
                leaderBoardBtn.onClick.AddListener(() =>
                {
                    leaderBoardTxt.SetText("REFRESH");
                    leaderBoardCanvas.alpha = 0;
                    leaderBoardCanvas.interactable = true;
                    leaderBoardCanvas.blocksRaycasts = true;
                    leaderBoardRect.transform.localPosition = new Vector3(600, -1000f, 0);
                    leaderBoardRect.DOAnchorPos(new Vector2(0, 0), 0.2f).SetEase(Ease.OutExpo);
                    leaderBoardCanvas.DOFade(1, 0.2f);
                });

            if (quitButton != null)
                quitButton.onClick.AddListener(() =>
                {
                    menuCanvas.alpha = 1;
                    menuCanvas.interactable = false;
                    menuCanvas.blocksRaycasts = false;
                    menuRect.transform.localPosition = new Vector3(0, 0, 0);
                    menuRect.DOAnchorPos(new Vector2(0, -1000f), 0.2f).SetEase(Ease.OutExpo);
                    menuCanvas.DOFade(0, 0.2f);
                    Invoke(nameof(QuitGame), 1f);
                });
        }

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;

            if (settingsCanvas.alpha != 0)
            {
                settingsCanvas.alpha = 1;
                settingsCanvas.interactable = false;
                settingsCanvas.blocksRaycasts = false;
                settingsRect.transform.localPosition = new Vector3(0, 0, 0);
                settingsRect.DOAnchorPos(new Vector2(0, -1000f), 0.2f).SetEase(Ease.OutExpo);
                settingsCanvas.DOFade(0, 0.2f);
            }

            if (creditsCanvas.alpha != 0)
            {
                creditsAnim.enabled = false;
                creditsCanvas.alpha = 1;
                creditsCanvas.interactable = false;
                creditsCanvas.blocksRaycasts = false;
                creditsRect.transform.localPosition = new Vector3(0, 0, 0);
                creditsRect.DOAnchorPos(new Vector2(0, -1000f), 0.2f).SetEase(Ease.OutExpo);
                creditsCanvas.DOFade(0, 0.2f);
            }
        }

        void QuitGame()
        {
            Application.Quit();
        }

        public void StartTheGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
