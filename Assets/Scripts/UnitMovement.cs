using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;

    public bool isCommandedToMove;

    public bool allowManualInput = true;

    private void Start() {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();

        // Debug.Log("UnitMovement Start");
    }

    private void Update() {
        if (!allowManualInput) return;

        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) {
                MoveTo(hit.point, true);
            }
        }
    }

    public void MoveTo(Vector3 position, bool isPlayerCommand)
    {
        isCommandedToMove = true;
        StartCoroutine(NoCommand());

        agent.SetDestination(position);

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
    }

    IEnumerator NoCommand()
    {
        yield return new WaitForSeconds(1);
        isCommandedToMove = false;
    }
}
