using UnityEngine;

public class AttackState : IState
{
    private EnemyController enemy;


    public AttackState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.Animator.SetFloat("velocity", 0f);
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (enemy.IsAttacking)
            return;

        if (enemy.CanAttack())
        {
            enemy.Attack();
        }
    }
}
