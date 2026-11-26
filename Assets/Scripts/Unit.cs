using UnityEngine;

public class Unit : MonoBehaviour
{
    public float unitMaxHealth;
    public HealthTracker healthTracker;

    public bool IsDead { get; private set; }

    private float currentHealth;

    private void Start()
    {
        Register();
        currentHealth = unitMaxHealth;
        RefreshHealthUI();
    }

    private void OnDestroy()
    {
        Unregister();
    }

    internal void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        RefreshHealthUI();
    }

    private void Register()
    {
        if (UnitSelectionManager.Instance != null)
        {
            UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
        }
    }

    private void Unregister()
    {
        if (UnitSelectionManager.Instance != null)
        {
            UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
        }
    }

    private void RefreshHealthUI()
    {
        if (healthTracker != null)
        {
            healthTracker.UpdateSliderValue(currentHealth, unitMaxHealth);
        }

        if (!IsDead && currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        Unregister();
        Destroy(gameObject);
    }
}
