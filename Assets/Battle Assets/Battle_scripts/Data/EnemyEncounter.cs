using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyEncounter")]
public class EnemyEncounter : ScriptableObject
{
    [SerializeField] private List<EnemyData> enemies;

    public List<EnemyData> Enemies => enemies;
}
