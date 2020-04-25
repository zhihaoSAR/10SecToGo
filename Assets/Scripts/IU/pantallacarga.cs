using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class pantallacarga : MonoBehaviour
{
    // Start is called before the first frame update
    private int nivel=1;
    Animator a;
    public GameObject pantalla,menu;
    void Start()
    {
        

        nivel= PlayerPrefs.GetInt("nivel");

     
        a = pantalla.GetComponent<Animator>();
        
        
            a.SetTrigger("Previo"+nivel.ToString());
        
        
        StartCoroutine("Animacion");
    }

    
    IEnumerator Animacion()
    {
        yield return new WaitForSecondsRealtime(3f);
        
        a.SetTrigger(nivel.ToString());
        yield return new WaitForSecondsRealtime(3.5f);
        pantalla.SetActive(false);
        menu.SetActive(true);

    }

    public void Siguiente()
    {
        MenuManager menu = transform.GetComponent<MenuManager>();
        switch (nivel)
        {

            case 1: menu.PlayLevel2(); break;
            case 2: menu.PlayLevel3(); break;
            case 3: menu.PlayLevel4(); break;
            case 4: menu.PlayLevel5(); break;
            case 5: menu.GoMenu(); break;
        }
    }
}
