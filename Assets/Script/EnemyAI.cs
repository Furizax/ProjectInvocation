using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public GameObject pointA;
    public GameObject pointB;

    private Rigidbody2D rb;
    private Transform currentTarget;
    EnnemyStat stats;
    private float distanceToPlayer;
    public bool isChasing;

    enum State
    {
        Idle,
        Chase,
        Attack
    }

    State currentState;

    private void Start()
    {
        stats = GetComponent<EnnemyStat>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        currentTarget = pointB.transform;
        currentState = State.Idle;
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        UpdateState();
        HandleState();
    }

    void UpdateState()
    {
        if(distanceToPlayer > stats.detectionRange)
        {
            currentState = State.Idle;
        }
        if(distanceToPlayer < stats.detectionRange)
        {
            currentState = State.Chase;
        }
    }

    void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                Debug.Log("Idle");
                break;
            case State.Chase:
                HandleChase();
                Debug.Log("Chase");
                break;

        }
    }

    void HandleIdle()
    {
        Vector2 Point = currentTarget.position - transform.position;
        if (currentTarget == pointB.transform)
        {
            rb.velocity = new Vector2(stats.moveSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-stats.moveSpeed, 0);
        }

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f && currentTarget == pointB.transform)
        {
            currentTarget = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f && currentTarget == pointA.transform)
        {
            currentTarget = pointB.transform;
        }
    }

    void HandleChase()
    {
        if (isChasing)
        {
            if (transform.position.x > player.transform.position.x)
            {
                transform.position += Vector3.left * stats.moveSpeed * Time.deltaTime; //Si le joueur est chassé, en étant à gauche de l'ennemi
            }
            if (transform.position.x < player.transform.position.x)
            {
                transform.position += Vector3.right * stats.moveSpeed * Time.deltaTime; //Si le joueur est chassé, en étant à droite de l'ennemi
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) < stats.chaseDistance)
            {
                isChasing = true;
            }
        }
    }

    void HandleAttack()
    {
        // infliger dégâts (plus tard)

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
