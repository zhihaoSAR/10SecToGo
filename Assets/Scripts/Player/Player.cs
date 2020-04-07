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
    const float IMMUNE_TIME = 1f;
    const float EFFECT_TIME = 0.1f;

    public float speed = 5;
    public float health = 1;
    public float damage = 1;

    [SerializeField]
    AudioClip dashAudio;
    [SerializeField]
    AudioClip walkAudio;
    [SerializeField]
    AudioClip screamAudio;

    [HideInInspector]
    public State state = State.RUN;
    [HideInInspector]
    public bool attacking = false;
    

    public Municion municion;
    public Explosivo explosivo;
    public GameObject SceneController;


    Municion[] municiones;
    Explosivo[] explosivos;
    Attack attack;
    bool canAttack = true, immune = false;
    bool infect;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Camera mainC;
    private Animator anim;
    AudioSource audio;



    void Start()
    {
        mainC = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        audio.clip = walkAudio;
        audio.Play();
    }
    public void InitPlayer(Modificador mod)
    {
        health = 1;
        damage = 1;
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
        infect = false;
        for (int i = 0; i < 2; i++)
        {
            switch (mod.pasivos[i])
            {
                case Pasivo.MAS_DANYO:
                    damage+=0.5f;
                    continue;
                case Pasivo.MAS_VIDA:
                    health++;
                    continue;
            }
        }


    }
    void Update()
    {
        if(state != State.DEAD && canAttack && Input.GetButton("Fire"))
        {
            
            Vector3 mousePos = mainC.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 flip = (mousePos - transform.position);
            transform.localScale = new Vector3(Mathf.Sign(flip.x), 1f, 1f);
            anim.SetTrigger("attack");
            attack(mousePos);

        }
        if(Input.GetKey(KeyCode.C))
        {
            health = 999;
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
            bool walking = Mathf.Approximately( Mathf.Abs(xDir) + Mathf.Abs(yDir) ,0) ? false : true;
            
            if(walking)
            {
                anim.SetFloat("speed",1 );
                audio.UnPause();
            }
            else
            {
                anim.SetFloat("speed",0 );
                audio.Pause();
            }
            

            if (!Mathf.Approximately(xDir, 0f)){
                transform.localScale = new Vector3(Mathf.Sign(xDir), 1f, 1f);
                
            }
            
            //Fin animación x y
        }
    }
    void Dash(Vector3 mousePos)
    {
        immune = true;
        state = State.DASH;
        canAttack = false;
        attacking = true;
        Vector3 dir = (mousePos - transform.position).normalized;
        audio.PlayOneShot(dashAudio);

        StartCoroutine(Dashing(dir));

    }
    IEnumerator Dashing(Vector3 dir)
    {
        float time = 0;
        while(time < DASH_TIME)
        {
            rb.MovePosition(transform.position+ dir * DASH_SPEED * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        attacking = false;
        immune = false;
        state = State.RUN;
        StartCoroutine(Cooldown(DASH_COOLDOWN));
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

    void OnCollisionStay2D(Collision2D collision)
    {
        if (attacking)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<Enemy>().receiveDamage();
            }
        }
    }


    public void receiveDamage()
    {
        if(!immune)
        {
            if (--health > 0)
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
    public void death()
    {
        if(state != State.DEAD)
        {
            state = State.DEAD;
            immune = true;
            canAttack = false;
            audio.Stop();
            audio.PlayOneShot(screamAudio) ;
            anim.SetTrigger("death");
            StartCoroutine(SceneController.GetComponent<SceneController>().deadMenu());
        }
        
    }

    public bool getImmune() { return immune; }

    IEnumerator resetImmune()
    {
        yield return new WaitForSeconds(IMMUNE_TIME);
        immune = false;
    }

    IEnumerator resetDamegeEffect()
    {
        yield return new WaitForSeconds(EFFECT_TIME);
        sprite.color = Color.white;
    }

    public void pause()
    {

    }
}
