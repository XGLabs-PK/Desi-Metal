using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable once CheckNamespace
namespace XGStudios.MainMenu
{
    public class SceneTransition : MonoBehaviour
        {
            [SerializeField]
            GameObject transitionCanvas;
            [SerializeField]
            Material screenTransitionMat;

            [SerializeField]
            float transitionTime = 1f;

            [SerializeField]
            string propertyName = "_Progress";

            public UnityEvent onTransitionDone;
            
            void Start()
            {
                StartCoroutine(TransitionCoroutine());
            }

            IEnumerator TransitionCoroutine()
            {
                float currentTime = 0f;
                while (currentTime < transitionTime)
                {
                    currentTime += Time.deltaTime;
                    float progress = currentTime / transitionTime;
                    screenTransitionMat.SetFloat(propertyName, Mathf.Clamp01(progress));
                    yield return null;
                }
                onTransitionDone?.Invoke();
            }

            public void TransitionDone()
            {
                transitionCanvas.SetActive(false);
            }
        }
}
