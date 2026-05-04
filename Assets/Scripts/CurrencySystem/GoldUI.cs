using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{

    public TextMeshProUGUI goldText;
    private void OnEnable()
    {
        CurrencyManager.OnGoldChanged += UpdateGoldUI;

    }

    private void OnDisable()
    {
        // Lepaskan jika objek ini mati/hancur agar tidak error
        CurrencyManager.OnGoldChanged -= UpdateGoldUI;
    }


    void Start()
    {
        // update teks saat pertama kali dibuka
        if(CurrencyManager.Instance != null)
        {
            UpdateGoldUI(CurrencyManager.Instance.getGold());
        }
    }
    
    void UpdateGoldUI(int currentGold)
    {
        goldText.text = currentGold.ToString();
        // popup kecil animasi jika mau

    }
}
