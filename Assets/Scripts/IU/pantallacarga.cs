using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class pantallacarga : MonoBehaviour
{
    // Start is called before the first frame update
    private int nivel=1;
    Animator a;
    public GameObject pantalla;
    public List<Sprite> imagenes;
    void Start()
    {
        

        //nivel= PlayerPrefs.GetInt("nivel");

        StartCoroutine("Animacion");
        a = pantalla.GetComponent<Animator>();
        
        StartCoroutine("Animacion");
    }

    
    IEnumerator Animacion()
    {
        
        yield return new WaitForSecondsRealtime(1f);
        a.SetTrigger(nivel.ToString());
        yield return new WaitForSecondsRealtime(1.5f);
        //pantalla.SetActive(false);
        
    }
}
