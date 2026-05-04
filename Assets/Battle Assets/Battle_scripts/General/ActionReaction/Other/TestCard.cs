//code is currently disabled because errors in the tutorial video that I can't find the fix for. Ep. 5's offlink video jump around without showing the full script
// using UnityEngine;

// public class CardReaction : MonoBehaviour //in the video CardReaction was called Card, but that caused erorrs because of the existing Card class, so I renamed it to CardReaction. will change back to Card when the existing Card class is removed.
// {
//     void OnMouseDown()
//     {
//         if (ActionSystem.Instance.IsPerforming) return;
//         DrawCardGA drawCardGA = new();
//         ActionSystem.Instance.Perform(drawCardGA);
//         Destroy(gameObject);
//     }
// }