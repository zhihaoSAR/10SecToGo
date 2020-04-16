using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUImanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cronometro,ronda, sceneM,PowerUpsZone,powerObject;
    private List<GameObject> powersUps = new List<GameObject>();
    private SceneController SceneManager;
    private TMPro.TextMeshProUGUI c, r,v;
    string Nronda="",vida="1";
 
    void Start()
    {
        c = cronometro.GetComponent<TMPro.TextMeshProUGUI>();
        r = ronda.GetComponent<TMPro.TextMeshProUGUI>();
        SceneManager = sceneM.GetComponent<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        //revisarTiempos();
        if (SceneManager.time > 0)
            c.text = SceneManager.time.ToString("0.0");
        else c.text = 0.ToString();
        r.text = Nronda;
    }

    public void UpdateRonda(int numRonda)
    {
       
        Nronda = "Ronda " + numRonda.ToString();   
    }
    public void UpdateVida(int num)
    {
        vida = num.ToString();
    }
    public void createPowerUp(float time,Effect name)
    {
        bool esta = false;
   
        powersUps.ForEach(x=> {
            powerUpVariables aux = x.GetComponent<powerUpVariables>();
            if (aux.id.Equals(name)) { esta = true;aux.setTime(time); Debug.Log("creado"); } //actualizar tiempo
                
                });
        if (!esta)//crear objeto
        {
            GameObject este = Instantiate(powerObject, PowerUpsZone.transform) as GameObject;
            powerUpVariables aux = este.GetComponent<powerUpVariables>();
            aux.setTime(time);aux.id = name;
            powersUps.Add( este);
        }
       
       
    }
}
