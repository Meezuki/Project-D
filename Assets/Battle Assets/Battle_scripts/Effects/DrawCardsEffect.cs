using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawCardsEffect : Effect

{

    [SerializeField] private int drawAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DrawCardsGA drawCardsGA = new(drawAmount);
        return drawCardsGA;

    }
}
