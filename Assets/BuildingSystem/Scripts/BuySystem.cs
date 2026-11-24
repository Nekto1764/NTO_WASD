using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySystem : MonoBehaviour
{
    public PlacementSystem placementSystem;

    [SerializeField] private ObjectsDatabaseSO database;

    public bool IsDatabaseItemABuilding(int itemID)
    {
        return database.GetObjectByID(itemID).thisBuildingType != BuildingType.None;
    }
}
