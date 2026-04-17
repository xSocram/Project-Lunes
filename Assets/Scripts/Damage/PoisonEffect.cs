using System.Collections;
using UnityEngine;

public class PoisonEffect: StatusEffect
{
    [SerializeField] private float dps;
    [SerializeField] private float duration;

    public override void Apply(HealthController target)
    {
        target.StartCoroutine(DoPoison(target));
    }

    private IEnumerator DoPoison(HealthController target)
    {
        float timer = 0f;

        while(timer < duration)
        {
            target.TakeDamage(dps * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
