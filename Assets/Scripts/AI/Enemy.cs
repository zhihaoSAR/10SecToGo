﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public SceneController controller;
    [HideInInspector]
    public float playerDamage;
    public float maxHealth,immuneTime = 0.5f,effectTime = 0.1f;
    float health;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator anim;
    bool immune = false;

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        anim = GetComponent<Animator>();
    }
    void death()
    {
        controller.EnemyDead(maxHealth);
        anim.SetBool("Dead",true);
    }
    public void DestroyEnemy() { Destroy(gameObject); }
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
