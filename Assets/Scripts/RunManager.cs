using System.Collections.Generic;
using UnityEngine;

// Menggunakan PersistentSingleton agar otomatis menjadi DontDestroyOnLoad
public class RunManager : PersistentSingleton<RunManager>
{
    [Header("Current Run Data")]
    // Deck yang akan dibawa pemain dan bisa bertambah/berkurang selama run
    public List<CardData> CurrentDeck = new();

    [Header("Encounter Config")]
    [SerializeField] private EncounterPoolSO encounterPool;
    [HideInInspector] public List<EnemyData> NextEncounterEnemies = new();

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
        Debug.Log("Run baru dimulai! Deck di-reset sesuai Hero default.");
    }

    // Fungsi untuk memanggil saat pemain mendapat kartu dari hadiah/shop
    public void AddCardToDeck(CardData newCard)
    {
        CurrentDeck.Add(newCard);
        Debug.Log($"Kartu {newCard.name} ditambahkan ke deck!");
    }
}