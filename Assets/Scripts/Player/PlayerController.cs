using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private float upForce = 200f;
    [SerializeField] private float moveForce =10f;

    private Vector2 moveInput;
    private Vector3 moveDirection;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool shouldFaceMoveDirection;

    [Header("Reference")]
    private Rigidbody rb;
    private PlayerInput playerInput;




    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * moveInput.y + right * moveInput.x;

        if (shouldFaceMoveDirection && moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }

    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection * moveForce);
    }


    public void Jump(InputAction.CallbackContext cb)
    {
        if (cb.performed)
        {
            rb.AddForce(Vector3.up * upForce);
        }      
    }

    
}
