using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    public int currentHealth;

    [SerializeField] private Image healthFill;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        float value = (float)currentHealth / maxHealth;
        if(healthFill != null)
        {
            healthFill.fillAmount = value;
        }
        
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
    }
}
