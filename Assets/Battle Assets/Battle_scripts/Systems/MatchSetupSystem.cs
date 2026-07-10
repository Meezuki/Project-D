using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    //[SerializeField] private List<CardData> deckData;
    [SerializeField] private HeroData heroData;
    public HeroData HeroData => heroData;
    [SerializeField] private List<EnemyData> enemyDatas;

    [SerializeField] private PerkData perkData;

        private void Start()
        {
        //CardSystem.Instance.Setup(deckData);

        //RefillManaGA refillManaGA = new();
        //ActionSystem.Instance.Perform(refillManaGA, () =>
        //{
        //    DrawCardsGA drawCardsGA = new(5);
        //    ActionSystem.Instance.Perform(drawCardsGA);
        //});
        //DrawCardsGA drawCardsGA = new(5);
        //ActionSystem.Instance.Perform(drawCardsGA);


        // --------------------NEW---------------
        // sekarang ambil deck dari herodata bukan secara manual dari deckData
            HeroSystem.Instance.Setup(heroData);

            if (RunManager.Instance != null && RunManager.Instance.NextEncounterEnemies.Count > 0)
            {
                Debug.Log("Menggunakan Enemies dari RunManager!");
                EnemySystem.Instance.Setup(RunManager.Instance.NextEncounterEnemies);
            }
            else
            {
                Debug.LogWarning("RunManager tidak ditemukan atau NextEncounterEnemies kosong! Menggunakan default enemies.");
                EnemySystem.Instance.Setup(enemyDatas);
            }

            // Setup perks from RunManager if available, otherwise use inspector fallback
            if (RunManager.Instance != null && RunManager.Instance.ActivePerks.Count > 0)
            {
                foreach (var perkSO in RunManager.Instance.ActivePerks)
                {
                    PerkSystem.Instance.AddPerk(new Perk(perkSO));
                }
            }
            else if (perkData != null)
            {
                PerkSystem.Instance.AddPerk(new Perk(perkData));
            }


            // OLD: sekarang kita gunakan deck data dari run manager
            // CardSystem.Instance.Setup(heroData.Deck); 


            // --- NEW GLOBAL DECK LOGIC ---
            // Cek apakah RunManager ada dan deck-nya tidak kosong (artinya kita datang dari MapScene)
            if (RunManager.Instance != null && RunManager.Instance.CurrentDeck.Count > 0)
            {
                Debug.Log("Menggunakan Deck dari RunManager!");
                CardSystem.Instance.Setup(RunManager.Instance.CurrentDeck);
            }
            else
            {
                // FALLBACK: Jika kita langsung tekan Play di BattleTestScene, gunakan deck default dari HeroData
                Debug.LogWarning("RunManager tidak ditemukan! Menggunakan default deck dari HeroData untuk testing.");
                CardSystem.Instance.Setup(heroData.Deck);
            }


        RefillManaGA refillManaGA = new();
            ActionSystem.Instance.Perform(refillManaGA, () =>
            {
                DrawCardsGA drawCardsGA = new(5);
                ActionSystem.Instance.Perform(drawCardsGA);
            });

            //DrawCardsGA drawCardsGA = new(5);
            //ActionSystem.Instance.Perform(drawCardsGA);



        }
}
