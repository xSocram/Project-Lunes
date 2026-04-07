using UnityEngine;

public class IdleState: IState
{
    private EnemyController enemy;

    private Quaternion targetRotation;
    public IdleState(EnemyController enemy)
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
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 2f);
    }
}