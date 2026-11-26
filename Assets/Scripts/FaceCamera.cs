using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera facingCamera;

    private Transform self;

    private void Start()
    {
        self = transform;
    }

    private void Update()
    {
        if (facingCamera == null) return;

        self.LookAt(2f * self.position - facingCamera.transform.position);
    }
}
