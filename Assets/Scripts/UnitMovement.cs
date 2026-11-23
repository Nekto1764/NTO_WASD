using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;

    public bool isCommandedToMove;

    private void Start() {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) {
                isCommandedToMove = true;
                StartCoroutine(NoCommand());
                agent.SetDestination(hit.point);
            }
        }

        // if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        // {
        //     isCommandedToMove = false;
        // }
    }

    IEnumerator NoCommand()
    {
        yield return new WaitForSeconds(1);
        isCommandedToMove = false;
    }
}
