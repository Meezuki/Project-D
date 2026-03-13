using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card Card { get; set; } 
    public PlayCardGA(Card card)
    {
       Card = card;
    }
}
