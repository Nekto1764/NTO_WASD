using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public int credits = 300;

    public event Action OnResourceChanged;
    public event Action OnBuildingsChanged;

    public TextMeshProUGUI creditsUI;

    public List<BuildingType> allExistiongBuildings;

    public enum ResourcesType
    {
        Credits
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateBuildingChanged(BuildingType buildingType, bool isNew)
    {
        if (isNew)
        {
            allExistiongBuildings.Add(buildingType);
        }
        else
        {
            allExistiongBuildings.Remove(buildingType);
        }

        OnBuildingsChanged?.Invoke();
    }

    public void IncreaseResource(ResourcesType resource, int amountToIncrease)
    {
        switch (resource)
        {
            case ResourcesType.Credits:
                credits += amountToIncrease;
                break;
            default:
                break;
        }

        OnResourceChanged?.Invoke();
    }

    public void DecreaseResource(ResourcesType resource, int amountToIncrease)
    {
        switch (resource)
        {
            case ResourcesType.Credits:
                credits -= amountToIncrease;
                break;
            default:
                break;
        }

        OnResourceChanged?.Invoke();
    }

    private void OnEnable()
    {
        OnResourceChanged += UpdateUI;
    }

    private void OnDisable()
    {
        OnResourceChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        creditsUI.text = $"{credits}";
    }

    public int GetCredits()
    {
        return credits;
    }

    internal int GetResourceAmount(ResourcesType resource)
    {
        switch (resource)
        {
            case ResourcesType.Credits:
                return credits;
            default:
                break;
        }

        return 0;
    }

    internal void DecreaseResourcesBasedOnRequirement(ObjectData objectData)
    {
        foreach (BuildRequirement req in objectData.resourceRequirements){
            DecreaseResource(req.resource, req.amount);
        }
    }
}
