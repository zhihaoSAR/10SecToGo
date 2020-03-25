using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    private Transform targetTransform;
    private Vector3 endPosition;
    public float directionChangeInterval;
    private Animator anim;

    private enum State
    {
        Patrol,
        Flee,
        Attack
    }
    private State state;

    private Rigidbody2D body;
    Coroutine moveCoroutine;
    Coroutine wanderCoroutine;
    public float speed = 1;

    void Start()
    {

        body = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        wanderCoroutine = StartCoroutine(WanderRoutine());

    }

    void Update()
    {
        timecounter += Time.deltaTime;
        /*if (body.velocity == Vector2.zero)
            anim.SetBool("Moving",false);*/
    }

    IEnumerator Flee()
    {
        //Debug.Log("State: Flee");
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        while (remainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(body.position, -targetTransform.position * 10000000, speed * Time.deltaTime);
            body.MovePosition(newPosition);
            anim.SetBool("Moving", true);
            yield return new WaitForFixedUpdate();
        }

    }

    /*private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyTransform = other.transform;
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("triggerEnter");
        if (other.CompareTag("Player"))
        {
            StopCoroutine(wanderCoroutine);
            targetTransform = other.transform;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Flee());
        }
        state = State.Flee;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("triggerExit");
        if (other.CompareTag("Player"))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            wanderCoroutine = StartCoroutine(WanderRoutine());
        }
        state = State.Patrol;
    }

    float timecounter;
    public float patroltime;
    IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move());
            Debug.Log("wandering");
            yield return new WaitForSeconds(directionChangeInterval);
        }
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

    IEnumerator Move()
    {
        while (timecounter < patroltime)
        {
            Debug.Log("Move");
            Vector3 newPosition = Vector3.MoveTowards(body.position, endPosition, speed * Time.deltaTime);
            body.MovePosition(newPosition);
            anim.SetBool("Moving", true);
            yield return new WaitForFixedUpdate();
        }
    }

}
