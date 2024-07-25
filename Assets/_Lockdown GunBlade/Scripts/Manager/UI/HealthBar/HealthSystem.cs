using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    protected float currentHealth;

    [SerializeField] protected Image healthBar;
    [SerializeField] protected Image damagedHealthBar;
    [SerializeField] protected Text countHealth;
    [SerializeField] protected float damagedHealthBarSpeed = 2.0f;

    public Animator animator;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    protected virtual void Update()
    {
        UpdateDamagedHealthBar();
    }

    protected void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        countHealth.text = $"{currentHealth} / {maxHealth}";
        SetColor();
    }

    protected void UpdateDamagedHealthBar()
    {
        if (healthBar.fillAmount < damagedHealthBar.fillAmount)
        {
            damagedHealthBar.fillAmount -= damagedHealthBarSpeed * Time.deltaTime / maxHealth;
            if (damagedHealthBar.fillAmount < healthBar.fillAmount)
            {
                damagedHealthBar.fillAmount = healthBar.fillAmount;
            }
        }
        else if (healthBar.fillAmount > damagedHealthBar.fillAmount)
        {
            damagedHealthBar.fillAmount = healthBar.fillAmount;
        }
    }

    protected virtual void SetColor()
    {
        // Default implementation for setting color, can be overridden in subclasses
    }

    public void TakeDamage(int damage)
    {
        if (damage >= currentHealth)
        {
            currentHealth = 0;
            animator.SetTrigger("die");
            HandleDeath();
        }
        else
        {
            currentHealth -= damage;
            ActivatePowerUp();
            animator.SetTrigger("damage");
            //Check 
        }
        UpdateHealthBar();
    }

    public void Heal(int heal)
    {
        if (heal + currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += heal;
        }
        UpdateHealthBar();
    }

    protected abstract void HandleDeath();
    protected virtual void ActivatePowerUp(){}

}
