using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State { RUN, DASH, DEAD }
public class Player : MonoBehaviour
{
    

    private delegate void Attack(Vector3 mousePos);

    const float DASH_COOLDOWN = 0.5f; //segundos
    const float DASH_SPEED = 32;
    const float DASH_TIME = 0.1f; //segundos

    public State state = State.RUN;
    public float speed = 8;

    Attack attack;
    bool canAttack = true;
    Rigidbody2D rb;
    Camera mainC;



    void Start()
    {
        attack = new Attack(Dash);
        rb = GetComponent<Rigidbody2D>();
        mainC = Camera.main;
    }

    void Update()
    {
        if(canAttack && Input.GetButton("Fire"))
        {
            
            Vector3 mousePos = mainC.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
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
            Debug.Log((dir * speed * Time.deltaTime).magnitude);
            rb.MovePosition(transform.position + dir * speed*Time.deltaTime);
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
            Debug.Log((dir * speed * Time.deltaTime).magnitude);
            rb.MovePosition(transform.position + dir * DASH_SPEED * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        state = State.RUN;
        yield return new WaitForSeconds(DASH_COOLDOWN);
        canAttack = true;
    }
}
