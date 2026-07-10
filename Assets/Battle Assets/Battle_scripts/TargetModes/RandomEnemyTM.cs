using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomEnemyTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        CombatantView target = EnemySystem.Instance.Enemies[Random.Range(0, EnemySystem.Instance.Enemies.Count)];
        return new() { target }; //always return a list
    }
}
