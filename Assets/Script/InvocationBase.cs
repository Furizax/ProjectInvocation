using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocationBase : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] float followDistance;
    [SerializeField] float followSpeed;
    [SerializeField] float maxDistanceFromPlayer;

    [SerializeField] int damage;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;

    private float lastAttackTime;
    private Transform currentTarget;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFollowingPlayer();
    }

    void HandleFollowingPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > followDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                followSpeed * Time.deltaTime);
        }

    }

    void HandleTarget()
    {

    }

    void HandleAttack()
    {

    }

    void CheckDistanceToPlayer()
    {
    }


}
