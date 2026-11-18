using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;

    [Header("Indicator Settings")]
    public Renderer indicatorRenderer;

    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;

    public bool isPlayer;

    public int unitDamage;

    private UnitMovement unitMovement;
    private NavMeshAgent agent;

    private void Start()
    {
        unitMovement = GetComponent<UnitMovement>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateIndicatorMaterial();
    }

    private void UpdateIndicatorMaterial()
    {
        if (indicatorRenderer == null) return;

        // Проверяем, идет ли юнит прямо сейчас по приказу к точке на земле
        bool isMovingToPoint = unitMovement != null 
            && unitMovement.isCommandedToMove 
            && agent != null 
            && (agent.pathPending || (agent.hasPath && agent.remainingDistance > agent.stoppingDistance));

        // 1. Идет к указанной точке -> зеленый
        if (isMovingToPoint)
        {
            indicatorRenderer.material = followStateMaterial;
        }
        // 2. Идет бить врага или уже бьет его -> красный
        else if (targetToAttack != null)
        {
            indicatorRenderer.material = attackStateMaterial;
        }
        // 3. Просто стоит на месте -> белый
        else
        {
            indicatorRenderer.material = idleStateMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayer && other.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlayer && other.CompareTag("Enemy") && targetToAttack != null)
        {
            targetToAttack = null;
        }
    }

    // Пустые методы оставлены, чтобы Animator не выдавал ошибок и не перебивал цвет
    public void SetIdleMaterial() { }
    public void SetFollowMaterial() { }
    public void SetAttackMaterial() { }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f * 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1.2f);
    }
}

