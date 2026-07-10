using System.Collections;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject damageVFX;


    void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
    }
    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        if (dealDamageGA.Caster != null)
        {
            dealDamageGA.Caster.PlayAttackAnimation();
        }

        int strengthBonus = 0;
        if (dealDamageGA.IsCardAction && dealDamageGA.Caster != null)
        {
            strengthBonus = dealDamageGA.Caster.GetStatusEffectStacks(StatusEffectType.STRENGTH);
        }

        foreach (var target in dealDamageGA.Targets)
        {

            if (target == null) continue;
            target.Damage(dealDamageGA.Amount + strengthBonus);
            Instantiate(damageVFX, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.15f);

            if (target.CurrentHealth <= 0)
            {
                if (target is EnemyView enemyView)
                {
                    KillEnemyGA killEnemyGA = new(enemyView);
                    ActionSystem.Instance.AddReaction(killEnemyGA);
                }
                else
                {
                    DefeatGA defeatGA = new();
                    ActionSystem.Instance.AddReaction(defeatGA);
                }
            }
        }
    }
}
