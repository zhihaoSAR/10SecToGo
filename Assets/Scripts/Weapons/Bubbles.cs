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

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = player.transform;
        Vector2 force = new Vector2(target.position.x - body.transform.position.x, target.position.y - body.transform.position.y);
        //Vector3 newPosition = Vector3.MoveTowards(body.position, target.position, speed * Time.deltaTime);
        body.AddForce(force * speed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            player.receiveDamage();
            body.velocity = new Vector2(0, 0);
        }

    }

    void DestroyBullet() { Destroy(gameObject); }
}
