using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("quiting game...");
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
