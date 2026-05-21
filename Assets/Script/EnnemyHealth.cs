using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    EnemyStat enemyStat;
    int currentHealth;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        enemyStat = GetComponent<EnemyStat>();

        currentHealth = enemyStat.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }


    }

    void Die()
    {
        Destroy(gameObject);
    }
}
