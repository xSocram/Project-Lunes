using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private float upForce = 200f;
    [SerializeField] private float moveForce =10f;

    private Vector2 moveInput;

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
        moveInput=playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(moveInput.x, 0, moveInput.y) * moveForce);
    }

    public void Jump(InputAction.CallbackContext cb)
    {
        if (cb.performed)
        {
            rb.AddForce(Vector3.up * upForce);
        }      
    }

    
}
