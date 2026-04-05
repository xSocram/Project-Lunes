using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    LineOfSight los;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform eyePoint;
    private Animator animator;
    private CharacterController controller;
    private FSM fsm;

    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastAttackTime;
    private Vector3 velocity;

    private IState patrolState;
    private IState chaseState;
    private IState attackState;

    private IState currentState;

    [Header("Getter")]
    public Animator Animator => animator;

    private void Awake()
    {
        los = GetComponent<LineOfSight>();
        controller = GetComponent<CharacterController>();
        fsm = GetComponent<FSM>();
        animator = GetComponent<Animator>();

        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
    }

    private void Start()
    {
        ChangeState(fsm.currentState);
        fsm.SaveState();
    }

    private void Update()
    {
        ApplyGravity();

        bool canSeePlayer = los.CheckRange(transform, player.transform) && 
                            los.CheckAngle(transform, player.transform) &&
                            !los.CheckObstacles(transform, player.transform);

        Vector3 flatDirection = player.transform.position - transform.position;
        flatDirection.y = 0;

        bool isInRange = flatDirection.magnitude < attackRange;

        fsm.UpdateState(canSeePlayer, isInRange);

        if (fsm.HasStateChanged())
        {
            ChangeState(fsm.currentState);
            fsm.SaveState();
        }

        currentState?.Update();
    }

    private void ChangeState(FSM.EnemyState newState)
    {
        currentState?.Exit();
        switch (newState)
        {
            case FSM.EnemyState.Patrol:
                currentState = patrolState;
                break;
            case FSM.EnemyState.Chase:
                currentState = chaseState;
                break;
            case FSM.EnemyState.Attack:
                currentState = attackState;
                break;
        }
        currentState.Enter();
    }

    public void Attack()
    {
        lastAttackTime = Time.time;

        Debug.Log("ATTACK");

        RotateToPlayer();

        animator.SetTrigger("Attack");
        animator.SetFloat("velocity", 0f);
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePoint.position, attackRange);
    }

    public void Patrol()
    {
        //Debug.Log("Patrolling...");
        animator.SetFloat("velocity", 0.5f);    
    }

    public void MoveToPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();

        RotateToPlayer();

        controller.Move(direction * speed * Time.deltaTime);

        animator.SetFloat("velocity", 1f);
    }

    private Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position);
        direction.y = 0;
        return direction.normalized;
    }

    public void RotateToPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;       
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 gravityMove = new Vector3(0, velocity.y, 0);
        controller.Move(gravityMove * Time.deltaTime);
    }
}
