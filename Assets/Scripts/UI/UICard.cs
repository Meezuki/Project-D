using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UICard : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private Image cardImage;
    [SerializeField] private Button clickButton;

    public CardData CardData { get; private set; }

    public void Setup(CardData cardData, Action onClickAction = null)
    {
        CardData = cardData;
        
        if (titleText != null) titleText.text = cardData.name;
        if (descriptionText != null) descriptionText.text = cardData.Description;
        if (manaText != null) manaText.text = cardData.Mana.ToString();
        if (cardImage != null) cardImage.sprite = cardData.Image;

        if (clickButton != null)
        {
            clickButton.onClick.RemoveAllListeners();
            if (onClickAction != null)
            {
                clickButton.onClick.AddListener(() => onClickAction());
            }
        }
    }
}
