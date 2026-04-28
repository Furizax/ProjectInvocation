using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Transform player;

    EnnemyStat stats;
    float distanceToPlayer;

    enum State
    {
        Idle,
        Chase,
        Attack
    }

    State currentState;

    private void Start()
    {

    }

    private void Update()
    {

    }

    void UpdateState()
    {
       
    }

    void HandleState()
    {
        // switch(currentState)
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
