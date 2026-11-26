using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    private const float SmoothDuration = 0.5f;
    private const float GreenThreshold = 0.6f;
    private const float YellowThreshold = 0.3f;

    public Slider HealthBarSlider;
    public Image sliderFill;

    public Material greenEmission;
    public Material yellowEmission;
    public Material redEmission;

    private Coroutine smoothHealthChangeCoroutine;

    public void UpdateSliderValue(float currentHealth, float maxHealth)
    {
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        if (smoothHealthChangeCoroutine != null)
        {
            StopCoroutine(smoothHealthChangeCoroutine);
        }

        smoothHealthChangeCoroutine = StartCoroutine(
            SmoothHealthChange(HealthBarSlider.value, healthPercentage, SmoothDuration));

        UpdateColor(healthPercentage);
    }

    private IEnumerator SmoothHealthChange(float startValue, float targetValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            HealthBarSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HealthBarSlider.value = targetValue;
        smoothHealthChangeCoroutine = null;
    }

    private void UpdateColor(float healthPercentage)
    {
        if (healthPercentage >= GreenThreshold)
        {
            sliderFill.material = greenEmission;
        }
        else if (healthPercentage >= YellowThreshold)
        {
            sliderFill.material = yellowEmission;
        }
        else
        {
            sliderFill.material = redEmission;
        }
    }
}
