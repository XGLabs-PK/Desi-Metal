using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace XGStudios.SplashScreen
{
    public class SplashScreen : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("Main Menu");
        }
    }
}
