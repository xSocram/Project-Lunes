using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthController health;
    [SerializeField] private Image image;

    private void Start()
    {
        UpdateHealthBar(health.Health, health.MaxHealth);

        health.OnHealthChange += UpdateHealthBar;
    }

    private void UpdateHealthBar (float current, float max)
    {
        image.fillAmount = current / max;
    }

    private void OnDestroy()
    {
        health.OnHealthChange -= UpdateHealthBar;
    }
}
