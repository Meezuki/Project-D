using System;
using UnityEngine;

public class OnEnemyAttackCondition : PerkCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {

        // put conditiions here for example : if attacker is above x value

        return true;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AttackHeroGA>(reaction,reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AttackHeroGA>(reaction, reactionTiming);
    }
}
