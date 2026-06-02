using UnityEngine.UI;
using UnityEngine;

public class PerkUI : MonoBehaviour
{
    [SerializeField]private Image image;
    public Perk Perk { get; private set; }
    public void Setup(Perk perk)
    {
        Perk = perk;
        image.sprite = perk.Image;
    }
}
