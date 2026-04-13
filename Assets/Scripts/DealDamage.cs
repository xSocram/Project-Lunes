using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        var otherHealth = other.GetComponent<HealthController>();
        var myHealth = GetComponentInParent<HealthController>();

        if (otherHealth == null || myHealth == null) return;

        if (otherHealth.Team == myHealth.Team) return;

        Debug.Log($"Haciendo {damage} de daño a: {other.gameObject.name}");
        otherHealth.TakeDamage(damage);
    }
}