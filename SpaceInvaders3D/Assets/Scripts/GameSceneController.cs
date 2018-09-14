using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    public void LoadSceneFromIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void QuitGame()
    {
        print("Quiting Game");
        Application.Quit();
    }
}
