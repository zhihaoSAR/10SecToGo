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
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }


}
