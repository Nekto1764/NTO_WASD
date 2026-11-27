using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera facingCamera;

    private Transform self;

    private void Start()
    {
        facingCamera = Camera.main;
        self = transform;
    }

    private void Update()
    {
        if (facingCamera == null) return;

        self.LookAt(2f * self.position - facingCamera.transform.position);
    }
}
