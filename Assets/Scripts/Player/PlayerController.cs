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

    private void Awake()
    {
        instance = this;

        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();

        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
        Animations = GetComponent<PlayerAnimationController>();
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
}