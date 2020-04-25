using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{


    public void PlayLevel2()
    {
        SceneManager.LoadScene("DiseñoLVL2");
    }
    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void PlayLevel3()
    {
        SceneManager.LoadScene("DiseñoLVL3");
    }
    public void PlayLevel4()
    {
        SceneManager.LoadScene("DiseñoLVL4");
    }
    public void PlayLevel5()
    {
        SceneManager.LoadScene("DiseñoLVL5");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
