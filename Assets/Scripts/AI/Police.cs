using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : MonoBehaviour
{
    public GameObject bullet;
    private Transform enemyTransform;
    private Vector3 endPosition;

    private float timeFromLastShot = 0;
    public float shootTime = 1;

    private enum State
    {
        Chase,
        Flee,
        Attack
    }
    private State state;

    private Rigidbody2D body;
    Coroutine moveCoroutine;
    public float Speed = 1;

    void Start()
    {

        body = gameObject.GetComponent<Rigidbody2D>();
        enemyTransform = GameObject.FindGameObjectWithTag("Player").transform;
        moveCoroutine = StartCoroutine(Chase());

    }

    private void Update()
    {
        timeFromLastShot = timeFromLastShot + Time.deltaTime;
    }

    IEnumerator Chase()
    {
        Debug.Log("State: Chase");
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        while (remainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(body.position, enemyTransform.position, Speed * Time.deltaTime);
            body.MovePosition(newPosition);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Flee()
    {
        Debug.Log("State: Flee");
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        while (remainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(body.position, -enemyTransform.position, Speed * Time.deltaTime);
            body.MovePosition(newPosition);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Shoot()
    {
        Debug.Log(timeFromLastShot);
        if (timeFromLastShot > shootTime)
        {
            Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
            timeFromLastShot = 0;
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
        Debug.Log("triggerEnter");
        if (other.CompareTag("Player"))
        {
            enemyTransform = other.transform;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            StartCoroutine(Shoot());
            moveCoroutine = StartCoroutine(Flee());

        }
        state = State.Flee;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("triggerExit");
        if (other.CompareTag("Player"))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Chase());
        }
        state = State.Chase;
    }
}
