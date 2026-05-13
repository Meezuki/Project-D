using System.Collections;
using UnityEngine;

public class MatchManagerSystem : MonoBehaviour
{
    // If you have a Victory UI Panel, you can reference it here
    // [SerializeField] private GameObject victoryPanel;

    private void OnEnable()
    {
        // 1. Attach the performer to the ActionSystem
        ActionSystem.AttachPerformer<WinMatchGA>(WinMatchPerformer);
    }

    private void OnDisable()
    {
        // 2. Detach it when disabled
        ActionSystem.DetachPerformer<WinMatchGA>();
    }

    // 3. The actual logic that runs when the game is won
    private IEnumerator WinMatchPerformer(WinMatchGA winMatchGA)
    {
        Debug.Log("VICTORY! All enemies have been defeated!");

        // Step A: Stop the player from interacting with cards
        // (Assuming you have a variable in Interactions.cs to lock the game)
        // Interactions.Instance.IsGameOver = true; 

        // Step B: Wait for a second so the player can process the last kill
        yield return new WaitForSeconds(1f);

        // Step C: Show the Victory Screen UI
        // if (victoryPanel != null) victoryPanel.SetActive(true);

        yield return null;
    }
}
