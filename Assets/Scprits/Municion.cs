using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Municion : MonoBehaviour
{
    Vector3 direction;
    public float speed =10;
    public const float vida = 3; //segundos
    float now = 0;
    Vector3 movement;
    [SerializeField]
    float offset = 0.5f;
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if(now > vida)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.Translate(movement*Time.deltaTime);
            now += Time.deltaTime;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
    public void initialize(Vector3 dir,Vector3 pos)
    {
        direction = dir;
        transform.position = pos + dir * offset;
        movement = dir * speed;
        now = 0;
    }
}
