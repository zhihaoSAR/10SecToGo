using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public SceneController controller;
    [HideInInspector]
    public float playerDamage;
    public float health,immuneTime = 0.5f,effectTime = 0.1f;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    bool immune = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    void death()
    {
        controller.EnemyDead();
        Destroy(gameObject);
    }
    public void receiveDamage()
    {
        if(!immune)
        {
            health -= playerDamage;
            if (health > 0)
            {
                immune = true;

                sprite.color = Color.red;
                StartCoroutine("resetDamegeEffect");
                StartCoroutine("resetImmune");
            }
            else
            {
                death();
            }
        }
        
    }
    IEnumerator resetImmune()
    {
        yield return new WaitForSeconds(immuneTime);
        immune = false;

    }

    IEnumerator resetDamegeEffect()
    {
        yield return new WaitForSeconds(effectTime);
        sprite.color = Color.white;
        
    }
}
