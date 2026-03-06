using UnityEngine;
using System.Collections.Generic;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> deckData;

    private void Start()
    {
        CardSystem.Instance.Setup(deckData);
    }
}

//old
    // public class TestSystem : MonoBehaviour
    // {
    //     [SerializeField] private HandView handView;
    //     [SerializeField] private CardData cardData;


    // }
