using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PoliceAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 1f;
    public float stoppingDistance;
    public GameObject bullet;
    public float shootBackForce = 50f;
    private bool dead = false;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    private CircleCollider2D colisionador;

    Seeker seeker;
    Rigidbody2D body;

    public Transform GFX;

    int intState = 1; //1 si perseguir -1 si huir

    private LayerMask obstacles;
    private Animator anim;
    private float timeFromLastShot = 0;
    public float shootTime = 1;


    private enum State
    {
        Chase,
        Flee,
        Attack
    }
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        colisionador = GetComponent<CircleCollider2D>();

        obstacles = LayerMask.GetMask("Tilemap_objects");

        state = State.Chase;

        InvokeRepeating("UpdatePath",0f, 0.5f);
        intState = 1;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        seeker.StartPath(body.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }

    }

    // Update is called once per frame
    private void Update()
    {
        timeFromLastShot = timeFromLastShot + Time.deltaTime;

    }


    void FixedUpdate()
    {
        if (!dead)
        {
            if (path == null)
                return;
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - body.position).normalized * intState;
            Vector2 force = direction * speed * Time.deltaTime;

            body.AddForce(force * 1000);

            float distanceToWaypoint = Vector2.Distance(body.position, path.vectorPath[currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            if (state != State.Flee)
            {
                if (body.velocity.x >= .1f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (body.velocity.x <= -.1f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else
            {
                Vector2 enemyDirection = target.position - gameObject.transform.position;
                if (enemyDirection.x > 0.1f) { transform.localScale = new Vector3(1f, 1f, 1f); }
                else if (enemyDirection.x < 0.1f) { transform.localScale = new Vector3(-1f, 1f, 1f); }
            }
        }
    }
    /*
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc( GFX.position, Vector3.back, stoppingDistance);
        //UnityEditor.Handles.DrawWireDisc(path.vectorPath[currentWaypoint], Vector3.back, 0.1f);
    }*/

    IEnumerator Shoot(Collider2D other)
    {
        if (timeFromLastShot > shootTime)
        {
            anim.SetTrigger("Shoot");
            Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
            if (gameObject.name.StartsWith("Hazmat"))
            {
                GameObject bubble2 = Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
                bubble2.GetComponent<Bubbles>().changeDirection(25);
                GameObject bubble3 = Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
                bubble3.GetComponent<Bubbles>().changeDirection(50);
                GameObject bubble4 = Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
                bubble4.GetComponent<Bubbles>().changeDirection(-25);
                GameObject bubble5 = Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
                bubble5.GetComponent<Bubbles>().changeDirection(-50);
            }
            timeFromLastShot = 0;
            Vector2 enemyDirection = target.position - gameObject.transform.position;
            body.AddForce(-enemyDirection*shootBackForce);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 enemyDirection = target.position - gameObject.transform.position;
        //Debug.DrawRay(gameObject.transform.position, enemyDirection);
        if (other.CompareTag("Player") && !Physics2D.Raycast(gameObject.transform.position ,enemyDirection,colisionador.radius , obstacles) && !dead)
        {
            intState = -1;
            StartCoroutine(Shoot(other));
            state = State.Flee;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Vector2 enemyDirection = target.position - gameObject.transform.position;
        if (other.CompareTag("Player") && !Physics2D.Raycast(gameObject.transform.position, enemyDirection, colisionador.radius, obstacles) && !dead)
        {
            if (timeFromLastShot > shootTime)
                StartCoroutine(Shoot(other));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("triggerExit");
        if (other.CompareTag("Player"))
        {
            intState = 1;
            state = State.Chase;
        }

    }

    public void die()
    {
        dead = true;
    }


}
