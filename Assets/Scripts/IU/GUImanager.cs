using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUImanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cronometro,ronda, sceneM,PowerUpsZone,powerObject;
    private List<powers> powersUps = new List<powers>();
    private SceneController SceneManager;
    private TMPro.TextMeshProUGUI c, r,v;
    string Nronda="",vida="1";
    struct powers
    {
        public int time,totalTime;
        public Effect id;
        public GameObject PU;
    } 
    void Start()
    {
        c = cronometro.GetComponent<TMPro.TextMeshProUGUI>();
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
    public void UpdateVida(int num)
    {
        vida = num.ToString();
    }
    public void createPowerUp(int time,Effect name)
    {
        bool esta = false;
   
        powersUps.ForEach(x=> {
            if (x.id.Equals(name)){ esta = true;x.time = time; } //actualizar tiempo
                
                });
        if (!esta)//crear objeto
        {
            powers p; p.time = time; p.totalTime = time; p.id = name; p.PU = Instantiate(powerObject,PowerUpsZone.transform)as GameObject;
           // p.PU.transform.SetParent(PowerUpsZone.transform);
           // p.PU.transform.localScale.Set(1f, 1f, 1f);

            powersUps.Add(p);
        }
        
    }

}
