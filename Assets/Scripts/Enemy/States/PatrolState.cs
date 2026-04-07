using UnityEngine;

public class PatrolState : IState
{
    private EnemyController enemy;

    public PatrolState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.ResetWanderTimer();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        enemy.Wander(0.5f);
    }
}
