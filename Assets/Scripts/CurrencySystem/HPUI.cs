using UnityEngine;
using TMPro;

public class HPUI : MonoBehaviour
{
    public TextMeshProUGUI hpText;

    private void Start()
    {
        UpdateHPUI();
    }

    private void OnEnable()
    {
        UpdateHPUI();
    }

    public void UpdateHPUI()
    {
        if (RunManager.Instance != null)
        {
            if (RunManager.Instance.HeroMaxHP > 0)
            {
                hpText.text = RunManager.Instance.HeroCurrentHP + "/" + RunManager.Instance.HeroMaxHP;
            }
            else
            {
                hpText.text = "44/44";
            }
        }
        else
        {
            hpText.text = "44/44";
        }
    }
}
