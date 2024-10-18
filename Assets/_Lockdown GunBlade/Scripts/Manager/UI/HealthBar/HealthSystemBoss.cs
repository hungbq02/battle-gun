using UnityEngine;

public class HealthSystemBoss : HealthSystem
{
    private bool hasActivatedPowerUp = false;
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        ActivatePowerUp();
    }
    protected override void HandleDeath()
    {
        DisableComponents();
        Invoke(nameof(DestroyEnemy), 3f);
    }

    private void DestroyEnemy()
    {
        gameObject.SetActive(false);
        GUIManager.Instance.ShowWinDialog();
    }
    private void DisableComponents()
    {
        var collider = GetComponent<CapsuleCollider>();
        var cc = GetComponent<CharacterController>();
        if (collider != null) collider.enabled = false;
        if (cc != null) cc.enabled = false;
    }
    protected override void ActivatePowerUp()
    {
        float healthRatio = GetHealthRatio();
        if (hasActivatedPowerUp) return;

        if (healthRatio < 0.5f)
        {
            animator.SetBool("isImmune", true);
            hasActivatedPowerUp = true;

        }
    }
}
