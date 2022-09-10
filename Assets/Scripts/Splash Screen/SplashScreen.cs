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
            SceneManager.LoadScene("Main Menu");
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("Main Menu");
        }
    }
}
