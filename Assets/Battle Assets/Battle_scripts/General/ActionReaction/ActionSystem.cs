using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSystem : Singleton<ActionSystem>
{
    private Stack<List<GameAction>> reactionStack = new();
    public bool IsPerforming { get; private set; } = false;
    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();
    private static Dictionary<Delegate, Action<GameAction>> wrappers = new();

    private void OnDestroy()
    {
        preSubs.Clear();
        postSubs.Clear();
        performers.Clear();
        wrappers.Clear();
    }
    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
    if (IsPerforming) return;
    IsPerforming = true;
    StartCoroutine(Flow(action, () =>
    {
        IsPerforming = false;
        OnPerformFinished?.Invoke();
    }));

    }
    public void AddReaction(GameAction gameAction)
    {
        if (reactionStack.Count > 0)
        {
            reactionStack.Peek().Add(gameAction);
        }
        else
        {
            Debug.LogWarning("No active reaction phase to add reaction to!");
        }
    }
    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
    reactionStack.Push(action.PreReactions);
    PerformSubscribers(action, preSubs);
    yield return PerformReactions(action.PreReactions);
    reactionStack.Pop();

    reactionStack.Push(action.PerformReactions);
    yield return PerformPerformer(action);
    yield return PerformReactions(action.PerformReactions);
    reactionStack.Pop();

    reactionStack.Push(action.PostReactions);
    PerformSubscribers(action, postSubs);
    yield return PerformReactions(action.PostReactions);
    reactionStack.Pop();

    OnFlowFinished?.Invoke();
    }
    private IEnumerator PerformPerformer(GameAction action)
    {
    Type type = action.GetType();
    if (performers.ContainsKey(type))
    {
        yield return performers[type](action);
    }
    }
    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
            {
                sub(action);
            }
        }
    }
    private IEnumerator PerformReactions(List<GameAction> phaseReactions)
    {
       for (int i = 0; i < phaseReactions.Count; i++)
       {
        yield return Flow(phaseReactions[i]);
       }
    }
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (performers.ContainsKey(type)) performers[type] = wrappedPerformer;
        else performers.Add(type, wrappedPerformer);
    }

    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type)) performers.Remove(type);
    }

    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        Action<GameAction> wrappedReaction = (action) => reaction((T)action);
        wrappers[reaction] = wrappedReaction;
        
        if (subs.ContainsKey(typeof(T)))
        {
            subs[typeof(T)].Add(wrappedReaction);
        }
        else
        {
            subs.Add(typeof(T), new());
            subs[typeof(T)].Add(wrappedReaction);
        }
    }

    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        if (wrappers.TryGetValue(reaction, out Action<GameAction> wrappedReaction))
        {
            if (subs.ContainsKey(typeof(T)))
            {
                subs[typeof(T)].Remove(wrappedReaction);
            }
            wrappers.Remove(reaction);
        }
    }
}