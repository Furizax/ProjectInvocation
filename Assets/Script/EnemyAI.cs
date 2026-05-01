using System.Collections;
using System.Collections.Generic;
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
    private bool isPlayerDetected = false;


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
    }

    private void Update()
    {
        Vector2 Point = currentTarget.position - transform.position;
        if(currentTarget == pointB.transform)
        {
            rb.velocity = new Vector2(stats.moveSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-stats.moveSpeed, 0);
        }

        if(Vector2.Distance(transform.position, currentTarget.position) < 0.5f && currentTarget == pointB.transform)
        {
            currentTarget = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f && currentTarget == pointA.transform)
        {
            currentTarget = pointB.transform;
        }

        /*
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, stats.moveSpeed * Time.deltaTime); 
        */


    }

    void UpdateState()
    {
        
    }

    void HandleState()
    {

    }

    void HandleIdle()
    {

    }

    void HandleChase()
    {

    }

    void HandleAttack()
    {
        // infliger dégâts (plus tard)

    }
}
