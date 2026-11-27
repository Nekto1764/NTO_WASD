using UnityEngine;

public class HighlightingBuildings : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private PlacementSystem placementSystem;

    public GameObject BuyingSystem;
    public GameObject Actions;
    public GridData selectedData;

    public int gameObjectIndex = -1;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Actions.activeInHierarchy)
            {
                bool isHovered = Actions.GetComponent<IsHovered>().isHovered;

                if (!isHovered)
                {
                    OnAction();
                }
            }
            else
            {
                OnAction();
            }
        }
    }

    public void OnAction()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        selectedData = null;

        if (placementSystem.floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = placementSystem.floorData;
        }

        if (selectedData == null)
        {
            bool isHovered = Actions.GetComponent<IsHovered>().isHovered;

            if (!isHovered)
            {
                Actions.SetActive(false);
                BuyingSystem.SetActive(true);
            }
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;

            Actions.SetActive(true);
            BuyingSystem.SetActive(false);
        }
    }
}
