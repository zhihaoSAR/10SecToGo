using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Effect effect;
    public float life = 3f;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        effect = Modificador.RandomMod();
        animator.SetInteger("estado", (int)effect);
        Invoke("TimeUp", life);
        Invoke("ActivaColision", 1f);
        
    }
    void ActivaColision()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public Effect GetEffect()
    {
        return effect;
    }
    void TimeUp()
    {
        Destroy(gameObject);
    }
    


}
