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

        UpdateHPBar();
    }

    public void SetManager(InvocationManager mgr)
    {
        manager = mgr;
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

        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        float value = currentHealth / (float)stats.maxHealth;
        if(healthFill != null)
        {
            healthFill.fillAmount = value;
        }
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

