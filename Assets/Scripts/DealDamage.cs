using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        var healthController = other.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.TakeDamage(damage);
        }
    }
}
