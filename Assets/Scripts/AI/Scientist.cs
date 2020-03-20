using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    private Transform enemyTransform;
    private Vector3 endPosition;

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

    }

    void Update()
    {

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
        }
        state = State.Chase;
    }
}
