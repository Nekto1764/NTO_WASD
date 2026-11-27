using UnityEngine;
using UnityEngine.AI;

public class WarriorAttackState : StateMachineBehaviour
{
    private const float ResumeDistanceBuffer = 0.15f;
    private const float InitialAttackDelay = 0.1f;

    public float attackRate = 1.5f;
    public float attackTimer;

    private NavMeshAgent agent;
    private AttackController attackController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.updateRotation = false;
        }

        attackTimer = InitialAttackDelay;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController == null || agent == null)
        {
            StopAttacking(animator);
            return;
        }

        Unit selfUnit = animator.GetComponent<Unit>();
        if (selfUnit != null && selfUnit.IsDead)
        {
            StopAttacking(animator);
            return;
        }

        UnitMovement movement = animator.GetComponent<UnitMovement>();
        bool commandedToMove = movement != null && movement.isCommandedToMove;

        if (attackController.targetToAttack == null || commandedToMove)
        {
            StopAttacking(animator);
            return;
        }

        Transform target = attackController.targetToAttack;
        Unit targetUnit = target.GetComponent<Unit>();
        if (targetUnit != null && targetUnit.IsDead)
        {
            attackController.targetToAttack = null;
            StopAttacking(animator);
            return;
        }

        float stopDistance = attackController.GetAttackStopCenterDistance(target);
        float resumeDistance = stopDistance + ResumeDistanceBuffer;
        float distanceToTarget = Vector3.Distance(target.position, animator.transform.position);

        if (distanceToTarget > resumeDistance)
        {
            StopAttacking(animator);
            return;
        }

        HoldPositionAndFaceTarget(stopDistance);

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = 1f / attackRate;
        }
    }

    private void HoldPositionAndFaceTarget(float stopDistance)
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.stoppingDistance = stopDistance;
        LookAtTarget();
    }

    private void StopAttacking(Animator animator)
    {
        if (agent != null)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
        }
        animator.SetBool("isAttacking", false);
    }

    private void Attack()
    {
        Transform target = attackController.targetToAttack;
        if (target == null) return;

        Unit targetUnit = target.GetComponent<Unit>();
        if (targetUnit != null && targetUnit.IsDead)
        {
            attackController.targetToAttack = null;
            return;
        }

        AttackController targetController = target.GetComponent<AttackController>();
        targetController?.OnAttackedBy(attackController.transform);

        targetUnit?.TakeDamage(attackController.unitDamage);
    }

    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            agent.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
