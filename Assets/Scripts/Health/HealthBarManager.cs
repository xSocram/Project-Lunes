using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Material healthBarMaterial;

    private void Update()
    {
        healthBarMaterial.SetVector("_PlayerPos", PlayerController.instance.transform.position);
    }
}
