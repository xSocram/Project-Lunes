using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    private PlayerMovement movement;
    private PlayerCombat combat;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement.SetMoveInput(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
            movement.Jump();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        movement.SetSprint(context.ReadValueAsButton());
    }

    public void OnCombat(InputAction.CallbackContext context)
    {
        combat.SetCombatMode(context.ReadValueAsButton());
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        combat.SetBlock(context.ReadValueAsButton());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
            combat.OnAttack();
    }
}
