using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ZombieAI : MonoBehaviour
{
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

    private LayerMask obstacles;
    private Animator anim;

    private enum State
    {
        Spawn,
        Chase
    }
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        target = FindClosestEnemy().transform;
        Debug.Log(target.gameObject.name);
        seeker = GetComponent<Seeker>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        colisionador = GetComponent<CircleCollider2D>();

        state = State.Spawn;

        obstacles = LayerMask.GetMask("Tilemap_objects");

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
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
        if (state == State.Chase) {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - body.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            body.AddForce(force * 1000);

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
            else
            {
                Vector2 enemyDirection = target.position - gameObject.transform.position;
                if (enemyDirection.x > 0.1f) { transform.localScale = new Vector3(1f, 1f, 1f); }
                else if (enemyDirection.x < 0.1f) { transform.localScale = new Vector3(-1f, 1f, 1f); }
            }
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
        if (other.CompareTag("Enemy") && !Physics2D.Raycast(gameObject.transform.position, enemyDirection, colisionador.radius, obstacles))
        {
            Attack(other);
        }
    }

    public void Attack(Collider2D other)
    {
        anim.SetTrigger("Attack");
        Vector2 direction = (target.GetComponent<Rigidbody2D>().position - body.position).normalized;
        body.AddForce(direction * 200000);
        other.gameObject.GetComponent<Enemy>().receiveDamage();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void EndSpawn()
    {
        state = State.Chase;
    }
}
