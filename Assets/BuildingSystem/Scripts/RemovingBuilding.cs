using UnityEngine;

public class RemovingBuilding : MonoBehaviour
{
    [SerializeField] PlacementSystem placementSystem;

    public HighlightingBuildings highlightingBuildings;

    public void StartRemoving()
    {
        GameObject gameObject = placementSystem.objectPlacer.placedGameObjects[highlightingBuildings.gameObjectIndex];

        if (gameObject.GetComponent<Constructable>().joinedGatherer != null)
        {
            Destroy(gameObject.GetComponent<Constructable>().joinedGatherer.gameObject);
            // Debug.Log("destroyed");
        }

        Vector3Int gridPosition = placementSystem.grid.WorldToCell(gameObject.transform.position);

        highlightingBuildings.selectedData.RemoveObjectAt(gridPosition);
        placementSystem.objectPlacer.RemoveObjectAt(highlightingBuildings.gameObjectIndex);

        highlightingBuildings.ActionButtons.SetActive(false);
    }
}
