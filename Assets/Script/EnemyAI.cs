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
    public GameObject pointA;
    public GameObject pointB;

    private Rigidbody2D rb;
    private Transform currentTarget;
    private Transform nextPoint;
    EnemyStat stats;
    private float distanceToTarget;
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
        nextPoint = pointB.transform;
        currentState = State.Idle;
    }

    private void Update()
    {
        SearchForTarget();
        if (currentTarget == null)
        {
            distanceToTarget = Mathf.Infinity;

        }
        else
        {
            distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);
        }

        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                if (currentTarget != null)
                    currentState = State.Chase;
                break;
            case State.Chase:
                HandleChase();
                if (distanceToTarget < stats.attackRange) currentState = State.Attack;
                else if (distanceToTarget > stats.detectionRange)
                {
                    currentTarget = null;
                    currentState = State.Idle;
                }
                break;
            case State.Attack:
                HandleAttack();
                Debug.Log(
                    "Distance: " + distanceToTarget +
                    " AttackRange: " + stats.attackRange);
                if (distanceToTarget > stats.attackRange) currentState = State.Chase;
                break;
        }
    }

    void HandleIdle()
    {
        if (nextPoint == pointB.transform)
        {
            rb.velocity = new Vector2(stats.moveSpeed, 0);
            Debug.Log(rb.velocity);
        }
        else
        {
            rb.velocity = new Vector2(-stats.moveSpeed, 0);
        }

        if (Vector2.Distance(transform.position, nextPoint.position) < 0.5f && nextPoint == pointB.transform)
        {
            nextPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, nextPoint.position) < 0.5f && nextPoint == pointA.transform)
        {
            nextPoint = pointB.transform;
        }
    }

    void HandleChase()
    {
        if (currentTarget == null)
            return;

        Vector3 direction = (currentTarget.position - transform.position).normalized;
        moveDirection = direction;
        rb.velocity = new Vector2(moveDirection.x, 0) * stats.chaseSpeed;
        Debug.Log(currentTarget.name);

    }

    void SearchForTarget()
    {
        if (currentTarget != null)
            return;

        List<Transform> targets = new List<Transform>();
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, stats.detectionRange);

        foreach (var hit in hitCollider)
        {
            if (hit.CompareTag("Player") || hit.CompareTag("Invocation"))
            {
                targets.Add(hit.transform);
                Debug.Log(hit.name);
                Debug.Log("Target Found");
            }
        }

        if (targets.Count == 0)
            return;

        currentTarget = targets[0];
    }

    void HandleAttack()
    {
        Debug.Log("AttackState");

        if (currentTarget == null)
            return;

        if (distanceToTarget > stats.attackRange)
            return;

        if (Time.time >= lastAttackTime + stats.attackCooldown)
        {
            Debug.Log(distanceToTarget);
            Debug.Log(stats.attackRange);
            IDamageable damageable = currentTarget.GetComponent<IDamageable>();
            if (damageable != null)
            {
                rb.velocity = Vector2.zero;
                damageable.TakeDamage(stats.damage);
                Debug.Log("Target Hit");
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

