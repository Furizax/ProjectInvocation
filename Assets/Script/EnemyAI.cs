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
    private Collider2D enemyCollider;
    private Collider2D targetCollider;

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
        enemyCollider = GetComponent<Collider2D>();

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
            targetCollider = currentTarget.GetComponent<Collider2D>();

            if (targetCollider != null)
            {
                ColliderDistance2D distance = enemyCollider.Distance(targetCollider);
                distanceToTarget = distance.distance;
            }
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
                if (enemyCollider.IsTouching(targetCollider)) currentState = State.Attack;
                else if (distanceToTarget > stats.detectionRange)
                {
                    currentTarget = null;
                    ReturnToClosestPoint();
                    currentState = State.Idle;
                }
                break;
            case State.Attack:
                HandleAttack();
                if (distanceToTarget > stats.attackRange) currentState = State.Idle;
                break;
        }
    }

    void HandleIdle()
    {
        Vector2 direction = (nextPoint.position - transform.position).normalized;

        rb.velocity = new Vector2(direction.x, 0) * stats.moveSpeed;

        if (Vector2.Distance(transform.position, nextPoint.position) < 0.5f)
        {
            if (nextPoint == pointB.transform)
                nextPoint = pointA.transform;
            else
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
       // Debug.Log("Distance : " + distanceToTarget);
       // Debug.Log("Attack range : " + stats.attackRange);

    }

    void HandleAttack()
    {
        if (currentTarget == null)
            return;
        
        if (targetCollider == null)
            return;

        if (!enemyCollider.IsTouching(targetCollider))
            return;

        if (Time.time >= lastAttackTime + stats.attackCooldown)
        {
            Debug.Log(distanceToTarget);
            Debug.Log(stats.attackRange);

            IDamageable damageable = currentTarget.GetComponent<IDamageable>();

            Debug.Log("Target : " + currentTarget.name);
           // Debug.Log("Damageable : " + damageable);
            if (damageable != null)
            {
                rb.velocity = Vector2.zero;
                damageable.TakeDamage(stats.damage);
                Debug.Log("Target Hit");
            }
            lastAttackTime = Time.time;
        }
    }

    void SearchForTarget()
    {
        if (currentTarget != null)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stats.detectionRange);

        Transform playerTarget = null;
        List<Transform> invocations = new List<Transform>();

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerTarget = hit.transform;
            }

            if (hit.CompareTag("Invocation"))
            {
                invocations.Add(hit.transform);
            }
        }

        if (hasBeenHitByInvocation && invocations.Count > 0)
        {
            currentTarget = invocations[0];
            return;
        }

        if (playerTarget != null)
        {
            currentTarget = playerTarget;
            return;
        }

        if (invocations.Count > 0)
        {
            currentTarget = invocations[0];
        }

    }

    void ReturnToClosestPoint()
    {
        float distanceToA = Vector2.Distance(transform.position, pointA.transform.position);
        float distanceToB = Vector2.Distance(transform.position, pointB.transform.position);

        if (distanceToA < distanceToB)
        {
            nextPoint = pointA.transform;
        }
        else
        {
            nextPoint = pointB.transform;
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

    private void OnDrawGizmos()
    {


    }
}

