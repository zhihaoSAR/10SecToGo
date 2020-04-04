using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenuController : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
}
