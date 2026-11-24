using UnityEngine;

public class Initialization : MonoBehaviour
{
    public PlacementSystem placementSystem;
    private void Start()
    {
        placementSystem.PlaceStructureTo(new Vector3Int(2, 0, 1));
        // Vector3Int gridPosition = new Vector3Int(2, 0, 1);

        // int index = objectPlacer.PlaceObject(database.objectsData[0].Prefab, grid.CellToWorld(gridPosition));

        // BuildingType buildingType = database.objectsData[0].thisBuildingType;
        // ResourceManager.Instance.UpdateBuildingChanged(buildingType, true, new Vector3());

        // GridData selectedData = placementSystem.floorData;
       
        // selectedData.AddObjectAt(gridPosition,
        //     database.objectsData[0].Size,
        //     database.objectsData[0].ID,
        //     index);

        // previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }
}
