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
    private bool hasBeenHitByInvocation;

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

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stats.detectionRange);

        Transform playerTarget = null; 
        List<Transform> invocations = new List<Transform>();

        foreach(var hit  in hits)
        {
            if(hit.CompareTag("Player"))
            {
                playerTarget = hit.transform;
            }

            if(hit.CompareTag("Invocation"))
            {
                invocations.Add(hit.transform);
            }
        }

        if(hasBeenHitByInvocation && invocations.Count > 0)
        {
            currentTarget = invocations[0];
            return;
        }

        if(playerTarget != null)
        {
            currentTarget = playerTarget;
            return;
        }    

        if(invocations.Count > 0)
        {
            currentTarget = invocations[0];
        }
        
    }

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void OnHit(Transform attacker)
    {
        currentTarget = attacker;
        currentState = State.Chase;
    }

    public void onHitByInvocation()
    {
        hasBeenHitByInvocation = true;
    }

    void HandleAttack()
    {
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

