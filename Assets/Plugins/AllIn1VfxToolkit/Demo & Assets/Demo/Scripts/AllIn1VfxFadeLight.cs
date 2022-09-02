using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [RequireComponent(typeof(Light))]
    public class AllIn1VfxFadeLight : MonoBehaviour
    {
        [SerializeField]
        float fadeDuration = 0.1f;
        [SerializeField]
        bool destroyWhenFaded = true;
        Light targetLight;
        float animationRatioRemaining = 1f;
        float iniLightIntensity;

        void Start()
        {
            targetLight = GetComponent<Light>();
            iniLightIntensity = targetLight.intensity;
        }

        void Update()
        {
            targetLight.intensity = Mathf.Lerp(0f, iniLightIntensity, animationRatioRemaining);
            animationRatioRemaining -= Time.deltaTime / fadeDuration;
            if (destroyWhenFaded && animationRatioRemaining <= 0f) Destroy(gameObject);
        }
    }
}
