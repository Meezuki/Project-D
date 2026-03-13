using UnityEngine;
using System;
public class CurrencyManager : MonoBehaviour
{
    // 
    /*
     ============ Jika ingin add Gold ============

     CurrencyManager.Instance.AddGold(goldReward);
     
     ============Jika ingin spend Gold ============

     if(CurrencyManager.Instance.SpendGold(relicPrice){
        KASIH RELIC/KARTU
    } else{
        UANG KURANG
    }

    ============ Jika Game Over/ ingin menghilangkan semua Gold ============

    CurrencyManager.Instance.Instance.ResetGold();


     */

    public static CurrencyManager Instance { get; private set; }

    // otomatis kasih tau UI untuk update saat gold berubah
    public static event Action<int> OnGoldChanged;

    private int currentGold = 0;
    public int startingGold = 99;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Another instance found of this gameObject");
            // hancurin objek baru biar tetep satu
            Destroy(gameObject);
            return;
        }
        LoadGold();
    }

    private void LoadGold()
    {
        // ambil dari data playerGold dan masukkan ke  currentGold variable
        currentGold = PlayerPrefs.GetInt("PlayerGold");
    }

    public int getGold()
    {
        Debug.Log("getGold() called");
        return currentGold;
    }


    public void AddGold(int amount)
    {
        currentGold += amount;
        SaveGold();
        // kasih tau UI goldnya berubah ke value baru
        OnGoldChanged?.Invoke(currentGold);

    }

    public bool spendGold(int amount)
    {
        if(amount <= currentGold)
        {
            currentGold -= amount;
            SaveGold();
            // KAsih tau UI bahwa gold berubah
            OnGoldChanged?.Invoke(currentGold);
            return true;
        }
        else
        {
            Debug.Log("Gold tidak cukup! Gold: " + amount + " Amount to be subtracted" + amount);
            return false;
        }
    }


    private void SaveGold()
    {
        // save ke data "PlayerGold"
        PlayerPrefs.SetInt("PlayerGold", currentGold);
        PlayerPrefs.Save();

    }

    public void ResetGold()
    {
        currentGold = startingGold;
        SaveGold();
        OnGoldChanged?.Invoke(currentGold);
    }


}
