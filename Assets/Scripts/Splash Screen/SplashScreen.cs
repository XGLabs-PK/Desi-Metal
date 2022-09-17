using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class SplashScreen : MonoBehaviour
    {
        public float delay;
        public string sceneName;
        
        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("MainMenu");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(sceneName);
        }
    }
}
