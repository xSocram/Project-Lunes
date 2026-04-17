using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;

    [SerializeField] private StatusEffect[] effects;

    private void OnTriggerEnter(Collider other)
    {
        var otherHealth = other.GetComponent<HealthController>();
        var myHealth = GetComponentInParent<HealthController>();

        if (otherHealth == null || myHealth == null) return;

        if (otherHealth.Team == myHealth.Team) return;

        otherHealth.TakeDamage(damage);

        if(effects != null)
        {
            foreach (var effect in effects)
            {
                effect.Apply(otherHealth);
            }
        }    
    }
}