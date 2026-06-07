using System;
using UnityEngine;

public class LowHealthGrayScale : MonoBehaviour
{
    [SerializeField] private Material grayScaleMaterial;
    [SerializeField] private HealthController playerHealth;

    [SerializeField] private float threshold = 1f;

    private void Start()
    {
        playerHealth.OnHealthChange += UpdateEffect;
        grayScaleMaterial.SetFloat("_Intensity", 0f);
    }

    private void OnDestroy()
    {
        playerHealth.OnHealthChange -= UpdateEffect;
        grayScaleMaterial.SetFloat("_Intensity", 0f);
    }

    private void UpdateEffect(float currentHealth, float maxHealth)
    {
        float healthPercentage = currentHealth / maxHealth;
        float intensity = Mathf.InverseLerp(threshold, 0f, healthPercentage);

        grayScaleMaterial.SetFloat("_Intensity", intensity);
    }
}
