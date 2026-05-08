using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class InvocationBase : MonoBehaviour
{

    [SerializeField] float followDistance;
    [SerializeField] float followSpeed;
    [SerializeField] float maxDistanceFromPlayer;

    [SerializeField] int damage;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;

    [SerializeField] float detectionRange;

    private Transform player;
    private Transform currentTarget;

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
       
        HandleFollowingPlayer();
        HandleNearbyEnemies();
        checkDistanceFromPlayer();
    }

    void HandleFollowingPlayer()
    {
        //Si l'invocation a une cible elle ne suit pas le joueur 
        if (currentTarget != null)
            return;

        float distanceIP = Vector2.Distance(transform.position, player.position); //DistanceIP = Distance entre l'invocation et le joueur

        if (distanceIP > followDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                followSpeed * Time.deltaTime);
        }
    }

    void HandleNearbyEnemies()
    {
        // Si la cible existe encore, on garde cette cible
        if (currentTarget != null)
            return;

        List<Transform> enemies = new List<Transform>();
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, detectionRange);

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
