using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHealth : MonoBehaviour
{
    EnnemyStat ennemyStat;
    int currentHealth;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = ennemyStat.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(isDead)
        {
            Die();
            isDead = true; 
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
