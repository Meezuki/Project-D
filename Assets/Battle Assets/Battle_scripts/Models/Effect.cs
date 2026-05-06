using System.Collections.Generic;

[System.Serializable] //required to be editable in editor
public abstract class Effect
{
    public abstract GameAction GetGameAction(List<CombatantView> targets);
}
