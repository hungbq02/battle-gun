using UnityEngine;

public class HealthSystemEnemy : HealthSystem
{
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
    }
}
