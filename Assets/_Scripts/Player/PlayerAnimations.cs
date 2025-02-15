using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void IsMoving(bool isMoving)
    {
        animator.SetBool(Constants.ANIMATIONS.PLAYER.IS_MOVING, isMoving);
    }

    public void SetMovement(float horizontalInput, float verticalInput)
    {
        animator.SetFloat(Constants.ANIMATIONS.PLAYER.HORIZONTAL_INPUT, horizontalInput);
        animator.SetFloat(Constants.ANIMATIONS.PLAYER.VERTICAL_INPUT, verticalInput);
    }

    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }
}
