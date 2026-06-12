using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthController health;

    [Header("Images")]
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailingFillImage;

    [Header("Tween Settings")]
    [SerializeField] private float fillDuration = 0.2f;
    [SerializeField] private float trailDuration = 0.4f;
    [SerializeField] private float trailDelay = 0.3f;

    private Tween fillTween;
    private Tween trailTween;

    private void Start()
    {
        float normalizedHealth = health.Health / health.MaxHealth;

        healthBarFillImage.fillAmount = normalizedHealth;
        healthBarTrailingFillImage.fillAmount = normalizedHealth;

        health.OnHealthChange += UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        float targetFill = current / max;

        fillTween?.Kill();
        trailTween?.Kill();

        fillTween = healthBarFillImage
            .DOFillAmount(targetFill, fillDuration)
            .SetEase(Ease.OutQuad);

        trailTween = healthBarTrailingFillImage
            .DOFillAmount(targetFill, trailDuration)
            .SetDelay(trailDelay)
            .SetEase(Ease.OutQuad);
    }

    private void OnDestroy()
    {
        health.OnHealthChange -= UpdateHealthBar;

        fillTween?.Kill();
        trailTween?.Kill();
    }
}