using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour
{
    public Sprite availableSprite;
    public Sprite unAvailableSprite;

    private bool isAvaibale;

    public BuySystem buySystem;

    public int databaseItemID;

    public float trainingTime = 5;

    // public BuildSlotProgress myBuildSlotProgress;

    // private bool isTraining = false;
    // private bool isReadyToPlace = false;
    public GameObject readyText;

    private void Start()
    {
        ResourceManager.Instance.OnResourceChanged += HandleResourcesChanged;
        HandleResourcesChanged();

        ResourceManager.Instance.OnBuildingsChanged += HandleBuildingsChanged;
        HandleBuildingsChanged();
    }

    private void OnDestroy()
    {
        ResourceManager.Instance.OnResourceChanged -= HandleResourcesChanged;
        ResourceManager.Instance.OnBuildingsChanged -= HandleBuildingsChanged;
    }

    public void ClicedOnSlot()
    {
        if (isAvaibale)
        {
            if (buySystem.IsDatabaseItemABuilding(databaseItemID))
            {
                buySystem.placementSystem.StartPlacement(databaseItemID);
            }
            else
            {
                // myBuildSlotProgress.StartBuilding(trainingTime);
                // StartCoroutine(buySystem.StartUnitTraining(databaseItemID, trainingTime));
            }
        }
    }

    private void UpdateAvailabilityUI()
    {
        if (isAvaibale)
        {
            GetComponent<Image>().sprite = availableSprite;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Image>().sprite = unAvailableSprite;
            GetComponent<Button>().interactable = false;
        }
    }

    private void HandleResourcesChanged()
    {
        ObjectData objectData = DatabaseManager.Instance.databaseSO.objectsData[databaseItemID];

        bool requirementMet = true;

        foreach (BuildRequirement req in objectData.resourceRequirements)
        {
            if (ResourceManager.Instance.GetResourceAmount(req.resource) < req.amount)
            {
                requirementMet = false;
                break;
            }
        }

        isAvaibale = requirementMet;

        UpdateAvailabilityUI();
    }

    private void HandleBuildingsChanged()
    {
        ObjectData objectData = DatabaseManager.Instance.databaseSO.objectsData[databaseItemID];

        foreach (BuildingType dependency in objectData.buildDependency)
        {
            if (dependency == BuildingType.None)
            {
                gameObject.SetActive(true);
                return;
            }

            if (!ResourceManager.Instance.allExistiongBuildings.Contains(dependency))
            {
                gameObject.SetActive(false);
                return;
            }
        }

        gameObject.SetActive(true);
    }
}
