using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private float healAmount = 50f;

    private void OnTriggerEnter(Collider other)
    {
        HealthController healthController = other.GetComponent<HealthController>();

        if (healthController != null && healthController.Team == Team.Player)
        {

            healthController.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
