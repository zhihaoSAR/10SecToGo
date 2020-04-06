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
        if (SceneManager.time > 0)
            c.text = SceneManager.time.ToString("0.0");
        else c.text = 0.ToString();
    }

    public void UpdateRonda(int numRonda)
    {
        r.text = string.Concat("Ronda " , numRonda.ToString());
    }
}
