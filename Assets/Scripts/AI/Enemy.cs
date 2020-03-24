using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public SceneController controller;
    void death()
    {
        controller.EnemyDead();
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            death();
        }
    }
}
