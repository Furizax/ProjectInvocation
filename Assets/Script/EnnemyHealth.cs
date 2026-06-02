using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    EnemyStat enemyStat;
    public int currentHealth;
    bool isDead;

    [SerializeField] private GameObject enemySoul;

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
        if (enemySoul != null)
        {
          GameObject soul = Instantiate(enemySoul, transform.position, Quaternion.identity);
          SoulPickup soulScript = soul.GetComponent<SoulPickup>();
            soulScript.invocationPrefab = enemyStat.InvocationPrefab;
        }
        Destroy(gameObject);
    }
}
