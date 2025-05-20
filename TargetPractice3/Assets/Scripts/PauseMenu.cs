using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private PlayerInputHandler _playerInputHandler;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        fpc.enabled = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        fpc.enabled = true;
    }

    public void LoadMenu()
    {
        //unfreeze time
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("quiting game...");
        Application.Quit();
    }
}
