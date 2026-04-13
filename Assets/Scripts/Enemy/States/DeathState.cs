using UnityEngine;

public class DeathState:IState
{
    private EnemyController enemy;

    public DeathState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.Animator.SetTrigger("die");
        enemy.DisableDamageCollider();
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}