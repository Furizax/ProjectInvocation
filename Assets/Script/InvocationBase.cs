using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class InvocationBase : MonoBehaviour
{

    InvocationStats stats;

    private Transform player;
    private Transform currentTarget;

    private float lastAttackTime;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<InvocationStats>();
        player = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        HandleNearbyEnemies();
        MoveToEnemy();
        HandleAttack();
        checkDistanceFromPlayer();
        HandleFollowingPlayer();
    }

    void HandleFollowingPlayer()
    {
        //Si l'invocation a une cible elle ne suit pas le joueur 
        if (currentTarget != null)
            return;

        float distanceIP = Vector2.Distance(transform.position, player.position); //DistanceIP = Distance entre l'invocation et le joueur

        if (distanceIP > stats.followDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                stats.followSpeed * Time.deltaTime);
        }
    }

    void HandleNearbyEnemies()
    {
        // Si la cible existe encore, on garde cette cible
        if (currentTarget != null)
            return;

        List<Transform> enemies = new List<Transform>();
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, stats.detectionRange);

        foreach (var hit in hitCollider)
        {
            if (hit.CompareTag("Enemy"))
            {
                enemies.Add(hit.transform);
                Debug.Log("Enemy found");
            }
        }

        //Aucun ennemis trouvé
        if (enemies.Count == 0)
            return;

        //Choisir un ennemi aléatoire 
        int randomIndex = Random.Range(0, enemies.Count);

        currentTarget = enemies[randomIndex];
    }

    void HandleAttack()
    {
      
        if (currentTarget == null)
            return;

        float distanceToEnemy = distanceToEnemy = Vector2.Distance(transform.position, currentTarget.position);

        if (distanceToEnemy > stats.attackRange)
            return; 

        if (currentTarget != null)
          
        if (Time.time < lastAttackTime + stats.attackCooldown)
            return;

        IDamageable damageable = currentTarget.GetComponent<IDamageable>();


        if (damageable != null)
        {
            damageable.TakeDamage(stats.damage);
            Debug.Log("Enemy hitted");
        }

        lastAttackTime = Time.time;
    }

    void MoveToEnemy()
    {
        if (currentTarget == null)
            return;

        float distanceToEnemy = Vector2.Distance(transform.position, currentTarget.position);

        if (currentTarget != null && distanceToEnemy > stats.attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, stats.offenseSpeed * Time.deltaTime) ;
            Debug.Log(distanceToEnemy);
        }
    }

    void checkDistanceFromPlayer()
    {
        float DistanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (DistanceToPlayer > stats.maxDistanceFromPlayer)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.detectionRange);
    }
}
