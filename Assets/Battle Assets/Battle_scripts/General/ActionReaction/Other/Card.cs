using UnityEngine;

public class Card : MonoBehaviour
{
    void OnMouseDown()
    {
        if (ActionSystem.Instance.IsPerforming) return;
        DrawCardGA drawCardGA = new();
        ActionSystem.Instance.Perform(drawCardGA);
        Destroy(gameObject);
    }
}