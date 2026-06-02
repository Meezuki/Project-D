using TMPro; // Wajib ada untuk TMP_Text
using UnityEngine;
using Map;
using System.Collections;

public class MatchManagerSystem : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text goldRewardText; // Tarik objek teks Gold ke sini

    private int temporaryGoldWon; // Untuk menyimpan angka sementara

    private void OnEnable() => ActionSystem.AttachPerformer<WinMatchGA>(WinMatchPerformer);
    private void OnDisable() => ActionSystem.DetachPerformer<WinMatchGA>();

    private IEnumerator WinMatchPerformer(WinMatchGA winMatchGA)
    {
        //Interactions.Instance.IsGameOver = true;
        yield return new WaitForSeconds(1.5f);

        if (victoryPanel != null)
        {
            // 1. Minta BattleManager hitung Gold
            temporaryGoldWon = battleManager.GenerateGoldReward();

            // 2. Tampilkan ke UI
            if (goldRewardText != null)
            {
                goldRewardText.text = "You won " + temporaryGoldWon + " Gold!";
            }

            victoryPanel.SetActive(true);
        }
        yield return null;
    }

    public void ContinueFromVictory()
    {
        // Kirim angka gold yang tadi ditampilkan di UI untuk disimpan
        battleManager.WinBattle(temporaryGoldWon);
    }
}