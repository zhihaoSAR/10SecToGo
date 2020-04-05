using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosivo : MonoBehaviour
{
    Vector3 direction;
    const float vida = 3; //segundos
    const float duration = 2f;
    const float maxHeight = 1f;
    [SerializeField]
    Zone zone;
    [HideInInspector]
    public bool desactive;
    bool fin = false;
    AudioSource audio;

    [SerializeField]
    float offset = 0.1f;



    Vector3 startPosition, destination;
    float percentComplete = 0.0f;



    void Start()
    {
        gameObject.SetActive(false);
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(desactive)
        {
            gameObject.SetActive(false);
        }
    }
    void FixedUpdate()
    {

            
        if (percentComplete < 1.0f)
        {
            percentComplete += Time.deltaTime / duration;
            float currentHeight = Mathf.Sin(Mathf.PI * percentComplete) * maxHeight;
            transform.position = Vector3.Lerp(startPosition, destination, percentComplete) +
            Vector3.up * currentHeight;
        }
        else
        {
            if(!fin)
            {
                fin = true;
                Zone z = Instantiate<Zone>(zone);
                z.initialize(transform.position);
            }
            
        }
            
    }
    public void initialize(Vector3 dst, Vector3 pos)
    {
        startPosition = pos + (dst-pos).normalized*offset;
        destination = dst;
        percentComplete = 0;
        transform.position = startPosition;
        desactive = false;
        fin = false;
        audio.Play();
    }
}
