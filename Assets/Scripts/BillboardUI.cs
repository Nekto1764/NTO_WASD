using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null) return;
        transform.forward = cameraTransform.forward;
    }
}

