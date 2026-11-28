using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySystem : MonoBehaviour
{
    public PlacementSystem placementSystem;

    public ObjectsDatabaseSO database;

    public bool IsDatabaseItemABuilding(int itemID)
    {
        return database.GetObjectByID(itemID).thisBuildingType != BuildingType.None;
    }

    #region | --- Unit Training --- |

    public void StartUnitTraining(int databaseItemID, int buildingID, float trainingTime)
    {
        StartCoroutine(StartUnitTrainingCoroutine(databaseItemID, buildingID, trainingTime));
    }

    internal IEnumerator StartUnitTrainingCoroutine(int databaseItemID, int buildingID, float trainingTime)
    {
        yield return new WaitForSeconds(trainingTime);
    
        ObjectData unitToTrain = database.GetObjectByID(databaseItemID);

        if (unitToTrain.thisUnitType == UnitType.None)
        {
            Debug.LogError("Forgot to assign a unit type");
        }

        InstantitateUnit(unitToTrain.Name, buildingID);
    }

    private void InstantitateUnit(string unitName, int buildingID)
    {
        Constructable barrack = placementSystem.objectPlacer.placedGameObjects[buildingID].GetComponent<Constructable>();
        GameObject unitPrefab = Resources.Load<GameObject>(unitName);

        if (unitPrefab == null)
        {
            Debug.LogWarning("-");
            return;
        }

        Vector3 spawnPosition = barrack.transform.position + new Vector3(-0.75f, 0, 0);
        GameObject nuwUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
        UnitMovement unitMovement = nuwUnit.GetComponent<UnitMovement>();

        unitMovement.MoveTo(spawnPosition + new Vector3(-0.5f, 0, 0), false);
    }

    #endregion
}
