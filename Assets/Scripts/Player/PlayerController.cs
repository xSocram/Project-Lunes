using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }

    public PlayerMovement Movement { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerAnimationController Animations { get; private set; }

    public Vector3 Velocity { get; private set; }

    private Vector3 lastPosition;

    private HealthController health;
    private bool isDead;
    public bool IsDead => isDead;

    private void Awake()
    {
        if(isDead) return;

        instance = this;

        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();

        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
        Animations = GetComponent<PlayerAnimationController>();

        health = GetComponent<HealthController>();
    }
    private void Start()
    {
        health.OnDeath += HandleDeath;
    }

    private void Update()
    {
        Combat.HandleTimers();
        Movement.HandleMovement();
        Combat.HandleBlock();
        Animations.UpdateAnimations();

        Velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    private void HandleDeath()
    {
        isDead = true;

        Movement.enabled = false;
        Combat.enabled = false;

        Controller.enabled = false;

        Animator.SetTrigger("die");
    }
}