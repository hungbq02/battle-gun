using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    public bool isAlive;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image damagedHealthBar;
    [SerializeField] private Text countHealth;
    [SerializeField] private float damagedHealthBarSpeed = 2.0f;

    public Animator animator;
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        isAlive = true;
    }
    private void Update()
    {
        UpdateDamagedHealthBar();
    }
    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        countHealth.text = currentHealth + " / " + maxHealth;
        SetColor();
    }
    private void UpdateDamagedHealthBar()
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

    private void SetColor()
    {
        if ((currentHealth / maxHealth) * 100 >= 80)
        {
            healthBar.color = Color.green;
        }
        else if ((currentHealth / maxHealth) * 100 >= 50)
        {
            healthBar.color = Color.yellow;
        }
        else
        {
            healthBar.color = Color.red;
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage >= currentHealth)
        {
            currentHealth = 0;
            isAlive = false;
            animator.SetTrigger("die");
            //Removed collision component when enermy die
            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            CharacterController cc = GetComponent<CharacterController>();

            if (collider != null)
            {
                collider.enabled = false;
                Invoke("DestroyEnermy", 3f);
            }
            if (cc != null)
            {
                cc.enabled = false;
            }
        }
        else
        {
            currentHealth -= damage;
            animator.SetTrigger("damage");
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
            if (isAlive == false && currentHealth > 0)
            {
                isAlive = true;
            }
        }
        UpdateHealthBar();
    }
    void DestroyEnermy()
    {
        gameObject.SetActive(false);
    }    
}
