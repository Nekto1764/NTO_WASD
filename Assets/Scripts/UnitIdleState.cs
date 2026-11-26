using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
    private AttackController attackController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.GetComponent<AttackController>();
        attackController?.SetIdleMaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController == null)
        {
            attackController = animator.GetComponent<AttackController>();
        }

        if (attackController != null && attackController.targetToAttack != null)
        {
            animator.SetBool("isFollowing", true);
        }
    }
}
