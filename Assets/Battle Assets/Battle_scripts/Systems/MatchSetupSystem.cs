using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    //[SerializeField] private List<CardData> deckData;
    [SerializeField] private HeroData heroData;
    [SerializeField] private List<EnemyData> enemyDatas;

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
            EnemySystem.Instance.Setup(enemyDatas);
            CardSystem.Instance.Setup(heroData.Deck);
            DrawCardsGA drawCardsGA = new(5);
            ActionSystem.Instance.Perform(drawCardsGA);



        }
}
