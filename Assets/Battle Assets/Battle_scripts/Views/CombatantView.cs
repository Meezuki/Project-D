using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int MaxHealth { get; private set; }
    public int CurrentHealth {  get; private set; }

    protected void SetupBase(int health, Sprite image)
    {
        MaxHealth = health;
        CurrentHealth = health; // fix
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "HP: " + CurrentHealth;
    }

}
