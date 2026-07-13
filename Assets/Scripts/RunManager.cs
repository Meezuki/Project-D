using System.Collections.Generic;
using UnityEngine;

// Menggunakan PersistentSingleton agar otomatis menjadi DontDestroyOnLoad
public class RunManager : PersistentSingleton<RunManager>
{
    [Header("Current Run Data")]
    // Deck yang akan dibawa pemain dan bisa bertambah/berkurang selama run
    public List<CardData> CurrentDeck = new();
    public int HeroCurrentHP;
    public int HeroMaxHP;
    public List<PerkData> ActivePerks = new();

    [Header("Encounter Config")]
    [SerializeField] private EncounterPoolSO encounterPool;
    [HideInInspector] public List<EnemyData> NextEncounterEnemies = new();

    private readonly List<CardData> defaultDeck = new();
    private int defaultMaxHP;
    private int defaultCurrentHP;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
        {
            defaultDeck.AddRange(CurrentDeck);
            defaultMaxHP = HeroMaxHP;
            defaultCurrentHP = HeroCurrentHP;
        }
    }

    public void ResetToDefault()
    {
        CurrentDeck.Clear();
        CurrentDeck.AddRange(defaultDeck);
        HeroMaxHP = defaultMaxHP;
        HeroCurrentHP = defaultCurrentHP;
        ActivePerks.Clear();
        NextEncounterEnemies.Clear();
        Debug.Log("RunManager: Reset to defaults.");
    }

    public void PrepareEncounter(Map.NodeType nodeType)
    {
        NextEncounterEnemies.Clear();
        if (encounterPool != null)
        {
            NextEncounterEnemies.AddRange(encounterPool.GetRandomEncounter(nodeType));
            Debug.Log($"Prepared encounter with {NextEncounterEnemies.Count} enemies for {nodeType}");
        }
        else
        {
            Debug.LogWarning("RunManager: encounterPool is null! Cannot prepare encounter.");
        }
    }

    // Panggil ini saat pemain menekan "New Game" di Main Menu
    public void StartNewRun(HeroData startingHero)
    {
        CurrentDeck.Clear();
        // Meng-copy daftar kartu dari HeroData ke CurrentDeck agar data asli aman
        CurrentDeck.AddRange(startingHero.Deck);
        HeroMaxHP = startingHero.Health;
        HeroCurrentHP = HeroMaxHP;
        ActivePerks.Clear();
        Debug.Log("Run baru dimulai! Deck & HP & Perks di-reset sesuai Hero default.");
    }

    // Fungsi untuk memanggil saat pemain mendapat kartu dari hadiah/shop
    public void AddCardToDeck(CardData newCard)
    {
        CurrentDeck.Add(newCard);
        Debug.Log($"Kartu {newCard.name} ditambahkan ke deck!");
    }
}