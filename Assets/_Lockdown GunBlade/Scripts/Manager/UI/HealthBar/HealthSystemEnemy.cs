using UnityEngine;

public class HealthSystemEnemy : HealthSystem
{
    ItemDropScript itemDropScript;

    protected override void Start()
    {
        base.Start();
        itemDropScript = GetComponent<ItemDropScript>();

    }
    protected override void HandleDeath()
    {
        itemDropScript.DropItem(transform.position);

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
