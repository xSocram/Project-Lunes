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

    private Vector3 lastPosition;
    public Vector3 Velocity {  get; private set; }

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

    private void Block(bool isBusy)
    {
        bool canBlock = isInCombat && isBlocking && !isBusy;

        animator.SetBool("block", canBlock);
    }

    private void Update()
    {
        bool isBusy = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") ||
              (animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).IsTag("Attack"));

        timeSinceLastAttack += Time.deltaTime;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        animator.SetBool("isInCombat", isInCombat);

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;
        if (move.sqrMagnitude > 1f)
            move.Normalize();

        bool canBlock = isInCombat && isBlocking;

        if (canBlock || isBusy)
        {
            move = Vector3.zero;
        }

        float currentSpeed = speed;

        if (!isInCombat && isSprinting)
            currentSpeed *= sprintMultiplier;

        Vector3 lookDirection = isInCombat ? camForward : move;

        Vector3 localMove = transform.InverseTransformDirection(move);

        if (isInCombat)
        {
            animator.SetFloat("moveX", localMove.x, 0.1f, Time.deltaTime);
            animator.SetFloat("moveY", localMove.z, 0.1f, Time.deltaTime);
        }
        else
        {
            float speedPercent = isSprinting ? 1f : 0.5f;
            float velocityAnim = moveInput.magnitude * speedPercent;

            if (isBusy)
                velocityAnim = 0f;

            animator.SetFloat("velocity", velocityAnim, 0.1f, Time.deltaTime);
        }

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }


        Vector3 movement = Vector3.zero;

        if (!isAttacking)
        {
            movement = move * currentSpeed + velocity;
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;
        }

        controller.Move(movement * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        if (isBusy)
        {
            isBlocking = false;
        }

        Block(isBusy);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);
        } 

        Velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
}
