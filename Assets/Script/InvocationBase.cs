using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
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

    [SerializeField] float detectionRange;

    private float lastAttackTime;
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
        List<Transform> currentEnemies = HandleNearbyEnemies();

        HandleFollowingPlayer();
        checkDistanceFromPlayer();
    }

    void HandleFollowingPlayer()
    {
        float distanceIP = Vector2.Distance(transform.position, player.position); //DistanceIP = Distance entre l'invocation et le joueur

        if (distanceIP > followDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                followSpeed * Time.deltaTime);
        }
    }

    List<Transform> HandleNearbyEnemies()
    {
        List<Transform> enemies = new List<Transform>();
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        foreach (var hits in hitCollider)
        {
            if (hits.CompareTag("Enemy"))
            {
                enemies.Add(hits.transform);
                Debug.Log("Enemy found");
            }
        }

        return enemies;
    }

    void HandleAttack()
    {

    }
    void checkDistanceFromPlayer()
    {
        float DistanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (DistanceToPlayer > maxDistanceFromPlayer)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
