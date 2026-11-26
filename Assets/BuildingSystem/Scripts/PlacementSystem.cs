using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[DefaultExecutionOrder(-100)]
public class PlacementSystem : MonoBehaviour
{

    [SerializeField] private InputManager inputManager;
    public Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;

    [SerializeField] public GridData floorData;

    [SerializeField] private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    public ObjectPlacer objectPlacer;

    int selectedID;

    public IBuildingState buildingState;

    public bool inSellMode;

    private void Start()
    {
        floorData = new();
        // Debug.Log($"Start {floorData == null}");
    }

    public void StartPlacement(int ID)
    {
        // Debug.Log("Should Start Placement");

        selectedID = ID;

        // Debug.Log("Placement ID: " + ID);


        StopPlacement();

        buildingState = new PlacementState(ID, grid, previewSystem, database, floorData, objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();

        // Debug.Log("StartRemoving");

        buildingState = new RemovingState(grid, previewSystem, floorData, objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

        inputManager.OnClicked += EndSelling;
        inputManager.OnExit += EndSelling;
    }

    public void RemovePlacementData(Vector3 position)
    {
        floorData.RemoveObjectAt(grid.WorldToCell(position));
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()){
            // Debug.Log("Pointer was over UI - Returned");
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Debug.Log(gridPosition);

        buildingState.OnAction(gridPosition);


        ObjectData ob = database.GetObjectByID(selectedID);

        foreach (BuildBenefits bf in ob.benefits)
        {
            CalculateAndAddBenefit(bf);
        }

        StopPlacement();
    }

    private void EndSelling()
    {
        inSellMode = false;
    }

    public void PlaceStructureTo(Vector3Int gridPosition)
    {
        int index = objectPlacer.PlaceObject(database.objectsData[0].Prefab, grid.CellToWorld(gridPosition));

        BuildingType buildingType = database.objectsData[0].thisBuildingType;
        ResourceManager.Instance.UpdateBuildingChanged(buildingType, true, new Vector3());

        GridData selectedData = floorData;

        selectedData.AddObjectAt(gridPosition,
            database.objectsData[0].Size,
            database.objectsData[0].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);

        ObjectData ob = database.GetObjectByID(selectedID);

        foreach (BuildBenefits bf in ob.benefits)
        {
            CalculateAndAddBenefit(bf);
        }

        StopPlacement();
    }

    private void CalculateAndAddBenefit(BuildBenefits bf)
    {
        switch (bf.benefitType)
        {
            case BuildBenefits.BenefitType.Housing:
                // StatusManager.Instance.IncreaseHousing(bf.benefitAmount);
                break;
        }
    }

    private void StopPlacement()
    {
        if (buildingState == null)
            return;
       
        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        lastDetectedPosition = Vector3Int.zero;

        buildingState = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            inSellMode = true;
            StartRemoving();
        }

        if (buildingState == null)
            return;
      
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }

    }
}
