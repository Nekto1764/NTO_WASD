using System.Collections.Generic;
using UnityEngine;

public class FindTrees : MonoBehaviour
{
    public float radius = 5f;
    public string targetTag = "Tree";

    // private void Update()
    // {
    //     GetObjectsInRadius();
    // }

    public List<GameObject> GetObjectsInRadius()
    {
        List<GameObject> foundObjects = new List<GameObject>();

        // Находим все коллайдеры в заданном радиусе
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag))
            {
                foundObjects.Add(hitCollider.gameObject);
            }
        }

        Debug.Log($"[{foundObjects.Count}]");

        return foundObjects;
    }
}