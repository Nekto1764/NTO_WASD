using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AttackController : MonoBehaviour
{
    private const float AttackStopBuffer = 0.25f;
    private const float StationaryVelocityThreshold = 0.05f;
    private const int StationaryAvoidancePriority = 30;
    private const int MovingAvoidancePriority = 50;

    [Header("Combat")]
    [Tooltip("Радиус, на котором юнит замечает врага и начинает идти к нему")]
    public float aggroRadius = 0.8f;

    [Tooltip("Радиус поиска следующего врага после убийства текущего (2 - 3)")]
    public float autoAcquireRadius = 2.5f;

    public bool isPlayer;
    public int unitDamage;
    public Transform targetToAttack;

    [Header("Indicator")]
    public Renderer indicatorRenderer;
    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;

    private UnitMovement unitMovement;
    private NavMeshAgent agent;
    private SimplePatrol patrolScript;
    private Unit selfUnit;
    private bool wasAttackingTarget;

    private void Awake()
    {
        unitMovement = GetComponent<UnitMovement>();
        agent = GetComponent<NavMeshAgent>();
        patrolScript = GetComponent<SimplePatrol>();
        selfUnit = GetComponent<Unit>();
    }

    private void Update()
    {
        bool commandedToMove = isPlayer && unitMovement != null && unitMovement.isCommandedToMove;

        if (commandedToMove)
        {
            ClearTarget();
        }

        TryAcquireNextTarget(commandedToMove);
        TryAutoAggro(commandedToMove);
        UpdateAvoidancePriority();
        UpdateIndicatorMaterial();
    }

    public void SetTarget(Transform newTarget)
    {
        if (IsDeadTarget(newTarget)) return;

        targetToAttack = newTarget;
        wasAttackingTarget = newTarget != null;

        if (unitMovement != null)
        {
            unitMovement.isCommandedToMove = false;
        }

        // Останавливаем патруль на время боя
        if (newTarget != null && patrolScript != null)
        {
            patrolScript.enabled = false;
        }
    }

    public void ClearTarget()
    {
        targetToAttack = null;
        wasAttackingTarget = false;
    }

    // Вызывается, когда по этому юниту нанесли удар
    public void OnAttackedBy(Transform attacker)
    {
        if (attacker == null) return;
        if (selfUnit != null && selfUnit.IsDead) return;

        bool isRunningAway = unitMovement != null && unitMovement.isCommandedToMove;
        if (targetToAttack == null && !isRunningAway)
        {
            SetTarget(attacker);
        }
    }

    private void TryAcquireNextTarget(bool commandedToMove)
    {
        if (!wasAttackingTarget || targetToAttack != null) return;

        if (commandedToMove)
        {
            wasAttackingTarget = false;
            EnablePatrol();
            return;
        }

        Transform nextEnemy = FindNearestEnemyInRange(autoAcquireRadius);
        if (nextEnemy != null)
        {
            SetTarget(nextEnemy);
        }
        else
        {
            wasAttackingTarget = false;
            EnablePatrol();
        }
    }

    private void TryAutoAggro(bool commandedToMove)
    {
        if (targetToAttack != null) return;

        bool canAutoAggro = !isPlayer || (unitMovement != null && !commandedToMove);
        if (!canAutoAggro) return;

        Transform nearbyEnemy = FindNearestEnemyInRange(aggroRadius);
        if (nearbyEnemy != null)
        {
            SetTarget(nearbyEnemy);
        }
    }

    private void EnablePatrol()
    {
        if (patrolScript != null)
        {
            patrolScript.enabled = true;
        }
    }

    private void UpdateAvoidancePriority()
    {
        if (agent == null) return;

        bool stationary = agent.isStopped || agent.velocity.sqrMagnitude < StationaryVelocityThreshold;
        agent.avoidancePriority = stationary ? StationaryAvoidancePriority : MovingAvoidancePriority;
    }

    private Transform FindNearestEnemyInRange(float edgeRadius)
    {
        if (UnitSelectionManager.Instance == null) return null;

        Transform closest = null;
        float minDistance = edgeRadius;

        foreach (GameObject obj in UnitSelectionManager.Instance.allUnitsList)
        {
            if (obj == null || obj == gameObject) continue;
            if (IsDeadUnit(obj)) continue;

            AttackController other = obj.GetComponent<AttackController>();
            if (other == null || other.isPlayer == isPlayer) continue;

            float distance = GetEdgeDistanceTo(obj.transform);
            if (distance <= minDistance)
            {
                minDistance = distance;
                closest = obj.transform;
            }
        }

        return closest;
    }

    private void UpdateIndicatorMaterial()
    {
        if (indicatorRenderer == null) return;

        indicatorRenderer.material = targetToAttack != null
            ? (IsMovingToPoint() ? followStateMaterial : attackStateMaterial)
            : (IsMovingToPoint() ? followStateMaterial : idleStateMaterial);
    }

    private bool IsMovingToPoint()
    {
        return unitMovement != null
            && unitMovement.isCommandedToMove
            && agent != null
            && (agent.pathPending || (agent.hasPath && agent.remainingDistance > agent.stoppingDistance));
    }

    private bool IsDeadTarget(Transform target)
    {
        if (target == null) return false;

        Unit unit = target.GetComponent<Unit>();
        return unit != null && unit.IsDead;
    }

    private bool IsDeadUnit(GameObject obj)
    {
        Unit unit = obj.GetComponent<Unit>();
        return unit != null && unit.IsDead;
    }

    public void SetIdleMaterial() { }
    public void SetFollowMaterial() { }
    public void SetAttackMaterial() { }

    public float GetBodyRadius(Transform t)
    {
        if (t == null) return 0f;

        CapsuleCollider capsule = t.GetComponent<CapsuleCollider>();
        if (capsule == null) return 0f;

        float maxScale = Mathf.Max(t.lossyScale.x, t.lossyScale.z);
        return capsule.radius * maxScale;
    }

    public float GetEdgeDistanceTo(Transform target)
    {
        if (target == null) return float.MaxValue;

        float centerDistance = Vector3.Distance(transform.position, target.position);
        float edgeDistance = centerDistance - GetBodyRadius(transform) - GetBodyRadius(target);
        return Mathf.Max(0f, edgeDistance);
    }

    public float GetAttackStopCenterDistance(Transform target)
    {
        float bodySum = GetBodyRadius(transform) + GetBodyRadius(target);
        return bodySum + AttackStopBuffer;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, autoAcquireRadius);
    }
}
