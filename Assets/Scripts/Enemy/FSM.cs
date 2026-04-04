using UnityEngine;

public class FSM : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    public EnemyState currentState = EnemyState.Patrol;

    public void UpdateState(bool canSeePlayer, bool isCloseToPlayer)
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                if (canSeePlayer)
                    currentState = EnemyState.Chase;
                break;

            case EnemyState.Chase:
                if (!canSeePlayer)
                    currentState = EnemyState.Patrol;
                else if (isCloseToPlayer)
                    currentState = EnemyState.Attack;
                break;

            case EnemyState.Attack:
                if (!isCloseToPlayer)
                    currentState = EnemyState.Chase;
                break;
        }

        //Debug.Log("Current State: " + currentState);
    }

}
