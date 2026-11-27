using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask attackbale;
    public GameObject groundMarker;

    public bool attackCursorVisible;

    private Camera cam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        HandleSelectionInput();
        HandleOrderInput();
        UpdateAttackCursor();
    }

    private void HandleSelectionInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickable))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MultiSelect(hit.collider.gameObject);
            }
            else
            {
                SelectByClicking(hit.collider.gameObject);
            }
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            DeselectAll();
        }
    }

    private void HandleOrderInput()
    {
        if (!Input.GetMouseButtonDown(1) || unitsSelected.Count == 0) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Transform enemyTarget = HasOffensiveUnit(unitsSelected) ? FindEnemyUnderPointer(ray) : null;

        if (enemyTarget != null)
        {
            IssueAttackOrder(enemyTarget);
        }
        else if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ground))
        {
            IssueMoveOrder(hit.point);
        }
    }

    private void IssueAttackOrder(Transform enemyTarget)
    {
        if (groundMarker != null)
        {
            groundMarker.SetActive(false);
        }

        foreach (GameObject unit in unitsSelected)
        {
            AttackController controller = unit != null ? unit.GetComponent<AttackController>() : null;
            controller?.SetTarget(enemyTarget);
        }
    }

    private void IssueMoveOrder(Vector3 destination)
    {
        if (groundMarker != null)
        {
            groundMarker.transform.position = destination;
            groundMarker.SetActive(false);
            groundMarker.SetActive(true);
        }

        foreach (GameObject unit in unitsSelected)
        {
            if (unit == null) continue;

            UnitMovement movement = unit.GetComponent<UnitMovement>();
            if (movement == null) continue;

            if (!movement.enabled) movement.enabled = true;
            movement.CommandMoveTo(destination);
        }
    }

    private void UpdateAttackCursor()
    {
        attackCursorVisible = unitsSelected.Count > 0
            && HasOffensiveUnit(unitsSelected)
            && FindEnemyUnderPointer(cam.ScreenPointToRay(Input.mousePosition)) != null;
    }

    private Transform FindEnemyUnderPointer(Ray ray)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, attackbale);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null || hit.collider.isTrigger) continue;

            AttackController controller = hit.collider.GetComponentInParent<AttackController>();
            if (controller != null)
            {
                return controller.transform;
            }
        }

        return null;
    }

    private bool HasOffensiveUnit(List<GameObject> units)
    {
        foreach (GameObject unit in units)
        {
            if (unit != null && unit.GetComponent<AttackController>() != null)
            {
                return true;
            }
        }

        return false;
    }

    private void MultiSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    public void DeselectAll()
    {
        foreach (GameObject unit in unitsSelected)
        {
            if (unit != null)
            {
                SelectUnit(unit, false);
            }
        }

        if (groundMarker != null)
        {
            groundMarker.SetActive(false);
        }

        unitsSelected.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        unitsSelected.Add(unit);
        SelectUnit(unit, true);
    }

    internal void DragSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        EnableUnitMovement(unit, isSelected);
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        UnitMovement movement = unit.GetComponent<UnitMovement>();
        if (movement != null)
        {
            movement.enabled = shouldMove;
        }
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        if (unit.transform.childCount > 0)
        {
            unit.transform.GetChild(0).gameObject.SetActive(isVisible);
        }
    }
}
