using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    private Rigidbody2D body;
    public int speed = 100;
    private Transform target;
    Player player;
    private Animator anim;
    SceneController controller;
    float timeFromLastHit = 1;
    float timeDamage = 2;
    float rotationQuantity = 0;
    bool rotate = false;
    public float duration = 5;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<SceneController>();
        body = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = player.transform;
        Vector2 force = new Vector2(target.position.x - body.transform.position.x, target.position.y - body.transform.position.y);

        if (rotate)
        {
            float sinAngle = -Mathf.Sin(Mathf.Deg2Rad * rotationQuantity);
            float cosAngle = Mathf.Cos(Mathf.Deg2Rad * rotationQuantity);

            Vector2 force2 = new Vector2(force.x * cosAngle - force.y * sinAngle, force.y * cosAngle - force.x * sinAngle);
            body.AddForce(force2 * speed);
        }
        else
        {
            body.AddForce(force * speed);
        }
    }

    private void Update()
    {
        timeFromLastHit += Time.deltaTime;
        time += Time.deltaTime;
        if (time >= duration) { anim.SetTrigger("Hit"); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && timeFromLastHit >=1)
        {
            //player.receiveDamage();
            Debug.Log("bubbledamage");
            controller.ReduceTime(timeDamage);
        }

    }
    public void changeDirection(float quantity)
    {
        rotate = true;
        rotationQuantity = quantity;
    }

    void StopMovement()
    {
        gameObject.transform.localScale += new Vector3(4, 4, 0);
        body.velocity = new Vector2(0, 0);
    }

    void DestroyBullet() { Destroy(gameObject); }
}
