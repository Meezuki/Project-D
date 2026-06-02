using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text stackCountText;
    public void Set(Sprite sprite, int stackCount)
    {
        image.sprite = sprite;
        stackCountText.text = stackCount.ToString();
    }
}
