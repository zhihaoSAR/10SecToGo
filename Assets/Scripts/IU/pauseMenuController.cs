using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuController : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUI,sceneController;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !sceneController.GetComponent<SceneController>().Paused)
        {
            if (IsPaused) resume();
            else pause();
        }
        
    }

    public void pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }
    public void resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
    public void exit()
    {
            SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        
    }
}
