using UnityEngine;

public class CowardEnemyController : EnemyController
{

    [Header("Coward Enemy Settings")]
    [SerializeField] private float fleeSpeed = 5f;

    public override void PursuePlayer()
    {
        Vector3 dir = SteeringBehaviours.Flee(transform, Player.transform.position);
        Move(dir, fleeSpeed);
    }
}
