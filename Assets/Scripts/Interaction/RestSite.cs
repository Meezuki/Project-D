using UnityEngine;

public class RestSite : MonoBehaviour, Interactable
{
    [SerializeField] private string _prompt = "Rest";
    [SerializeField] private float _healthRefillPercentage = 0.20f; // 20%
    
    private bool _isUsed = false;

    public string InteractablePrompt => _isUsed ? "Already Rested" : _prompt;

    public bool Interact(Interactor interactor)
    {
        if (_isUsed) return false;

        _isUsed = true;
        
        if (RunManager.Instance != null)
        {
            // Refill 20% of max health
            int maxHp = RunManager.Instance.HeroMaxHP;
            if (maxHp <= 0)
            {
                // Fallback if not initialized (should not happen in actual run, but good for testing)
                maxHp = 44; 
            }
            int refillAmount = Mathf.RoundToInt(maxHp * _healthRefillPercentage);
            
            // Add health, clamp to maxHp
            RunManager.Instance.HeroCurrentHP = Mathf.Min(maxHp, RunManager.Instance.HeroCurrentHP + refillAmount);
            
            Debug.Log($"Rested! Refilled {refillAmount} HP (20% of {maxHp}). Current HP: {RunManager.Instance.HeroCurrentHP}/{maxHp}");
        }
        else
        {
            Debug.LogWarning("RestSite: RunManager.Instance is null! Cannot refill health.");
        }

        return true;
    }
}
