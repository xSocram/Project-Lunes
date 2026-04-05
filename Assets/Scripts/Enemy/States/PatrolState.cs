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
        enemy.Animator.SetFloat("velocity", 0.5f);
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
