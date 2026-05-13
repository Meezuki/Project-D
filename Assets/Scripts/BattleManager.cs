using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    public class BattleManager : MonoBehaviour
    {

        [Header("Pengaturan Hadiah Gold")]
        public int minGoldReward = 10;
        public int maxGoldReward = 25;


        public int GenerateGoldReward()
        {
            // Hanya menghitung angka random, belum menambahkannya ke CurrencyManager
            return Random.Range(minGoldReward, maxGoldReward + 1);
        }

        public void GameOver()
        {
            // 1. Hapus memori peta yang lama
            PlayerPrefs.DeleteKey("Map");
            PlayerPrefs.SetInt("IsBossBattle", 0); // just in cas
            PlayerPrefs.Save();

            // 2. Load scene peta
            // Saat MapScene terbuka, MapManager di sana akan mendeteksi
            // tidak ada save data, lalu otomatis memanggil GenerateNewMap()
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.ResetGold();
            }
            SceneManager.LoadScene("MapScene");
        }


        public void WinBattle()
        {
            StartCoroutine(WinBattleRoutine());
        }

        // Ubah WinBattle agar menerima jumlah gold yang sudah dihitung
        public void WinBattle(int amountToAdd)
        {
            StartCoroutine(WinBattleRoutine(amountToAdd));
        }


        public IEnumerator WinBattleRoutine()
        {

            int randomGold = Random.Range(minGoldReward, maxGoldReward + 1);
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddGold(randomGold);
                Debug.Log($"Musuh dikalahkan! Mendapatkan hadiah {randomGold} Gold.");
            }
            else
            {
                Debug.LogWarning("Gagal memberikan Gold: CurrencyManager tidak ditemukan di Scene!");
            }

            // Optional: Wait for a moment so the player sees the reward/death animation
            yield return new WaitForSeconds(1.5f);

            // Kita baca catatan yang dibuat sebelum masuk scene ini
            int isBoss = PlayerPrefs.GetInt("IsBossBattle", 0);

            if (isBoss == 1)
            {
                // PLAYER MENGALAHKAN BOSS!
                Debug.Log("BOSS DEFEATED! Selamat!");

                // Jangan lupa reset catatannya agar pertarungan selanjutnya normal lagi
                PlayerPrefs.SetInt("IsBossBattle", 0);

                // --- PILIH SALAH SATU AKSI DI BAWAH INI ---

                // Opsi A: Pindah ke Scene Kemenangan (Victory/Credit Screen)
                // SceneManager.LoadScene("VictoryScene"); 


                // Opsi B: Lanjut ke Peta Baru (Lantai/Act 2)
                // Hapus data peta lama, lalu buka MapScene untuk generate map baru
                PlayerPrefs.DeleteKey("Map");
                PlayerPrefs.Save();
                SceneManager.LoadScene("MapScene"); // OR
                //SceneManager.LoadScene("VictoryScene");
            }
            else
            {
                // PLAYER MENGALAHKAN MUSUH BIASA
                Debug.Log("Musuh biasa dikalahkan. Kembali ke peta...");
                SceneManager.LoadScene("MapScene");
            }
        }


        private IEnumerator WinBattleRoutine(int amountToAdd)
        {
            // 1. Tambahkan gold yang dikirim dari UI
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddGold(amountToAdd);
            }

            yield return new WaitForSeconds(0.5f);

            // 2. Cek Boss dan pindah Scene (logika lama kamu)
            bool isBoss = PlayerPrefs.GetInt("IsBossBattle", 0) == 1;
            if (isBoss)
            {
                PlayerPrefs.SetInt("IsBossBattle", 0);
                PlayerPrefs.Save();
                SceneManager.LoadScene("VictoryScene");
            }
            else
            {
                SceneManager.LoadScene("MapScene");
            }


        }
    }
}