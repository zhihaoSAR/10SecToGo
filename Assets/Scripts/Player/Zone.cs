using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{

    const float time = 3;
    float offset = 0.3f;


    public void initialize(Vector3 pos)
    {
        pos.y -= offset;
        transform.position = pos;
        StartCoroutine(destroy());
    }
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        
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
