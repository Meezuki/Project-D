using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    public List<GameAction> PreReactions { get; private set; } = new(); //do action before
    public List<GameAction> PerformReactions { get; private set; } = new(); //do action now
    public List<GameAction> PostReactions { get; private set; } = new(); //do action after
}