using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySystem : MonoBehaviour
{
    public GameObject buildingsPanel;
    public GameObject unitsPanel;

    public Button buildingsButton;
    public Button unitsButton;

    public PlacementSystem placementSystem;

    [SerializeField] private ObjectsDatabaseSO database;

    #region | --- Unit Training --- |

    public bool IsDatabaseItemABuilding(int itemID)
    {
        return database.GetObjectByID(itemID).thisBuildingType != BuildingType.None;
    }

    internal IEnumerator StartUnitTraining(int databaseItemID, float trainingTime)
    {
        yield return new WaitForSeconds(trainingTime);
    
        ObjectData unitToTrain = database.GetObjectByID(databaseItemID);

        if (unitToTrain.thisUnitType == UnitType.None)
        {
            Debug.LogError("Forgot to assign a unit type");
        }

        InstantitateUnit(unitToTrain.Name);
    }

    private void InstantitateUnit(string unitName)
    {
        Constructable[] allConstructables = FindObjectsOfType<Constructable>();
        Constructable commandCenter = null;

        foreach (var c in allConstructables)
        {
            if (c.buildingType == BuildingType.CommandCenter)
            {
                commandCenter = c;
                break;
            }
        }

        if (commandCenter == null)
        {
            Debug.LogWarning("No command center found.");
        }

        GameObject unitPrefab = Resources.Load<GameObject>(unitName);

        if (unitPrefab == null)
        {
            Debug.LogWarning("-");
            return;
        }

        Vector3 spawnPosition = commandCenter.transform.position + new Vector3(-1f, 0, 0.5f);
        GameObject nuwUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
    }


    #endregion

    #region | --- UI Related --- |
    private void Start()
    {
        unitsButton.onClick.AddListener(UnitsCategorySelected);
        buildingsButton.onClick.AddListener(BuildingsCategorySelected);

        unitsPanel.SetActive(false);
        buildingsPanel.SetActive(true);
    }

    private void UnitsCategorySelected()
    {
        unitsPanel.SetActive(true);
        buildingsPanel.SetActive(false);
    }

    private void BuildingsCategorySelected()
    {
        unitsPanel.SetActive(false);
        buildingsPanel.SetActive(true);
    }

    #endregion
}
