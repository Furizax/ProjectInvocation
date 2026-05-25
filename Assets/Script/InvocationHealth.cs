using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocationHealth : MonoBehaviour
{
    InvocationStats stats;
    public int currentHealth;
    bool isDead;
    bool canSummon;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<InvocationStats>();

        currentHealth = stats.maxHealth;
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
