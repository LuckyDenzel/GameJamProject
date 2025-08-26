using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public const string GROUND_TAG = "Ground";
    private const string BISCUITS_EARNED_AMOUNT_PLAYER_PREFS = "SavedBiscuitsEarnedPlayerPrefs";
    private const string PINTS_EARNED_AMOUNT_PLAYER_PREFS = "SavedPintsEarnedPlayerPrefs";

    public static GameManager Instance { get; private set; }


    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public class OnScoreChangedEventArgs : EventArgs {
        public int newScore;

        public OnScoreChangedEventArgs(int newScore) {
            this.newScore = newScore;
        }
    }

    private int currentBiscuitsScore = 0;
    private int currentPintsScore = 0;

    private int totalEarnedBiscuitsScore;
    private int totalEarnedPintsScore;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        totalEarnedBiscuitsScore = PlayerPrefs.GetInt(BISCUITS_EARNED_AMOUNT_PLAYER_PREFS, 0);
        totalEarnedPintsScore = PlayerPrefs.GetInt(PINTS_EARNED_AMOUNT_PLAYER_PREFS, 0);

        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs(currentBiscuitsScore));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            // Disable all the stage handling
            GameManager.Instance.EndGameSuccesfully();
        }
    }

    public void AddBiscuitToScore(int value) {
        currentBiscuitsScore += value;

        GameStatsTracker.BiscuitsEarned++;

        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs(currentBiscuitsScore));
    }

    public void AddPintToScore(int value) {
        currentPintsScore += value;

        GameStatsTracker.PintsEarned++;
    }

    public void StartNewRun() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGameSuccesfully() {
        Time.timeScale = 0f;

        // Add the earned points to the total points
        totalEarnedBiscuitsScore += currentBiscuitsScore;
        totalEarnedPintsScore += currentPintsScore;

        // Save the amount of biscuits earned
        PlayerPrefs.SetInt(BISCUITS_EARNED_AMOUNT_PLAYER_PREFS, totalEarnedBiscuitsScore);

        // Save the amount of pints earned
        PlayerPrefs.SetInt(PINTS_EARNED_AMOUNT_PLAYER_PREFS, totalEarnedPintsScore);

        GameStageManager.Instance.CurrentRunStats.biscuitsEarned = currentBiscuitsScore;
        GameStageManager.Instance.CurrentRunStats.pintsEarned = currentPintsScore;

        PlayerPrefs.Save();

        GameStageManager.Instance.EndGame();
    }

    public void EndGameFailed() {
        Time.timeScale = 0f;

        currentBiscuitsScore = 0;
        currentPintsScore = 0;

        GameStageManager.Instance.CurrentRunStats.biscuitsEarned = currentBiscuitsScore;
        GameStageManager.Instance.CurrentRunStats.pintsEarned = currentPintsScore;

        GameStageManager.Instance.EndGame();
    }

    public int GetCurrentBiscuitsScore() {
        return currentBiscuitsScore;
    }

    public int GetCurrentPintsScore() {
        return currentPintsScore;
    }

    public int GetTotalBiscuitsScore() {
        return totalEarnedBiscuitsScore;
    }

    public int GetTotalPintsScore() {
        return totalEarnedPintsScore;
    }
}