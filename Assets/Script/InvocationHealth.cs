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


    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<InvocationStats>();
        currentHealth = stats.maxHealth;
    }

    public void SetManager(InvocationManager mgr)
    {
        manager = mgr;
    }

    public void SetHealtBar(Image bar)
    {
        healthFill = bar;
    }

    public float GetMaxHealth()
    {
        return stats.maxHealth;
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

    public void RegenHealth()
    {
      
    }

    void Die()
    {
        if (manager != null)
        {
            manager.OnInvocationDeath();
        }

        Destroy(gameObject);
    }


}

