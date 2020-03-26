using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CientificoAI : MonoBehaviour
{
    float timecounter = 0;
    public float patroltime;

    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 1f;
    public float stoppingDistance;

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
    private Vector3 endPosition;


    private enum State
    {
        Roam,
        Flee
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
        state = State.Roam;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        intState = 1;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            if (state == State.Roam)
            {
                seeker.StartPath(body.position, endPosition, OnPathComplete);
            }
            else if (state == State.Flee)
            {
                seeker.StartPath(body.position, target.position, OnPathComplete);
            }
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
        timecounter += Time.deltaTime;
        if (timecounter > patroltime)
            ChooseNewEndpoint();
    }


    void FixedUpdate()
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

        body.AddForce(force*1000);

        float distanceToWaypoint = Vector2.Distance(body.position, path.vectorPath[currentWaypoint]);

        if (distanceToWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (body.velocity.x >= .1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (body.velocity.x <= -.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(GFX.position, Vector3.back, stoppingDistance);
        //UnityEditor.Handles.DrawWireDisc(path.vectorPath[currentWaypoint], Vector3.back, 0.1f);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 enemyDirection = target.position - gameObject.transform.position;
        //Debug.DrawRay(gameObject.transform.position, enemyDirection);
        if (other.CompareTag("Player") && !Physics2D.Raycast(gameObject.transform.position, enemyDirection, colisionador.radius, obstacles))
        {
            intState = -1;
            RunAway();
            state = State.Flee;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("triggerExit");
        if (other.CompareTag("Player"))
        {
            intState = 1;
            ChooseNewEndpoint();
            state = State.Roam;
        }
    }

    void RunAway()
    {

    }

    float currentAngle = 0;
    void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle);
        timecounter = 0;
    }

    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float radians = inputAngleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
    }
}
