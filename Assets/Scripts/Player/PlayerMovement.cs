using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform cameraTransform;

    private PlayerController player;
    private PlayerCombat combat;

    private Vector2 moveInput;
    private Vector3 currentMoveDirection;
    private Vector3 velocity;
    private bool isSprinting;

    public Vector2 MoveInput => moveInput;
    public Vector3 MoveDirection => currentMoveDirection;
    public bool IsSprinting => isSprinting;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        combat = GetComponent<PlayerCombat>();
    }

    public void SetMoveInput(Vector2 input) => moveInput = input;
    public void SetSprint(bool value) => isSprinting = value;

    public void Jump()
    {
        if(player.Controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        player.Animator.SetBool("isJumping", true);
    }

    public void HandleMovement()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        currentMoveDirection = move;

        bool canBlock = combat.IsInCombat && combat.IsBlocking;

        if (canBlock || combat.IsAttacking)
        {
            move = Vector3.zero;
        }

        float currentSpeed = speed;

        if (!combat.IsInCombat && isSprinting)
            currentSpeed *= sprintMultiplier;

        Vector3 lookDirection = combat.IsInCombat ? camForward : move;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        Vector3 movement = Vector3.zero;

        if (!combat.IsAttacking)
        {
            movement = move * currentSpeed + velocity;
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;
        }

        player.Controller.Move(movement * Time.deltaTime);

        if (player.Controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            player.Animator.SetBool("isJumping", false);
        }
    }
}
