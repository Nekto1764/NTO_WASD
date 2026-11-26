using UnityEngine;

public class UnitSelectionBox : MonoBehaviour
{
    [SerializeField] private RectTransform boxVisual;

    private Camera cam;
    private Rect selectionBox;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private void Start()
    {
        cam = Camera.main;
        DrawVisual();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            if (boxVisual.rect.width > 0f || boxVisual.rect.height > 0f)
            {
                UnitSelectionManager.Instance.DeselectAll();
                SelectUnits();
            }

            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    private void DrawVisual()
    {
        boxVisual.position = (startPosition + endPosition) / 2f;
        boxVisual.sizeDelta = new Vector2(
            Mathf.Abs(startPosition.x - endPosition.x),
            Mathf.Abs(startPosition.y - endPosition.y));
    }

    private void DrawSelection()
    {
        Vector2 mouse = Input.mousePosition;

        selectionBox.xMin = Mathf.Min(mouse.x, startPosition.x);
        selectionBox.xMax = Mathf.Max(mouse.x, startPosition.x);
        selectionBox.yMin = Mathf.Min(mouse.y, startPosition.y);
        selectionBox.yMax = Mathf.Max(mouse.y, startPosition.y);
    }

    private void SelectUnits()
    {
        foreach (GameObject unit in UnitSelectionManager.Instance.allUnitsList)
        {
            if (selectionBox.Contains(cam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelectionManager.Instance.DragSelect(unit);
            }
        }
    }
}
