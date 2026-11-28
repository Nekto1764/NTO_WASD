using UnityEngine;
using UnityEngine.EventSystems;

public class RTSCameraController : MonoBehaviour
{
    public static RTSCameraController instance;

    [Header("General")]
    [SerializeField] private Transform cameraTransform;
    public Transform followTransform;

    [Header("Optional Functionality")]
    [SerializeField] private bool moveWithKeyboad;
    [SerializeField] private bool moveWithEdgeScrolling;
    [SerializeField] private bool moveWithMouseDrag;

    [Header("Keyboard Movement")]
    [SerializeField] private float fastSpeed = 0.05f;
    [SerializeField] private float normalSpeed = 0.01f;
    [SerializeField] private float movementSensitivity = 2f;

    [Header("Edge Scrolling Movement")]
    [SerializeField] private float edgeSize = 50f;
    public Texture2D cursorArrowUp;
    public Texture2D cursorArrowDown;
    public Texture2D cursorArrowLeft;
    public Texture2D cursorArrowRight;

    private Vector3 newPosition;
    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private float movementSpeed;
    private bool isCursorSet;
    private CursorArrow currentCursor = CursorArrow.DEFAULT;

    private enum CursorArrow
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT
    }

    private void Start()
    {
        instance = this;
        newPosition = transform.position;
        movementSpeed = normalSpeed;
    }

    private void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else
        {
            HandleCameraMovement();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }
    }

    private void HandleCameraMovement()
    {
        if (moveWithMouseDrag)
        {
            HandleMouseDragInput();
        }

        if (moveWithKeyboad)
        {
            HandleKeyboardInput();
        }

        if (moveWithEdgeScrolling)
        {
            HandleEdgeScrolling();
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSensitivity);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void HandleKeyboardInput()
    {
        movementSpeed = Input.GetKey(KeyCode.LeftCommand) ? fastSpeed : normalSpeed;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += transform.forward * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += transform.right * -movementSpeed;
        }
    }

    private void HandleEdgeScrolling()
    {
        Vector3 mouse = Input.mousePosition;

        if (mouse.x > Screen.width - edgeSize)
        {
            newPosition += transform.right * movementSpeed;
            SetCursor(CursorArrow.RIGHT);
            isCursorSet = true;
        }
        else if (mouse.x < edgeSize)
        {
            newPosition += transform.right * -movementSpeed;
            SetCursor(CursorArrow.LEFT);
            isCursorSet = true;
        }
        else if (mouse.y > Screen.height - edgeSize)
        {
            newPosition += transform.forward * movementSpeed;
            SetCursor(CursorArrow.UP);
            isCursorSet = true;
        }
        else if (mouse.y < edgeSize)
        {
            newPosition += transform.forward * -movementSpeed;
            SetCursor(CursorArrow.DOWN);
            isCursorSet = true;
        }
        else if (isCursorSet)
        {
            SetCursor(CursorArrow.DEFAULT);
            isCursorSet = false;
        }
    }

    private void SetCursor(CursorArrow newCursor)
    {
        if (currentCursor == newCursor) return;

        switch (newCursor)
        {
            case CursorArrow.UP:
                Cursor.SetCursor(cursorArrowUp, Vector2.zero, CursorMode.Auto);
                break;
            case CursorArrow.DOWN:
                Cursor.SetCursor(cursorArrowDown, new Vector2(cursorArrowDown.width, cursorArrowDown.height), CursorMode.Auto);
                break;
            case CursorArrow.LEFT:
                Cursor.SetCursor(cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                break;
            case CursorArrow.RIGHT:
                Cursor.SetCursor(cursorArrowRight, new Vector2(cursorArrowRight.width, cursorArrowRight.height), CursorMode.Auto);
                break;
            case CursorArrow.DEFAULT:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }

        currentCursor = newCursor;
    }

    private void HandleMouseDragInput()
    {
        // if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(2) && TryGetMouseGroundPosition(out Vector3 start))
        {
            dragStartPosition = start;
        }

        if (Input.GetMouseButton(2) && TryGetMouseGroundPosition(out Vector3 current))
        {
            dragCurrentPosition = current;
            newPosition = transform.position + dragStartPosition - dragCurrentPosition;
        }
    }

    private bool TryGetMouseGroundPosition(out Vector3 position)
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float distance))
        {
            position = ray.GetPoint(distance);
            return true;
        }

        position = Vector3.zero;
        return false;
    }
}
