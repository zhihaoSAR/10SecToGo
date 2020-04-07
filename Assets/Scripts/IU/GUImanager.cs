using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUImanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cronometro,puntuacion,ronda, sceneM;
    private SceneController SceneManager;
    private TMPro.TextMeshProUGUI c, p, r;
    string Nronda="";
    void Start()
    {
        c = cronometro.GetComponent<TMPro.TextMeshProUGUI>();
        p = puntuacion.GetComponent<TMPro.TextMeshProUGUI>();
        r = ronda.GetComponent<TMPro.TextMeshProUGUI>();
        SceneManager = sceneM.GetComponent<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.time > 0)
            c.text = SceneManager.time.ToString("0.0");
        else c.text = 0.ToString();
        r.text = Nronda;
    }

    public void UpdateRonda(int numRonda)
    {
        
       
        Nronda= "Ronda " + numRonda.ToString();

        
    }
}
