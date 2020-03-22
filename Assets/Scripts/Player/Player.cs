﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State { RUN, DASH, DEAD }
public class Player : MonoBehaviour
{
    

    private delegate void Attack(Vector3 mousePos);

    const float DASH_COOLDOWN = 0.5f; //segundos
    const float DASH_SPEED = 15;
    const float DASH_TIME = 0.1f; //segundos
    const int MUNICION_NUM = 5;
    const float MUNICION_COOLDOWN = 1f; //segundos
    const int EXPLOSIVO_NUM = 3;
    const float EXPLOSIVO_COOLDOWN = 3f; //segundos

    public State state = State.RUN;
    public float speed = 5;
    public float health = 1;

    public Municion municion;
    public Explosivo explosivo;

    Municion[] municiones;
    Explosivo[] explosivos;
    Attack attack;
    bool canAttack = true, immune = false;
    Rigidbody2D rb;
    Camera mainC;
    private Animator anim;



    void Start()
    {
        mainC = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    public void InitPlayer(Modificador mod)
    {
        switch(mod.Ataque)
        {
            case Ataque.DASH:
                attack = new Attack(Dash);
                break;
            case Ataque.DISTANTIA:
                municiones = new Municion[MUNICION_NUM];
                for (int i = 0; i< MUNICION_NUM;i++)
                {
                    municiones[i] = Instantiate<Municion>(municion);
                }
                attack = new Attack(AtkDistantia);
                break;
            case Ataque.EXPLOTION:
                explosivos = new Explosivo[EXPLOSIVO_NUM];
                for (int i = 0; i < EXPLOSIVO_NUM; i++)
                {
                    explosivos[i] = Instantiate<Explosivo>(explosivo);
                }
                attack = new Attack(Explosive);
                break;
        }
        
        
    }
    void Update()
    {
        if(canAttack && Input.GetButton("Fire"))
        {
            
            Vector3 mousePos = mainC.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 flip = (mousePos - transform.position);
            transform.localScale = new Vector3(Mathf.Sign(flip.x), 1f, 1f);
            anim.SetTrigger("attack");
            attack(mousePos);

        }

    }

    void FixedUpdate()
    {
        if(state.Equals(State.RUN))
        {
            float xDir = Input.GetAxis("Horizontal");
            float yDir = Input.GetAxis("Vertical");

            Vector3 dir = (new Vector3(xDir, yDir,0)).normalized;
            rb.MovePosition(transform.position + dir * speed*Time.deltaTime);

            //Animación x y
            anim.SetFloat("speed", Mathf.Abs(xDir)+Mathf.Abs(yDir));
            if(!Mathf.Approximately(xDir, 0f)){
                transform.localScale = new Vector3(Mathf.Sign(xDir), 1f, 1f);
                
            }
            
            //Fin animación x y
        }
    }
    void Dash(Vector3 mousePos)
    {
        state = State.DASH;
        canAttack = false;
        Vector3 dir = (mousePos - transform.position).normalized;
        

        StartCoroutine(Dashing(dir));

    }
    IEnumerator Dashing(Vector3 dir)
    {
        float time = 0;
        while(time < DASH_TIME)
        {
            rb.MovePosition(transform.position + dir * DASH_SPEED * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        state = State.RUN;
        yield return new WaitForSeconds(DASH_COOLDOWN);
        canAttack = true;
    }

    void AtkDistantia(Vector3 mousePos)
    {
        int i;
        Vector3 dir = (mousePos - transform.position).normalized;
        for(i = 0;i< MUNICION_NUM;i++)
        {
            if(!municiones[i].gameObject.activeSelf)
            {
                municiones[i].initialize(dir, transform.position);
                municiones[i].gameObject.SetActive(true);
                break;
            }
        }
        if(i < MUNICION_NUM)
        {
            canAttack = false;
            StartCoroutine(Cooldown(MUNICION_COOLDOWN));
        }
    }

    void Explosive(Vector3 mousePos)
    {
        int i;
        for (i = 0; i < EXPLOSIVO_NUM; i++)
        {
            if (!explosivos[i].gameObject.activeSelf)
            {
                explosivos[i].initialize(mousePos, transform.position);
                explosivos[i].gameObject.SetActive(true);
                break;
            }
        }
        if (i < EXPLOSIVO_NUM)
        {
            canAttack = false;
            StartCoroutine(Cooldown(EXPLOSIVO_COOLDOWN));
        }
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if(!immune && collision.collider.CompareTag("Bala"))
        {
            recibirDanyo();
        }*/
    }

    void recibirDanyo()
    {
        if(--health< 0)
        {
            anim.SetTrigger("death");
        }
    }
}
