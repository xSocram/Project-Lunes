using UnityEngine;

public class ChaseState : IState
{
    private EnemyController enemy;

    public ChaseState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.Animator.SetFloat("velocity", 1f);
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        //enemy.MoveToPlayer();
        enemy.PursuePlayer();
    }
}
