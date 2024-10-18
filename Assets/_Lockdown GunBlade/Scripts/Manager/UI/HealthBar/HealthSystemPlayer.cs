using UnityEngine;

public class HealthSystemPlayer : HealthSystem
{
    public static bool isAlive;

    protected override void Start()
    {
        base.Start();
        isAlive = true;
    }

    protected override void SetColor(float healthPercentage)
    {
        if (healthPercentage >= 80)
        {
            healthDisplay.SetColor(Color.green);
        }
        else if (healthPercentage >= 50)
        {
            healthDisplay.SetColor(Color.yellow);
        }
        else
        {
            healthDisplay.SetColor(Color.red);
        }
    }

    protected override void HandleDeath()
    {
        isAlive = false;
        // Player-specific death handling logic
    }
}
