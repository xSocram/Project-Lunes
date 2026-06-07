using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private float shieldDuration = 10f;

    private bool shieldActive;

    private void Start()
    {
        shieldVisual.SetActive(false);
    }

    public void ActivateShield()
    {
        if (shieldActive) return;

        StartCoroutine(ShieldRoutine());
    }

    private IEnumerator ShieldRoutine()
    {
        shieldActive = true;
        shieldVisual.SetActive(true);

        yield return new WaitForSeconds(shieldDuration);

        shieldActive = false;
        shieldVisual.SetActive(false);
    }

    public bool hasShield()
    {
        return shieldActive;
    }
}
