using UnityEngine;

public class HeroView : CombatantView
{
    public void Setup(HeroData heroData)
    {
        int maxHp = heroData.Health;
        int currentHp = maxHp;

        if (RunManager.Instance != null)
        {
            if (RunManager.Instance.HeroMaxHP > 0)
            {
                maxHp = RunManager.Instance.HeroMaxHP;
                currentHp = RunManager.Instance.HeroCurrentHP;
            }
            else
            {
                RunManager.Instance.HeroMaxHP = maxHp;
                RunManager.Instance.HeroCurrentHP = currentHp;
            }
        }

        SetupBase(maxHp, heroData.Image, currentHp);
    }
}
