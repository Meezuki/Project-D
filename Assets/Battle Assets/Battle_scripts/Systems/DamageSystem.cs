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
        foreach (var targets in dealDamageGA.Targets)
        {
            targets.Damage(dealDamageGA.Amount);
            Instantiate(damageVFX, targets.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
