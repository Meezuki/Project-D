using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllEnemiesTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        return new(EnemySystem.Instance.Enemies);
    }
}
