using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Load next scene
    /// </summary>
    public void NewGame()
    {
        Debug.Log("Loading new scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// quit the game
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
