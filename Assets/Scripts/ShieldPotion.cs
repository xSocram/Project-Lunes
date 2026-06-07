using UnityEngine;

public class ShieldPotion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ShieldController shield = other.GetComponent<ShieldController>();
        if (shield != null)
        {
            shield.ActivateShield();
            Destroy(gameObject);
        }
    }
}
