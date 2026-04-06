using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField]private float jumpHeight = 2f;
    [SerializeField] private float speed =5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    private bool isSprinting;

    [Header("Combat Variables")]
    private bool isInCombat;
    private bool isBlocking;
    private bool isAttacking;
    private float timeSinceLastAttack;
    private int currentAttackCombo = 0;
    private bool canCombo;

    private Vector2 moveInput;
    private Vector3 velocity;

    [Header("Camera Stuff")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool shouldFaceMoveDirection;

    [Header("Reference")]
    private CharacterController controller;
    private Animator animator;


    private void Awake()
    {   
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void OnCombat(InputAction.CallbackContext context)
    {
        isInCombat = context.ReadValueAsButton();
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        isBlocking = context.ReadValueAsButton();

        Debug.Log("OnBlock triggered | value: " + isBlocking + " | phase: " + context.phase);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isAttacking)
        {
            if (canCombo)
            {
                canCombo = false;
                Attack();
            }

            return;
        }

        Attack();
    }

    private void Attack()
    {
        if (!isInCombat || !controller.isGrounded) return;
        if (isBlocking) return;

        if (timeSinceLastAttack > 1.5f)
            currentAttackCombo = 0;

        currentAttackCombo++;

        if (currentAttackCombo > 3)
            currentAttackCombo = 1;

        isAttacking = true;
        canCombo = false;

        animator.SetTrigger("attack" + currentAttackCombo);

        timeSinceLastAttack = 0f;
    }

    public void EnableCombo()
    {
        canCombo = true;
    }

    public void ResetAttack()
    {
        isAttacking = false;
        canCombo = false;
    }

    private void Block()
    {
        bool canBlock = isInCombat && isBlocking;

        animator.SetBool("block", canBlock);
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        animator.SetBool("isInCombat", isInCombat);

        Vector3 move = forward * moveInput.y + right * moveInput.x;
        move.Normalize();

        bool canBlock = isInCombat && isBlocking;

        if (canBlock)
        {
            move = Vector3.zero;
        }

        float currentSpeed = isInCombat ? speed : (isSprinting ? speed * sprintMultiplier : speed);

        if (isInCombat)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            if (move.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (isInCombat)
        {
            animator.SetFloat("moveX", moveInput.x, 0.1f, Time.deltaTime);
            animator.SetFloat("moveY", moveInput.y, 0.1f, Time.deltaTime);
        }
        else
        {
            float targetVelocity = isSprinting ? 1f : 0.5f;
            float finalVelocity = moveInput.magnitude * targetVelocity;
            animator.SetFloat("velocity", finalVelocity, 0.1f, Time.deltaTime);
        }

        Block();

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);
        } 
    }
}
