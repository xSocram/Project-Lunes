using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    LineOfSight los;
    [SerializeField] private GameObject player;//reference to the player object
    [SerializeField] private Transform eyePoint;//reference to the point from which the enemy sees
    [SerializeField] private PlayerController playerController;

    private Animator animator;
    private CharacterController controller;
    private FSM fsm;//finite state machine

    [Header("Settings")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Wander")]
    [SerializeField] private float wanderInterval = 2f;
    private float wanderTimer;
    private Vector3 wanderDir;

    private float lastAttackTime;
    private Vector3 velocity;

    private IState patrolState;
    private IState chaseState;
    private IState attackState;
    private IState idleState;
    private IState currentState;

    [Header("Getter")]
    public Animator Animator => animator;
    public GameObject Player => player;
    public Vector3 playerVelocity => playerController.Velocity;

    private void Awake()
    {
        los = GetComponent<LineOfSight>();
        controller = GetComponent<CharacterController>();
        fsm = GetComponent<FSM>();
        animator = GetComponent<Animator>();

        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        idleState = new IdleState(this);

        wanderDir = transform.forward;
        wanderTimer = 0f;
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
            case FSM.EnemyState.Idle:
                currentState = idleState;
                break;
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

    public void Move(Vector3 dir, float moveSpeed ,float animMultiplier = 1f)
    {
        controller.Move(dir * moveSpeed * Time.deltaTime);

        if(dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

        }
        animator.SetFloat("velocity", dir.magnitude * animMultiplier);
    }

    public void Wander(float animMultiplier)
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0) 
        { 
            wanderDir = SteeringBehaviours.Wander(transform.forward, 180f);
            wanderTimer = wanderInterval;
        }

        wanderDir = AvoidObstacles(wanderDir);

        Move(wanderDir,patrolSpeed,animMultiplier);
    }

    private Vector3 AvoidObstacles(Vector3 dir)
    {
        if(los.CheckForwardObstacle(transform, dir, 2f))
        {
            float angle = Random.Range(-90f, 90f);
            Vector3 newDir = Quaternion.Euler(0, angle, 0) * transform.forward;
            return newDir.normalized;
        }

        return dir;
    }

    public void ResetWanderTimer()
    {
        wanderDir = transform.forward;
        wanderTimer = wanderInterval;
    }

    public void SeekPlayer()
    {
        Vector3 dir = SteeringBehaviours.Seek(transform, player.transform.position);
        Move(dir,chaseSpeed);
    }

    public void Attack()
    {
        lastAttackTime = Time.time;

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

    public void PursuePlayer()
    {
        Vector3 dir = 
            SteeringBehaviours.Pursue(transform, player.transform, playerVelocity, 0.5f) * 0.7f +
            SteeringBehaviours.Seek(transform, player.transform.position) * 0.3f;

        dir = dir.normalized;

        Move(dir,chaseSpeed);
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
