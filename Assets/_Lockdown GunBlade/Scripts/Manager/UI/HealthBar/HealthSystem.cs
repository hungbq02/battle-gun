using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthDisplay))]
public abstract class HealthSystem : MonoBehaviour, IDamageable
{
    public float maxHealth;
    protected float currentHealth;

    [SerializeField] protected HealthDisplay healthDisplay;
    [SerializeField] protected float damagedHealthBarSpeed = 2.0f;

    public Animator animator;
    [SerializeField] private float damageAnimationDelay = 2; // Delay time between
    private bool isDamageAnimating = false;

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
        float healthRatio = GetHealthRatio();
        healthDisplay.UpdateHealthBar(healthRatio, currentHealth, maxHealth);
        SetColor(healthRatio * 100);
    }

    protected void UpdateDamagedHealthBar()
    {
        float healthRatio = GetHealthRatio();
        healthDisplay.UpdateDamagedHealthBar(healthRatio, damagedHealthBarSpeed);
    }
    protected float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }
    public virtual void TakeDamage(int damage)
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
            if (!isDamageAnimating)
            {
                StartCoroutine(DelayAnimDamage());
            }
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
    //Coroutine Delay (ðang ????)
    private IEnumerator DelayAnimDamage()
    {
        isDamageAnimating = true;
        animator.SetTrigger("damage");

        // Wait for the duration of the animation delay
        yield return new WaitForSeconds(damageAnimationDelay);

        isDamageAnimating = false;
    }
    protected abstract void HandleDeath();
    protected virtual void SetColor(float healthPercentage) { }
    protected virtual void ActivatePowerUp() { }
}
