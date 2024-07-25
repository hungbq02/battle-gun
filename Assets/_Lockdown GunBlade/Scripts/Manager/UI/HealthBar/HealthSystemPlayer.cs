using UnityEngine;

public class HealthSystemPlayer : HealthSystem
{
    public static bool isAlive;

    protected override void Start()
    {
        base.Start();
        isAlive = true;
    }

    protected override void SetColor()
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

    protected override void HandleDeath()
    {
        isAlive = false;
        // Player-specific death handling logic
    }
}
