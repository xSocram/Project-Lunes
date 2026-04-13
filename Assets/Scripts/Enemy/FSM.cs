using UnityEngine;

public class FSM : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public EnemyState currentState = EnemyState.Idle;
    private EnemyState lastState;

    [SerializeField] private float patrolTime = 3f;
    [SerializeField] private float idleTime = 2f;

    private float stateTimer;

    private void Start()
    {
        stateTimer = patrolTime;

    }

    public bool HasStateChanged()
    {
        return currentState != lastState;
    }

    public void SaveState()
    {
        lastState = currentState;
    }

    public void UpdateState(bool canSeePlayer, bool isCloseToPlayer)
    {
        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case EnemyState.Idle:

                if (canSeePlayer)
                {
                    currentState = EnemyState.Chase;
                }
                else if (stateTimer <= 0f)
                {
                    currentState = EnemyState.Patrol;
                    stateTimer = patrolTime;
                }

                break;

            case EnemyState.Patrol:

                if (canSeePlayer)
                {
                    currentState = EnemyState.Chase;
                }
                else if (stateTimer <= 0f)
                {
                    currentState = EnemyState.Idle;
                    stateTimer = idleTime;
                }

                break;

            case EnemyState.Chase:

                if (!canSeePlayer)
                {
                    currentState = EnemyState.Patrol;
                    stateTimer = patrolTime;
                }
                else if (isCloseToPlayer)
                {
                    currentState = EnemyState.Attack;
                }

                break;

            case EnemyState.Attack:

                if (!isCloseToPlayer)
                {
                    currentState = EnemyState.Chase;
                }

                break;
        }
    }
}
