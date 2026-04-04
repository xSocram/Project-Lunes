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

    private void Update()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;

        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float targetVelocity = isSprinting ? 1f : 0.5f;
        float finalVelocity = moveInput.magnitude * targetVelocity;
        animator.SetFloat("velocity", finalVelocity);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);
        }
    }
}
