using TMPro;
using UnityEngine;

public class StatisticsUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI totalEarnedBiscuitsText;
    [SerializeField] private TextMeshProUGUI totalEnemiesKilledText;
    [SerializeField] private TextMeshProUGUI totalEarnedPintsText;
    [SerializeField] private TextMeshProUGUI totalStagesPassedText;
    [SerializeField] private TextMeshProUGUI totalRunsCompletedText;


    private void Start() {
        Hide();
    }

    private void UpdateUI() {
        totalEarnedBiscuitsText.text = $"Total Earned Biscuits: {PlayerPrefs.GetInt(GameStatsTracker.TOTAL_BISCUITS_EARNED_PLAYER_PREFS)}";
        totalEnemiesKilledText.text = $"Total Killed Enemies: {PlayerPrefs.GetInt(GameStatsTracker.TOTAL_ENEMIES_KILLED_PLAYER_PREFS)}";
        totalEarnedPintsText.text = $"Total Earned Pints: {PlayerPrefs.GetInt(GameStatsTracker.TOTAL_PINTS_EARNED_PLAYER_PREFS)}";
        totalStagesPassedText.text = $"Total Passed Stages: {PlayerPrefs.GetInt(GameStatsTracker.TOTAL_STAGES_PASSED_PLAYER_PREFS)}";
        totalRunsCompletedText.text = $"Total Completed Runs: {PlayerPrefs.GetInt(GameStatsTracker.TOTAL_STAGES_PASSED_PLAYER_PREFS)}";
    }

    public void Show() {
        UpdateUI();

        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}