using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUImanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cronometro, ronda, hearts, sceneM,PowerUpsZone,powerObject;
    
    IDictionary<Effect, GameObject> dic = new Dictionary<Effect, GameObject>();
    private SceneController SceneManager;
    private TMPro.TextMeshProUGUI c, r,v;
    [SerializeField] private Sprite[] images;
    string Nronda ="1",vida="1";
 
    void Start()
    {
        c = cronometro.GetComponent<TMPro.TextMeshProUGUI>();
        r = ronda.GetComponent<TMPro.TextMeshProUGUI>();
        v = hearts.GetComponent<TMPro.TextMeshProUGUI>();
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
        v.text = vida;
    }

    public void UpdateRonda(int numRonda)
    {
       
        Nronda = "Ronda " + numRonda.ToString();   
    }
    public void UpdateVida(float num)
    {
        vida = num.ToString();
    }
    public void createPowerUp(float time,Effect name)
    {

        GameObject salida;
        if(dic.TryGetValue(name, out salida)){
            salida.GetComponent<powerUpVariables>().setTime(time); Debug.Log("creado");
        }

        else { 
        
            salida = Instantiate(powerObject, PowerUpsZone.transform) as GameObject;
            powerUpVariables aux = salida.GetComponent<powerUpVariables>();
            aux.instanciar(name,time, images[(int)name]);
            aux.GetComponentInChildren<Image>().sprite= images[(int)name];
            dic.Add(name, salida);

        }
       
       
    }
    public void Medestruyo(Effect yo)
    {
        Debug.Log("ME destruyo"+yo);
        dic.Remove(yo);
    }
    public void destruir(Effect id)
    {
        GameObject salida;
        if (dic.TryGetValue(id, out salida))
        {
            Destroy(salida);
            dic.Remove(id);
        }
    }
}
