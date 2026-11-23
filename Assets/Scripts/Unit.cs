using UnityEngine;

public class Unit : MonoBehaviour, IDamageable
{
    private float unitHealth;
    public float unitMaxHealth;

    public HealthTracker healthTracker;
    
    void Start()
    {
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);

        unitHealth = unitMaxHealth;
        UpdateHealthUI();
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }

    public void TakeDamage(int damageToInflict)
    {
        unitHealth -= damageToInflict;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);

        if (unitHealth <= 0)
        {
            //
            Destroy(gameObject);
        }
    }
}
