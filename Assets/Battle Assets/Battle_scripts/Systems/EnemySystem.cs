using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;

    // temporary
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA> ();
    }

    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
        {
            enemyBoardView.AddEnemy(enemyData);
        }
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        //Debug.Log("Enemy Turn");
        //yield return new WaitForSeconds(2f);
        //Debug.Log("End Enemy Turn");

        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            int burnStacks = enemy.GetStatusEffectStacks(StatusEffectType.BURN);
            if(burnStacks > 0)
            {
                ApplyBurnGA applyBurnGA = new(burnStacks, enemy);
                ActionSystem.Instance.AddReaction(applyBurnGA);
            }
            AttackHeroGA attackHeroGA = new(enemy);
            ActionSystem.Instance.AddReaction(attackHeroGA);

        }
        yield return null;
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {

        EnemyView attacker = attackHeroGA.Attacker;

        // Make sure the attacker is still alive before moving
        if (attacker == null) yield break;


        // Play animation: for now just move forward and back
        Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1f, 0.15f);
        yield return tween.WaitForCompletion();
        attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.25f);

        // Deal Damage
        DealDamageGA dealDamageGA = new(attacker.AttackPower, new() { HeroSystem.Instance.HeroView }, attackHeroGA.Caster);
        ActionSystem.Instance.AddReaction(dealDamageGA);
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return enemyBoardView.RemoveEnemy(killEnemyGA.EnemyView);


        // Check if there are no more enemies

        if (Enemies.Count == 0)
        {
            // Add the Win reaction to the queue
            WinMatchGA winMatchGA = new();
            ActionSystem.Instance.AddReaction(winMatchGA);
        }
    }
}