using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PerkUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    public Perk Perk { get; private set; }

    public void Setup(Perk perk)
    {
        Perk = perk;
        image.sprite = perk.Image;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Perk != null && Perk.Data != null && TooltipSystem.Instance != null)
        {
            string description = string.IsNullOrEmpty(Perk.Description) ? "No description available." : Perk.Description;
            TooltipSystem.Instance.Show(description, Perk.Data.name);
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
