using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SaveAndQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AbandonRun()
    {
        // 1. Hapus memori peta yang lama
        PlayerPrefs.DeleteKey("Map");
        PlayerPrefs.Save();
        // 2. Load scene peta
        // Saat MapScene terbuka, MapManager di sana akan mendeteksi
        // tidak ada save data, lalu otomatis memanggil GenerateNewMap()
        CurrencyManager.Instance.ResetGold();
        SceneManager.LoadScene("MainMenu");
    }
}
