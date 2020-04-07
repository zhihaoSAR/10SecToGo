using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMenu : MonoBehaviour
{
    public GameObject scene,deadMenu;
    private SceneController scenecontroller;
    void Start()
    {
        scenecontroller = scene.GetComponent<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void reempezar() {
        scenecontroller.Paused = false;
        Time.timeScale = 1f;
        deadMenu.SetActive(false);
        scenecontroller.NextRound();
    }
    
}
