using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuController : MonoBehaviour
{
    public static bool IsPaused = false;
    public string nivel;
    public GameObject PauseMenuUI,sceneController,opciones;
    private GameObject opcionesVolver;
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
        sceneController.GetComponent<SceneController>().pausePlayer();
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }
    public void resume()
    {
        sceneController.GetComponent<SceneController>().unPausePlayer();
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
    public void exit()
    {
            SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        
    }
    public void reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nivel);
    }
    public void GoOptions(GameObject este)
    {
        opcionesVolver = este;
        este.SetActive(false);
        opciones.SetActive(true);
    }
    public void VolverOptions()
    {
        opciones.SetActive(false);
        opcionesVolver.SetActive(true);
    }



}
