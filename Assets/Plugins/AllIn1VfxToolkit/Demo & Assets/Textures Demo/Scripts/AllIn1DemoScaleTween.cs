using System;
using UnityEngine;

namespace AllIn1VfxToolkit.DemoAssets.TexturesDemo.Scripts
{
    public class AllIn1DemoScaleTween : MonoBehaviour
    {
        [SerializeField]
        float maxTweenScale = 2.0f;
        [SerializeField]
        float minTweenScale = 0.8f;
        [SerializeField]
        float tweenSpeed = 15f;

        bool isTweening = false;
        float currentScale = 1f, iniScale;
        Vector3 scaleToApply = Vector3.one;

        void Start()
        {
            iniScale = transform.localScale.x;
        }

        void Update()
        {
            if (!isTweening) return;
            currentScale = Mathf.Lerp(currentScale, iniScale, Time.unscaledDeltaTime * tweenSpeed);
            UpdateScaleToApply();
            ApplyScale();
            if (Mathf.Abs(currentScale - 1f) < 0.02f) isTweening = false;
        }

        void UpdateScaleToApply()
        {
            scaleToApply.x = currentScale;
            scaleToApply.y = currentScale;
        }

        void ApplyScale()
        {
            transform.localScale = scaleToApply;
        }

        public void ScaleUpTween()
        {
            isTweening = true;
            currentScale = iniScale * maxTweenScale;
            UpdateScaleToApply();
        }

        public void ScaleDownTween()
        {
            isTweening = true;
            currentScale = iniScale * minTweenScale;
            UpdateScaleToApply();
        }
    }
}
