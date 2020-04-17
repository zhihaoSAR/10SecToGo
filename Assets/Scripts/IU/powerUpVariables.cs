using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerUpVariables : MonoBehaviour
{
    public float tiempo=10, Maxtime=0;
    public Effect id;
    public RawImage timebar;
    void Update()
    {
        tiempo -= Time.deltaTime;
        SetTimeOutValue(tiempo / Maxtime);
        if (tiempo <= 0) Destroy(transform.gameObject);
    }
    public void setTime(float time)
    {
        tiempo = time;
        Maxtime = time;
    }
    public void SetTimeOutValue(float timeout)
    {
        timebar.transform.localScale = new Vector3(timeout, 1.0f);
    }


}
