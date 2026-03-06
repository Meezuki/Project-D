using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    public class BattleManager : MonoBehaviour
    {
        
        public void GameOver()
        {
            // 1. Hapus memori peta yang lama
            PlayerPrefs.DeleteKey("Map");
            PlayerPrefs.Save(); 

            // 2. Load scene peta
            // Saat MapScene terbuka, MapManager di sana akan mendeteksi
            // tidak ada save data, lalu otomatis memanggil GenerateNewMap()
            SceneManager.LoadScene("MapScene");
        }


        public void WinBattle()
        {
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
                SceneManager.LoadScene("MapScene");
            }
            else
            {
                // PLAYER MENGALAHKAN MUSUH BIASA
                Debug.Log("Musuh biasa dikalahkan. Kembali ke peta...");
                SceneManager.LoadScene("MapScene");
            }
        }

    }
}