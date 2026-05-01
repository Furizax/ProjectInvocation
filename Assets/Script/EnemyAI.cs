using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    EnnemyStat stats;

    private float distanceToPlayer;

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, stats.moveSpeed * Time.deltaTime);
    }

    void UpdateState()
    {

    }

    void HandleState()
    {

    }

    void HandleIdle()
    {
        // rien ou patrouille simple
    }

    void HandleChase()
    {
        // se déplacer vers joueur
    }

    void HandleAttack()
    {
        // infliger dégâts (plus tard)

    }
}
