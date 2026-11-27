using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float stoppingDistance = 0.2f;
    [SerializeField] private float agentRadius = 0.6f;
    [SerializeField] private float turnSpeed = 360f;

    [Header("Input")]
    public LayerMask ground;

    public bool isCommandedToMove;
    public bool allowManualInput = true;

    [SerializeField] private bool applyMovementSettings = true;

    private Camera cam;
    private NavMeshAgent agent;
    private AttackController attackController;

    private void Awake()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        attackController = GetComponent<AttackController>();
    }

    private void Start()
    {
        if (applyMovementSettings)
        {
            ApplyMovementSettings();
        }
    }

    private void Update()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null || agent == null) return;

        if (allowManualInput)
        {
            HandleMoveInput();
        }

        UpdateArrival();
    }

    private void ApplyMovementSettings()
    {
        if (agent == null) return;

        agent.autoBraking = false;
        agent.stoppingDistance = stoppingDistance;
        agent.acceleration = acceleration;
        agent.speed = moveSpeed;
        agent.angularSpeed = turnSpeed;
        agent.updateRotation = true;
        agent.radius = agentRadius;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    public void CommandMoveTo(Vector3 destination)
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (agent == null) return;

        attackController?.ClearTarget();

        if (applyMovementSettings)
        {
            ApplyMovementSettings();
        }

        isCommandedToMove = true;
        agent.isStopped = false;
        agent.ResetPath();
        agent.SetDestination(destination);
    }

    public void MoveTo(Vector3 position, bool isPlayerCommand)
    {
        if (isPlayerCommand)
        {
            Gatherer gatherer = GetComponent<Gatherer>();

            if (gatherer != null)
            {
                if (gatherer.assignedNode == null || Vector3.Distance(gatherer.assignedNode.position, position) > 2f)
                {
                    gatherer.CancelHarvesting();
                }
            }
        }

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (agent == null) return;

        if (isCommandedToMove && agent.hasPath && !agent.pathPending
            && Vector3.Distance(agent.destination, position) < 0.25f)
        {
            agent.isStopped = false;
            return;
        }

        CommandMoveTo(position);
    }

    private void HandleMoveInput()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (IsPointerOverEnemy(ray))
        {
            isCommandedToMove = false;
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ground))
        {
            CommandMoveTo(hit.point);
        }
    }

    private bool IsPointerOverEnemy(Ray ray)
    {
        if (UnitSelectionManager.Instance == null) return false;

        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, UnitSelectionManager.Instance.attackbale);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && !hit.collider.isTrigger
                && hit.collider.GetComponentInParent<AttackController>() != null)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateArrival()
    {
        if (!isCommandedToMove || agent.pathPending) return;

        if (!agent.hasPath)
        {
            isCommandedToMove = false;
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedToMove = false;
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }
    }
}
