using SerializeReferenceEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Perk")]
public class PerkData : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField, TextArea(3, 5)] public string Description { get; private set; }
    [field: SerializeReference, SR] public PerkCondition PerkCondition { get; private set; }
    [field: SerializeReference, SR] public AutoTargetEffect AutoTargetEffect { get; private set; }

    [field: SerializeField] public bool UseAutoTarget { get; private set; } = true;

    // use the caster as the target (for example: buff)
    [field: SerializeField] public bool UseActionCasterAsTarget { get; private set; } = false;

}
