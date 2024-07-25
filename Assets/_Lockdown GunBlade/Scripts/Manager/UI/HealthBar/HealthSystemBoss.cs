using UnityEngine;

public class HealthSystemBoss : HealthSystem
{
    private bool hasPoweredUp = false;
    protected override void SetColor()
    {
        // Enemies might not need to change color
    }

    protected override void HandleDeath()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        CharacterController cc = GetComponent<CharacterController>();

        if (collider != null)
        {
            collider.enabled = false;
            Invoke(nameof(DestroyEnemy), 3f);
        }
        if (cc != null)
        {
            cc.enabled = false;
        }
    }

    private void DestroyEnemy()
    {
        gameObject.SetActive(false);
    }

    protected override void ActivatePowerUp()
    {
        float healthPercentage = currentHealth / maxHealth;
       if (hasPoweredUp) return;
        Debug.Log("healthPercentage " + healthPercentage);

        if (healthPercentage < 0.5f)
        {
            animator.SetBool("isImmune", true);
            Invoke(nameof(PowerUpCompleted), 5f);
        }
    }
    void PowerUpCompleted()
    {
        hasPoweredUp = true;
    }
}
