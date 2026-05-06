using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardView : MonoBehaviour
{
    [SerializeField] private List<Transform> slots;
    public List<EnemyView> EnemyViews { get; private set; } = new();

    // mamke ienumerator if you want addenemy with animation
    public void AddEnemy(EnemyData enemyData)
    {
        Transform slot = slots[EnemyViews.Count];
        EnemyView enemyView = EnemyViewCreator.Instance.CreateEnemyView(enemyData, slot.position, slot.rotation);
        enemyView.transform.parent = slot;
        EnemyViews.Add(enemyView);

    }

    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        EnemyViews.Remove(enemyView);
        Tween tween = enemyView.transform.DOScale(Vector3.zero, 0.25f);
        yield return tween.WaitForCompletion();
        Destroy(enemyView.gameObject);
    }
}
