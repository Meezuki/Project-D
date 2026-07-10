using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        return null;
    }
}
