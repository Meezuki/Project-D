//code is currently disabled because errors in the tutorial video that I can't find the fix for. Ep. 5's offlink video jump around without showing the full script

// using DG.Tweening;
// using UnityEngine;
// using System.Collections;


// public class CardSystem : MonoBehaviour
// {
//     [SerializeField] private Card cardPrefab;
//     [SerializeField] private Transform spawn;
//     [SerializeField] private Transform hand;

//     private void OnEnable()
//     {
//         ActionSystem.AttachPerformer<DrawCardGA>(DrawCardPerformer);
//     }

//     private void OnDisable()
//     {
//         ActionSystem.DetachPerformer<DrawCardGA>();
//     }

//     private IEnumerator DrawCardPerformer(DrawCardGA drawCardGA)
//     {
//         CardReaction card = Instantiate(cardPrefab, spawn.position, Quaternion.identity);
//         Tween tween = card.transform.DOMove(hand.position, 0.5f);
//         yield return tween.WaitForCompletion();
//     }
// }