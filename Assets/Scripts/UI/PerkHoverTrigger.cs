using UnityEngine;
using UnityEngine.EventSystems;

public class PerkHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PerkData perkData;

    public void SetPerk(PerkData data)
    {
        perkData = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (perkData != null && TooltipSystem.Instance != null)
        {
            string description = string.IsNullOrEmpty(perkData.Description) ? "No description available." : perkData.Description;
            TooltipSystem.Instance.Show(description, perkData.name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipSystem.Instance != null)
        {
            TooltipSystem.Instance.Hide();
        }
    }

    private void OnDisable()
    {
        if (TooltipSystem.Instance != null)
        {
            TooltipSystem.Instance.Hide();
        }
    }
}
