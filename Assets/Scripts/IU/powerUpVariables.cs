using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpVariables : MonoBehaviour
{
    public float tiempo=10, Maxtime=0;
    public Effect id;
    void Update()
    {
        tiempo -= Time.deltaTime;
        if (tiempo <= 0) Destroy(transform.gameObject);
    }
    public void setTime(float time)
    {
        tiempo = time;
        Maxtime = time;
    }
}
