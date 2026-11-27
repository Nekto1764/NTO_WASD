using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public float resources = 100f;
    public float depletionRate = 1f;

    public bool IsDepleted => resources <= 0;

    public ResourceType resourceType;

    public void Harvest(float amount)
    {
        if (!IsDepleted)
        {
            resources -= amount;
            if (resources < 0)
            {
                resources = 0;
            }
        }

        if (IsDepleted)
        {
            
            Destroy(gameObject);
        }
    }
}
