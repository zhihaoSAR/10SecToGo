using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public SceneController controller;
    public float maxHealth,immuneTime = 0.5f,effectTime = 0.1f;
    [HideInInspector]
    public float health;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator anim;
    bool immune = false;
    public Zone toxic;

    [Tooltip("probabilidad de 0-1.")]
    public float probabilityPowerUp;
    public PowerUp powerUp;

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
        if(controller.explosivo)
        {
            toxic = Instantiate<Zone>(toxic);
            toxic.initialize(transform.position);
        }
        if(Random.Range(0f,1f) < probabilityPowerUp )
        {
            Instantiate<PowerUp>(powerUp, transform.position, Quaternion.identity);
        }
    }
    public void DestroyEnemy() { Destroy(gameObject); }
    public void receiveDamage()
    {
        if(!immune)
        {
            immune = true;
            health -= controller.damage;
            if (health > 0)
            {
                Debug.Log("hit");
                //sprite.color = Color.red;
                anim.SetInteger("Life",anim.GetInteger("Life")-1);
                StartCoroutine("resetDamegeEffect");
                StartCoroutine("resetImmune");
            }
            else
            {
                death();
            }
        }
        
    }
    public void ChangeFase()
    {
        anim.SetBool("FaseChanged", true);
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
