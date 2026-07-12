using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Map;

public class ShopOverlayUI : MonoBehaviour
{
    public static ShopOverlayUI Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button returnButton;

    [Header("Card Shop")]
    [SerializeField] private GameObject cardUiPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private List<CardData> allCardsPool = new();

    [Header("Perk Shop")]
    [SerializeField] private List<GameObject> perkSlotContainers = new(); // 3 slots
    [SerializeField] private List<Image> perkSlotImages = new();
    [SerializeField] private List<TMP_Text> perkSlotTitles = new();
    [SerializeField] private List<TMP_Text> perkSlotPrices = new();
    [SerializeField] private List<Button> perkSlotButtons = new();
    [SerializeField] private List<PerkData> allPerksPool = new();

    [Header("Card Removal Service")]
    [SerializeField] private GameObject cardRemovalContainer;
    [SerializeField] private TMP_Text cardRemovalPriceText;
    [SerializeField] private Button cardRemovalButton;

    private List<CardData> shopCards = new();
    private List<PerkData> shopPerks = new();

    private const int CARD_COST = 50;
    private const int PERK_COST = 100;
    private const int REMOVAL_COST = 75;

    private void Awake()
    {
        Instance = this;
        if (shopPanel != null) shopPanel.SetActive(true);
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(CloseShop);
        }
        if (cardRemovalButton != null)
        {
            cardRemovalButton.onClick.AddListener(TryRemoveCard);
        }
    }

    private void Start()
    {
        GenerateShopItems();
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
        GenerateShopItems();
    }

    public void CloseShop()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
    }

    private void GenerateShopItems()
    {
        // 1. Generate 3 random cards
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
        shopCards.Clear();

        if (allCardsPool.Count > 0)
        {
            List<CardData> pool = new(allCardsPool);
            for (int i = 0; i < 3 && pool.Count > 0; i++)
            {
                int index = Random.Range(0, pool.Count);
                CardData selected = pool[index];
                pool.RemoveAt(index);
                shopCards.Add(selected);

                // Instantiate card
                GameObject cardObj = Instantiate(cardUiPrefab, cardContainer);
                
                // Add a text element for price inside the instantiated card UI
                // We'll look for a price text or add it
                TMP_Text[] texts = cardObj.GetComponentsInChildren<TMP_Text>();
                foreach (var txt in texts)
                {
                    if (txt.name == "ManaText") // Change mana display or just add price text
                    {
                        // We will set price text below
                    }
                }
                
                // Create a sub-price display or set title
                UICard uiCard = cardObj.GetComponent<UICard>();
                if (uiCard != null)
                {
                    CardData cardRef = selected;
                    GameObject slotObj = cardObj;
                    uiCard.Setup(cardRef, () => BuyCard(cardRef, slotObj));
                }

                // Add price tag text
                var priceTextObj = new GameObject("PriceText");
                priceTextObj.transform.SetParent(cardObj.transform, false);
                var rect = priceTextObj.AddComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, -110f); // Position below card
                var priceTxt = priceTextObj.AddComponent<TextMeshProUGUI>();
                priceTxt.text = CARD_COST + " G";
                priceTxt.fontSize = 20;
                priceTxt.alignment = TextAlignmentOptions.Center;
                priceTxt.color = Color.yellow;
            }
        }

        // 2. Generate 3 random perks
        shopPerks.Clear();
        List<PerkData> availablePerks = new();
        foreach (var perk in allPerksPool)
        {
            if (perk == null) continue;
            // Exclude perks player already has in RunManager
            bool alreadyHas = false;
            if (RunManager.Instance != null)
            {
                alreadyHas = RunManager.Instance.ActivePerks.Exists(p => p.name == perk.name);
            }
            if (!alreadyHas)
            {
                availablePerks.Add(perk);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (i < availablePerks.Count && perkSlotContainers.Count > i)
            {
                int index = Random.Range(0, availablePerks.Count);
                PerkData selected = availablePerks[index];
                availablePerks.RemoveAt(index);
                shopPerks.Add(selected);

                perkSlotContainers[i].SetActive(true);
                if (perkSlotImages[i] != null) perkSlotImages[i].sprite = selected.Image;
                if (perkSlotTitles[i] != null) perkSlotTitles[i].text = selected.name;
                if (perkSlotPrices[i] != null) perkSlotPrices[i].text = PERK_COST + " G";
                
                // Add hover triggers for tooltip
                AddPerkHoverTrigger(perkSlotContainers[i], selected);
                if (perkSlotImages[i] != null) AddPerkHoverTrigger(perkSlotImages[i].gameObject, selected);
                if (perkSlotButtons[i] != null) AddPerkHoverTrigger(perkSlotButtons[i].gameObject, selected);

                perkSlotButtons[i].onClick.RemoveAllListeners();
                int slotIndex = i;
                PerkData perkRef = selected;
                perkSlotButtons[i].onClick.AddListener(() => BuyPerk(perkRef, slotIndex));
                perkSlotButtons[i].interactable = true;
            }
            else if (perkSlotContainers.Count > i)
            {
                perkSlotContainers[i].SetActive(false);
            }
        }

        // 3. Setup Card Removal Service
        if (cardRemovalContainer != null)
        {
            cardRemovalContainer.SetActive(true);
            if (cardRemovalPriceText != null) cardRemovalPriceText.text = REMOVAL_COST + " G";
            if (cardRemovalButton != null) cardRemovalButton.interactable = true;
        }
    }

    private void BuyCard(CardData card, GameObject cardSlotObj)
    {
        if (CurrencyManager.Instance == null) return;

        if (CurrencyManager.Instance.getGold() >= CARD_COST)
        {
            CurrencyManager.Instance.spendGold(CARD_COST);
            if (RunManager.Instance != null)
            {
                RunManager.Instance.AddCardToDeck(card);
            }
            // Disable slot
            var btn = cardSlotObj.GetComponentInChildren<Button>();
            if (btn != null) btn.interactable = false;
            
            // Disable all buttons in this card slot
            foreach (var b in cardSlotObj.GetComponentsInChildren<Button>())
            {
                b.interactable = false;
            }
            
            // Grey out card image
            var img = cardSlotObj.GetComponentInChildren<Image>();
            if (img != null) img.color = Color.gray;
        }
        else
        {
            Debug.Log("Not enough gold to buy card!");
        }
    }

    private void BuyPerk(PerkData perk, int slotIndex)
    {
        if (CurrencyManager.Instance == null) return;

        if (CurrencyManager.Instance.getGold() >= PERK_COST)
        {
            CurrencyManager.Instance.spendGold(PERK_COST);
            if (RunManager.Instance != null)
            {
                RunManager.Instance.ActivePerks.Add(perk);
                Debug.Log($"Bought perk {perk.name}!");
            }
            perkSlotButtons[slotIndex].interactable = false;
            if (perkSlotImages[slotIndex] != null) perkSlotImages[slotIndex].color = Color.gray;
        }
        else
        {
            Debug.Log("Not enough gold to buy perk!");
        }
    }

    private void TryRemoveCard()
    {
        if (CurrencyManager.Instance == null) return;

        if (CurrencyManager.Instance.getGold() >= REMOVAL_COST)
        {
            if (DeckOverlayUI.Instance != null)
            {
                DeckOverlayUI.Instance.Show(removeMode: true, onCardSelectedCallback: RemoveCard);
            }
        }
        else
        {
            Debug.Log("Not enough gold for card removal!");
        }
    }

    private void RemoveCard(CardData card)
    {
        if (CurrencyManager.Instance == null || RunManager.Instance == null) return;

        if (CurrencyManager.Instance.spendGold(REMOVAL_COST))
        {
            RunManager.Instance.CurrentDeck.Remove(card);
            Debug.Log($"Removed card {card.name} from run deck!");

            // Disable removal service after single use
            if (cardRemovalButton != null)
            {
                cardRemovalButton.interactable = false;
            }
            var removalImg = cardRemovalContainer.GetComponentInChildren<Image>();
            if (removalImg != null) removalImg.color = Color.gray;
        }
    }

    private void AddPerkHoverTrigger(GameObject obj, PerkData perkData)
    {
        PerkHoverTrigger trigger = obj.GetComponent<PerkHoverTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<PerkHoverTrigger>();
        }
        trigger.SetPerk(perkData);
    }
}
