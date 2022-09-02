using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1CanvasFader : MonoBehaviour
    {
        [SerializeField]
        KeyCode fadeToggleKey = KeyCode.U;
        [SerializeField]
        float tweenSpeed = 1f;
        [SerializeField]
        AllIn1DemoScaleTween hideUiButtonTween;

        bool isTweening = false;
        float currentAlpha = 1f;
        float targetAlpha = 1f;
        CanvasGroup canvasGroup;
        bool hideUiButtonTweenNotNull;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
            hideUiButtonTweenNotNull = hideUiButtonTween != null;
        }

        void Update()
        {
            if (Input.GetKeyDown(fadeToggleKey)) HideUiButtonPressed();

            if (!isTweening) return;
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.unscaledDeltaTime * tweenSpeed);
            canvasGroup.alpha = currentAlpha;
            if (targetAlpha == currentAlpha) isTweening = false;
        }

        public void HideUiButtonPressed()
        {
            if (currentAlpha < 0.01f) MakeCanvasVisibleTween();
            else MakeCanvasInvisibleTween();

            if (hideUiButtonTweenNotNull) hideUiButtonTween.ScaleUpTween();
        }

        void MakeCanvasVisibleTween()
        {
            isTweening = true;
            targetAlpha = 1f;
        }

        void MakeCanvasInvisibleTween()
        {
            isTweening = true;
            targetAlpha = 0f;
        }
    }
}
