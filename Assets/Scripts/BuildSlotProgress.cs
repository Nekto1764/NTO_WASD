using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSlotProgress : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    private float buildTime;
    private float startTime;
    private bool isBuilding;

    public void StartBuilding(float time)
    {
        buildTime = time;
        startTime = Time.time;
        isBuilding = true;
        progressImage.fillAmount = 1f;
        progressImage.enabled = true;
    }

    private void Update()
    {
        if (!isBuilding) return;

        float elapsed = Time.time - startTime;
        float progress = elapsed / buildTime;
        progressImage.fillAmount = Mathf.Clamp01(1f - progress);

        if (elapsed >= buildTime)
        {
            isBuilding = false;
            progressImage.fillAmount = 0f;
            progressImage.enabled = false;
            OnBuildComplete();
        }
    }

    private void OnBuildComplete()
    {
        
    }
}
