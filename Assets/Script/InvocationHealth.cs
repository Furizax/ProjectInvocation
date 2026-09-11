using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvocationHealth : MonoBehaviour, IDamageable
{
    private InvocationStats stats;
    private InvocationManager manager;

    public int currentHealth;

    [SerializeField] private Image healthFill;

    void Awake()
    {
        stats = GetComponent<InvocationStats>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        if (healthFill != null)
        {
            healthFill.fillAmount = (float)currentHealth / stats.maxHealth;
        }
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.RoundToInt(health);

        if(healthFill != null)
        {
            healthFill.fillAmount = (float)currentHealth / stats.maxHealth;
        }
    }

    public int GetMaxHealth()
    {
        return stats.maxHealth;
    }

    public int GetCurrentHealth()
    { return currentHealth; }

    void Die()
    {
        if (manager != null)
        {
            manager.OnInvocationDeath();
        }

        Destroy(gameObject);
    }

    public void SetManager(InvocationManager mgr)
    {
        manager = mgr;
    }

    public void SetHealtBar(Image bar)
    {
        healthFill = bar;
    }

}

