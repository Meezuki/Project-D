using System.Collections;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;

    public int MaxMana
    {
        get
        {
            int baseMax = 3;
            if (PerkSystem.Instance != null && PerkSystem.Instance.HasPerk("Blessed"))
            {
                baseMax += 1;
            }
            return baseMax;
        }
    }

    private int currentMana = 3;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);

    }


    //check enough mana
    public bool HasEnoughMana(int mana)
    {
        return currentMana >= mana;
    }


    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        currentMana -= spendManaGA.Amount;
        manaUI.UpdateManaText(currentMana);
        yield return null;
    }

    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        currentMana = MaxMana;
        manaUI.UpdateManaText(currentMana);
        yield return null;
    }

    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        RefillManaGA refillManaGA = new RefillManaGA();
        ActionSystem.Instance.AddReaction(refillManaGA);
    }
}
