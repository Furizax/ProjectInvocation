using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public GameObject pointA;
    public GameObject pointB;

    private Rigidbody2D rb;
    private Transform currentTarget;
    EnemyStat stats;
    private float distanceToPlayer;
    private Vector2 moveDirection;
    private float lastAttackTime;

    enum State
    {
        Idle,
        Chase,
        Attack
    }

    State currentState;

    private void Start()
    {
        stats = GetComponent<EnemyStat>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentTarget = pointB.transform;
        currentState = State.Idle;
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                if (distanceToPlayer < stats.detectionRange)
                    currentState = State.Chase;
                break;
            case State.Chase:
                HandleChase();
                if (distanceToPlayer < stats.attackRange) currentState = State.Attack;
                else if (distanceToPlayer > stats.detectionRange) currentState = State.Idle;
                break;
            case State.Attack:
                HandleAttack();
                if (distanceToPlayer > stats.attackRange) currentState = State.Chase;
                break;
        }
    }

    void HandleIdle()
    {
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
        Vector3 direction = (player.position - transform.position).normalized;
        moveDirection = direction;
        rb.velocity = new Vector2(moveDirection.x, 0) * stats.chaseSpeed;


    }

    void HandleAttack()
    {
        if (Time.time >= lastAttackTime + stats.attackCooldown)
        {

            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null)
            {
                rb.velocity = Vector2.zero;
                damageable.TakeDamage(stats.damage);
            }
            lastAttackTime = Time.time;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

}

