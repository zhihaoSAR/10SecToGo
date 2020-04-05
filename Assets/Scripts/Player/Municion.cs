using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Municion : MonoBehaviour
{
    Vector3 direction;
    public float speed =8;
    public const float vida = 3; //segundos
    float now = 0;
    Vector3 movement;
    [SerializeField]
    float offset = 0.1f;
    bool canMove = true;
    [HideInInspector]
    public bool desactive;
    Animator anim;
    AudioSource audio;

    [SerializeField]
    AudioClip proyectilAudio;
    [SerializeField]
    AudioClip contactAudio;


    void Start()
    {
        gameObject.SetActive(false);
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }
    void OnEnable()
    {
            audio.PlayOneShot(proyectilAudio);
        
    }
    void FixedUpdate()
    {
        if(now > vida || desactive)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if(canMove)
            {
                transform.Translate(movement * Time.deltaTime);
                now += Time.deltaTime;
            }
            
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(canMove)
        {
            canMove = false;
            anim.SetTrigger("impact");
            audio.PlayOneShot(contactAudio);
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<Enemy>().receiveDamage();
            }
        }
    }

    public void initialize(Vector3 dir,Vector3 pos)
    {
        direction = dir;
        transform.position = pos + dir * offset;
        movement = dir * speed;
        now = 0;
        canMove = true;
        desactive = false;
        
    }
}
