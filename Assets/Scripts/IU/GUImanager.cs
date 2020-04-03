using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUImanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cronometro,puntuacion,ronda, sceneM;
    private SceneController SceneManager;
    private TMPro.TextMeshProUGUI c, p, r;
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
        c.text = SceneManager.time.ToString("#");
        p.text = "hola";
    }

    public void UpdateRonda(int numRonda)
    {
        r.text = "Ronda " + numRonda;
    }
}
