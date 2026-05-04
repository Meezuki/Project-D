using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> deckData;

        private void Start()
        {
            CardSystem.Instance.Setup(deckData);

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
