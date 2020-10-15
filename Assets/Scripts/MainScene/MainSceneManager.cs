using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public void Game()
    {
        SceneManager.LoadScene(2);
    }

    public void End()
    {
        Application.Quit();
    }
}
