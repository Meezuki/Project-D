using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [SerializeField] private int damageAmount;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DealDamageGA dealDamageGA = new(damageAmount, targets, caster);
        return dealDamageGA;    
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
