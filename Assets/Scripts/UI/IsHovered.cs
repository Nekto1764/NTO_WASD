using UnityEngine;
using UnityEngine.EventSystems;

public class IsHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHovered { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}