using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
    [SerializeField] private HealthController health;
    [SerializeField] private Image healthBar;

    private void Awake()
    {
        if (health == null)
            health = GetComponentInParent<HealthController>();
    }

    private void Start()
    {
        UpdateBar(health.Health, health.MaxHealth);
        health.OnHealthChange += UpdateBar;
    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void UpdateBar(float current, float max)
    {
        if (max <= 0) return;
        healthBar.fillAmount = current / max;
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnHealthChange -= UpdateBar;
    }
}