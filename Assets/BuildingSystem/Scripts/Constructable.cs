using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Constructable : MonoBehaviour, IDamageable
{
    private float constHealth;
    public float constMaxHealth;

    public GameObject treesFinder;

    // public HealthTracker healthTracker;

    public bool isEnemy = false;

    public BuildingType buildingType;

    public Vector3 buildPosition; 

    private void Start()
    {
        constHealth = constMaxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        // healthTracker.UpdateSliderValue(constHealth, constMaxHealth);

        if (constHealth <= 0)
        {
            ResourceManager.Instance.UpdateBuildingChanged(buildingType, false, buildPosition);

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        constHealth -= damage;
        UpdateHealthUI();
    }

    public void ConstructableWasPlaced()
    {
        if (treesFinder != null)
        {
            treesFinder.SetActive(true);
        }
        // healthTracker.gameObject.SetActive(true);

        ActivateObstacle();

        if (isEnemy)
        {
            gameObject.tag = "Enemy";
        }
        if (buildingType == BuildingType.Sawmill)
        {
            GameObject prefab = Resources.Load<GameObject>("Lumberjack");

            if (prefab != null)
            {
                Transform supplyDropTransform = transform.Find("SupplyDrop").transform;
                GameObject instance = Instantiate(prefab, supplyDropTransform.position, Quaternion.identity);
                instance.transform.SetParent(null);

                Lumberjack lumberjack = instance.GetComponent<Lumberjack>();
                lumberjack.sawmill = supplyDropTransform;
            }
        }
    }

    private void ActivateObstacle()
    {
        NavMeshObstacle[] obstacles = GetComponentsInChildren<NavMeshObstacle>();
        foreach (NavMeshObstacle obstacle in obstacles)
        {
            obstacle.enabled = true;
        }
    }
}
