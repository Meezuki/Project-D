using System.Collections.Generic;
using UnityEngine;
using Map;

[CreateAssetMenu(menuName = "Data/EncounterPool")]
public class EncounterPoolSO : ScriptableObject
{
    [SerializeField] private List<EnemyEncounter> minorEnemyPool;
    [SerializeField] private List<EnemyEncounter> eliteEnemyPool;
    [SerializeField] private List<EnemyEncounter> bossEnemyPool;

    public List<EnemyData> GetRandomEncounter(NodeType nodeType)
    {
        List<EnemyEncounter> pool = null;

        switch (nodeType)
        {
            case NodeType.MinorEnemy:
                pool = minorEnemyPool;
                break;
            case NodeType.EliteEnemy:
                pool = eliteEnemyPool;
                break;
            case NodeType.Boss:
                pool = bossEnemyPool;
                break;
        }

        if (pool == null || pool.Count == 0)
        {
            return new List<EnemyData>();
        }

        int index = Random.Range(0, pool.Count);
        EnemyEncounter encounter = pool[index];
        if (encounter != null && encounter.Enemies != null)
        {
            return new List<EnemyData>(encounter.Enemies);
        }

        return new List<EnemyData>();
    }
}
