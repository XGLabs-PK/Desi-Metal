using System;
using DG.Tweening;
using TMPro;
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

        [Header("Texts")]
        public TextMeshProUGUI leaderBoardTxt;
        
        [Header("Menu Buttons")]
        public Button playButton;
        public Button settingsButton;
        public Button creditsButton;
        public Button aboutUsButton;
        public Button leaderBoardBtn;
        public Button quitButton;
        
        [Space(5f)]
        [Header("UI Panels")]
        public CanvasGroup settingsCanvas;
        public RectTransform settingsRect;
        public CanvasGroup creditsCanvas;
        public RectTransform creditsRect;
        public CanvasGroup aboutUsCanvas;
        public RectTransform aboutUsRect;
        public CanvasGroup leaderBoardCanvas;
        public RectTransform leaderBoardRect;

        void Start()
        {
            leaderBoardTxt.SetText("LEADERBOARD");
            
            if (playButton != null)
            {
                playButton.onClick.AddListener(() => {
                    SceneManager.LoadScene(gameScene);
                });
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.AddListener(() => {
                    settingsCanvas.alpha = 0;
                    settingsCanvas.interactable = true;
                    settingsCanvas.blocksRaycasts = true;
                    settingsRect.transform.localPosition = new Vector3(0, -1000f, 0);
                    settingsRect.DOAnchorPos(new Vector2(0,0), 0.2f, false).SetEase(Ease.OutExpo);
                    settingsCanvas.DOFade(1, 0.2f);
                });
            }
            
            if (creditsButton != null)
            {
                creditsButton.onClick.AddListener(() => {
                    creditsCanvas.alpha = 0;
                    creditsCanvas.interactable = true;
                    creditsCanvas.blocksRaycasts = true;
                    creditsRect.transform.localPosition = new Vector3(0, -1000f, 0);
                    creditsRect.DOAnchorPos(new Vector2(0,0), 0.2f, false).SetEase(Ease.OutExpo);
                    creditsCanvas.DOFade(1, 0.2f);
                });
            }
            
            if (aboutUsButton != null)
            {
                aboutUsButton.onClick.AddListener(() => {
                    aboutUsCanvas.alpha = 0;
                    aboutUsCanvas.interactable = true;
                    aboutUsCanvas.blocksRaycasts = true;
                    aboutUsRect.transform.localPosition = new Vector3(0, -1000f, 0);
                    aboutUsRect.DOAnchorPos(new Vector2(0,0), 0.2f, false).SetEase(Ease.OutExpo);
                    aboutUsCanvas.DOFade(1, 0.2f);
                });
            }
            
            if (leaderBoardBtn != null)
            {
                leaderBoardBtn.onClick.AddListener(() => {
                    leaderBoardTxt.SetText("REFRESH");
                    leaderBoardCanvas.alpha = 0;
                    leaderBoardCanvas.interactable = true;
                    leaderBoardCanvas.blocksRaycasts = true;
                    leaderBoardRect.transform.localPosition = new Vector3(600, -1000f, 0);
                    leaderBoardRect.DOAnchorPos(new Vector2(0,0), 0.2f, false).SetEase(Ease.OutExpo);
                    leaderBoardCanvas.DOFade(1, 0.2f);
                });
            }
            
            if (quitButton != null)
                quitButton.onClick.AddListener(Application.Quit);
        }

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))return;
            
            if (settingsCanvas.alpha != 0)
            {
                settingsCanvas.alpha = 1;
                settingsCanvas.interactable = false;
                settingsCanvas.blocksRaycasts = false;
                settingsRect.transform.localPosition = new Vector3(0, 0, 0);
                settingsRect.DOAnchorPos(new Vector2(0,-1000f), 0.2f, false).SetEase(Ease.OutExpo);
                settingsCanvas.DOFade(0, 0.2f);
            }
            if (creditsCanvas.alpha != 0)
            {
                creditsCanvas.alpha = 1;
                creditsCanvas.interactable = false;
                creditsCanvas.blocksRaycasts = false;
                creditsRect.transform.localPosition = new Vector3(0, 0, 0);
                creditsRect.DOAnchorPos(new Vector2(0,-1000f), 0.2f, false).SetEase(Ease.OutExpo);
                creditsCanvas.DOFade(0, 0.2f);
            }
            if (aboutUsCanvas.alpha != 0)
            {
                aboutUsCanvas.alpha = 1;
                aboutUsCanvas.interactable = false;
                aboutUsCanvas.blocksRaycasts = false;
                aboutUsRect.transform.localPosition = new Vector3(0, 0, 0);
                aboutUsRect.DOAnchorPos(new Vector2(0,-1000f), 0.2f, false).SetEase(Ease.OutExpo);
                aboutUsCanvas.DOFade(0, 0.2f);
            }
        }
    }
}
