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

    public int wood = 100;
    public int stone = 100;
    public int population = 10;

    public int woodCoefficient = 2;
    public int stoneCoefficient = 2;

    public event Action OnResourceChanged;
    public event Action OnBuildingsChanged;

    public TextMeshProUGUI woodUI;
    public TextMeshProUGUI stoneUI;
    public TextMeshProUGUI populationUI;

    public List<BuildingType> allExistiongBuildings;

    public PlacementSystem placementSystem;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateBuildingChanged(BuildingType buildingType, bool isNew, Vector3 position)
    {
        if (isNew)
        {
            allExistiongBuildings.Add(buildingType);
        }
        else
        {
            placementSystem.RemovePlacementData(position);
            allExistiongBuildings.Remove(buildingType);
        }

        OnBuildingsChanged?.Invoke();
    }

    public void IncreaseResource(ResourceType resource, int amountToIncrease)
    {
        switch (resource)
        {
            case ResourceType.Wood:
                wood += amountToIncrease;
                break;
            case ResourceType.Stone:
                stone += amountToIncrease;
                break;
            case ResourceType.Population:
                population += amountToIncrease;
                break;
            default:
                break;
        }

        OnResourceChanged?.Invoke();
    }

    public void DecreaseResource(ResourceType resource, int amountToIncrease)
    {
        switch (resource)
        {
            case ResourceType.Wood:
                wood -= amountToIncrease;
                break;
            case ResourceType.Stone:
                stone -= amountToIncrease;
                break;
            case ResourceType.Population:
                population -= amountToIncrease;
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
        woodUI.text = $"[Wood] {wood}";
        stoneUI.text = $"[Stone] {stone}";
        populationUI.text = $"{population} [Population]";
    }

    public int GetWood()
    {
        return wood;
    }

    public int GetStone()
    {
        return wood;
    }

    public int GetPopulation()
    {
        return population;
    }

    internal int GetResourceAmount(ResourceType resource)
    {
        switch (resource)
        {
            case ResourceType.Wood:
                return wood;
            case ResourceType.Stone:
                return stone;
            case ResourceType.Population:
                return population;
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


public enum ResourceType
{
    None,
    
    Wood,
    Stone,

    Population
}