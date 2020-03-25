using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{

    const float time = 3;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void initialize(Vector3 pos)
    {
        transform.position = pos;
        StartCoroutine(destroy());
    }
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.isTrigger &&other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().receiveDamage();
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Enemy>().receiveDamage();
        }

    }*/
}
