using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class SplashScreen : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new WaitForSeconds(4f);
            SceneManager.LoadScene("MainMenu");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("MainMenu");
        }
    }
}
