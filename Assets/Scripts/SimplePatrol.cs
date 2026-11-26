using UnityEngine;

public class SimplePatrol : MonoBehaviour
{
    [SerializeField] private float switchDirectionTime = 5f;
    public float speed = 5f;

    private bool movingForward = true;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= switchDirectionTime)
        {
            movingForward = !movingForward;
            timer = 0f;
        }

        Vector3 direction = movingForward ? Vector3.forward : Vector3.back;
        transform.Translate(direction * (speed * Time.deltaTime));
    }
}
