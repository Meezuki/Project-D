using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        List<CombatantView> targets = new()
        {
            HeroSystem.Instance.HeroView
        };
        return targets;
    }
}
