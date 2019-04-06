using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour {

    int WinSceneNumber;

    public void LoadNextScene()
    {
        WinSceneNumber = SceneManager.sceneCountInBuildSettings - 1;

        if (SceneManager.GetActiveScene().buildIndex < WinSceneNumber) // If not win scene
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0); // Start over
        }
    }
}
