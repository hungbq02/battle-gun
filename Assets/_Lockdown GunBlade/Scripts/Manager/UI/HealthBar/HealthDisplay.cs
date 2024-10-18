using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image damagedHealthBar;
    [SerializeField] private Text countHealth;

    public void UpdateHealthBar(float healthPercentage, float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = healthPercentage;
        countHealth.text = $"{currentHealth} / {maxHealth}";
    }

    public void UpdateDamagedHealthBar(float healthPercentage, float speed)
    {
        damagedHealthBar.fillAmount = Mathf.MoveTowards(damagedHealthBar.fillAmount, healthPercentage, speed * Time.deltaTime);
    }

    public void SetColor(Color color)
    {
        healthBar.color = color;
    }
}
