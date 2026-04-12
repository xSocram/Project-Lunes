using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController player;

    private bool isInCombat;
    private bool isBlocking;
    private bool isAttacking;
    private bool canCombo;

    private float timeSinceLastAttack;
    private int currentAttackCombo;

    public bool IsInCombat => isInCombat;
    public bool IsBlocking => isBlocking;
    public bool IsAttacking => isAttacking;

    [SerializeField] private Collider weaponCollider;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    public void SetCombatMode(bool value) => isInCombat = value;
    public void SetBlock(bool value) => isBlocking = value;

    public void OnAttack()
    {
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
        if(!isInCombat || !player.Controller.isGrounded) return;
        if(isBlocking) return;

        if(timeSinceLastAttack > 1.5f)
            currentAttackCombo = 0;

        currentAttackCombo++;

        if(currentAttackCombo > 3)
            currentAttackCombo = 1;

        isAttacking = true;
        canCombo = false;

        player.Animator.SetTrigger($"attack{currentAttackCombo}");
        timeSinceLastAttack = 0;
    }

    public void EnableCombo()
    {
        canCombo = true;
    }

    public void ResetAttack()
    {
        isAttacking = false;
    }

    public void HandleTimers()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    public void HandleBlock()
    {
        bool canBlock = isInCombat && isBlocking && !isAttacking;
        player.Animator.SetBool("block", canBlock);
    }

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
}
