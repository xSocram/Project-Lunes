using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerController player;
    private PlayerMovement movement;
    private PlayerCombat combat;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
    }

    public void UpdateAnimations()
    {
        Animator animator = player.Animator;

        animator.SetBool("isInCombat", combat.IsInCombat);

        Vector2 moveInput = movement.MoveInput;
        Vector3 moveDirection = movement.MoveDirection;

        Vector3 localMove = transform.InverseTransformDirection(moveDirection);

        if (combat.IsInCombat)
        {
            animator.SetFloat("moveX", localMove.x, 0.1f, Time.deltaTime);
            animator.SetFloat("moveY", localMove.z, 0.1f, Time.deltaTime);
        }
        else
        {
            float speedPercent = movement.IsSprinting ? 1f : 0.5f;
            float velocityAnim = moveInput.magnitude * speedPercent;

            if (combat.IsAttacking)
                velocityAnim = 0f;

            animator.SetFloat("velocity", velocityAnim, 0.1f, Time.deltaTime);
        }
    }
}