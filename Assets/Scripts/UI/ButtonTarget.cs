using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private BuySlot buySlot;

    public TextMeshProUGUI info;

    public ObjectsDatabaseSO database;

    private void Awake()
    {
        button = GetComponent<Button>();
        buySlot = GetComponent<BuySlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButtonHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnButtonExit();
    }

    private void OnButtonHover()
    {
        string infoText = "";

        foreach (BuildRequirement req in database.objectsData[buySlot.databaseItemID].resourceRequirements){
            infoText += $"[{req.resource}] {req.amount}" + "\n";
        }

        info.text = infoText;
    }

    private void OnButtonExit()
    {
        info.text = "";
    }
}