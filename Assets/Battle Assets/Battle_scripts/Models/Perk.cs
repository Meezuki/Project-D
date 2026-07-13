using System.Collections.Generic;
using UnityEngine;

public class Perk
{
    public Sprite Image => data.Image;
    public string Description => data != null ? data.Description : string.Empty;
    public PerkData Data => data;
    private readonly PerkData data;
    private readonly PerkCondition condition;
    public readonly AutoTargetEffect effect;

    public Perk(PerkData perkData)
    {
        data = perkData;
        condition = data.PerkCondition;
        effect = data.AutoTargetEffect;
    }

    public void OnAdd()
    {
        if (condition != null)
        {
            condition.SubscribeCondition(Reaction);
        }
    }

    public void OnRemove()
    {
        if (condition != null)
        {
            condition.UnsubscribeCondition(Reaction);
        }
    }

    private void Reaction(GameAction gameAction)
    {
        if (condition.SubConditionIsMet(gameAction))
        {
            List<CombatantView> targets = new();
            if (data.UseActionCasterAsTarget && gameAction is IHaveCaster haveCaster)
            {
                targets.Add(haveCaster.Caster);
            }
            if (data.UseAutoTarget)
            {
                targets.AddRange(effect.TargetMode.GetTargets());
            }
            // perk caster is always hero
            GameAction perkEffectAction = effect.Effect.GetGameAction(targets,HeroSystem.Instance.HeroView);
            ActionSystem.Instance.AddReaction(perkEffectAction);
        }
    }
}
