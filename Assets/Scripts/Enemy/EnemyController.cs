using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    LineOfSight los;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform eyePoint;
    private CharacterController controller;
    private FSM fsm;

    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 velocity;

    private void Awake()
    {
        los = GetComponent<LineOfSight>();
        controller = GetComponent<CharacterController>();
        fsm = GetComponent<FSM>();
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

        fsm.UpdateState(canSeePlayer,isInRange);

        ExecuteState();

    }

    private void ExecuteState()
    {
        switch (fsm.currentState)
        {
            case FSM.EnemyState.Patrol:
                Patrol();
                break;

            case FSM.EnemyState.Chase:
                MoveToPlayer();
                break;

            case FSM.EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void Attack()
    {
        Debug.Log("ATTACK");

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePoint.position, attackRange);
    }

    private void Patrol()
    {
        //Debug.Log("Patrolling...");
    }

    private void MoveToPlayer()
    { 
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        controller.Move(direction * speed * Time.deltaTime); 
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
