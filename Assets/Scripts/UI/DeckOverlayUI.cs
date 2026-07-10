using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class DeckOverlayUI : MonoBehaviour
{
    public static DeckOverlayUI Instance;

    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text titleText;

    private bool isRemoveMode;
    private Action<CardData> onCardSelected;

    private void Awake()
    {
        Instance = this;
        if (overlayPanel != null) overlayPanel.SetActive(false);
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }
    }

    public void Show(bool removeMode = false, Action<CardData> onCardSelectedCallback = null)
    {
        isRemoveMode = removeMode;
        onCardSelected = onCardSelectedCallback;

        if (titleText != null)
        {
            titleText.text = isRemoveMode ? "Select Card to Remove" : "Your Deck";
        }

        if (overlayPanel != null)
        {
            overlayPanel.SetActive(true);
        }

        PopulateDeck();
    }

    public void Hide()
    {
        if (overlayPanel != null)
        {
            overlayPanel.SetActive(false);
        }
    }

    private void PopulateDeck()
    {
        // Clear old UI objects
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        List<CardData> deck = null;
        if (RunManager.Instance != null && RunManager.Instance.CurrentDeck.Count > 0)
        {
            deck = RunManager.Instance.CurrentDeck;
        }
        else
        {
            // Fallback for editor direct play tests
            var setup = FindFirstObjectByType<MatchSetupSystem>();
            if (setup != null && setup.HeroData != null)
            {
                deck = setup.HeroData.Deck;
            }
        }

        if (deck == null || deck.Count == 0)
        {
            Debug.LogWarning("DeckOverlayUI: Deck is empty or could not be loaded!");
            return;
        }

        foreach (var cardData in deck)
        {
            if (cardData == null) continue;
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            UICard uiCard = cardObj.GetComponent<UICard>();
            if (uiCard != null)
            {
                uiCard.Setup(cardData, () => OnCardClicked(cardData));
            }
        }
    }

    private void OnCardClicked(CardData cardData)
    {
        if (isRemoveMode)
        {
            onCardSelected?.Invoke(cardData);
            Hide();
        }
    }
}
