using UnityEngine;
using UnityEngine.AI;

public class WarriorFollowState : StateMachineBehaviour
{
    private const float ArriveBuffer = 0.05f;
    private const float LookRotationSpeed = 10f;

    private AttackController attackController;
    private UnitMovement unitMovement;
    private NavMeshAgent agent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.GetComponent<AttackController>();
        unitMovement = animator.GetComponent<UnitMovement>();
        agent = animator.GetComponent<NavMeshAgent>();

        attackController?.SetFollowMaterial();

        if (agent != null)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController == null || agent == null || attackController.targetToAttack == null)
        {
            animator.SetBool("isFollowing", false);
            return;
        }

        Transform target = attackController.targetToAttack;
        Unit targetUnit = target.GetComponent<Unit>();
        if (targetUnit != null && targetUnit.IsDead)
        {
            attackController.targetToAttack = null;
            animator.SetBool("isFollowing", false);
            return;
        }

        if (unitMovement != null && unitMovement.isCommandedToMove)
        {
            animator.SetBool("isFollowing", false);
            animator.SetBool("isAttacking", false);
            return;
        }

        float stopDistance = attackController.GetAttackStopCenterDistance(target);
        float distanceToTarget = Vector3.Distance(target.position, animator.transform.position);

        agent.isStopped = false;
        agent.updateRotation = true;
        agent.stoppingDistance = stopDistance;
        agent.SetDestination(target.position);

        FaceTarget(animator.transform, target.position);

        if (distanceToTarget <= stopDistance + ArriveBuffer)
        {
            EnterAttackRange(animator);
        }
    }

    private void FaceTarget(Transform self, Vector3 targetPosition)
    {
        Vector3 lookDirection = targetPosition - self.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude <= 0.001f) return;

        self.rotation = Quaternion.Slerp(
            self.rotation,
            Quaternion.LookRotation(lookDirection),
            Time.deltaTime * LookRotationSpeed);
    }

    private void EnterAttackRange(Animator animator)
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        animator.SetBool("isAttacking", true);
    }
}
