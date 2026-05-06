using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [SerializeField] private int damageAmount;

    public override GameAction GetGameAction()
    {
        // for now
        List<CombatantView> targets = new(EnemySystem.Instance.Enemies);
        DealDamageGA dealDamageGA = new(damageAmount, targets);
        return dealDamageGA;    
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
