using UnityEngine;

public class HighlightingBuildings : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private PlacementSystem placementSystem;

    public GameObject BuyingSystem;
    public GameObject Actions;
    public GameObject Barrack;
    public GridData selectedData;

    public int gameObjectIndex = -1;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (BuyingSystem.activeInHierarchy)
            {
                bool isHovered = BuyingSystem.GetComponent<IsHovered>().isHovered;

                if (!isHovered)
                {
                    OnAction();
                }
            }
            else if (Actions.activeInHierarchy)
            {
                bool isHovered = Actions.GetComponent<IsHovered>().isHovered;

                if (!isHovered)
                {
                    OnAction();
                }
            }
            else if (Barrack.activeInHierarchy)
            {
                bool isHovered = Barrack.GetComponent<IsHovered>().isHovered;

                if (!isHovered)
                {
                    OnAction();
                }
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
            Actions.SetActive(false);
            Barrack.SetActive(false);

            BuyingSystem.SetActive(true);
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;

            GameObject gameObject = placementSystem.objectPlacer.placedGameObjects[gameObjectIndex];
            BuildingType selectedBuildingType = gameObject.GetComponent<Constructable>().buildingType;

            if (selectedBuildingType == BuildingType.Barrack)
            {
                Barrack.SetActive(true);

                BuyingSystem.SetActive(false);
                Actions.SetActive(false);
            }
            else
            {
                Actions.SetActive(true);

                BuyingSystem.SetActive(false);
                Barrack.SetActive(false);
            }
        }
    }
}
