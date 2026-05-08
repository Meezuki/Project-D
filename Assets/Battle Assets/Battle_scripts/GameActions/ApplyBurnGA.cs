using UnityEngine;

public class ApplyBurnGA : GameAction
{
    public int BurnDamage { get; private set; }

    public CombatantView Target { get; private set; }

    public ApplyBurnGA(int burnDamage, CombatantView target)
    {
        BurnDamage = burnDamage;
        Target = target;
    }

}
