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

    private int currentBiscuitsScore;
    private int currentPintsScore;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        currentBiscuitsScore = PlayerPrefs.GetInt(BISCUITS_EARNED_AMOUNT_PLAYER_PREFS, 0);
        currentPintsScore = PlayerPrefs.GetInt(PINTS_EARNED_AMOUNT_PLAYER_PREFS, 0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            // Disable all the stage handling
            GameStageManager.Instance.EndGame();
        }
    }

    public void AddBiscuitToScore(int value) {
        currentBiscuitsScore += value;

        GameStageManager.Instance.CurrentRunStats.biscuitsEarned++;
        GameStatsTracker.BiscuitsEarned++;

        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs(currentBiscuitsScore));
    }

    public void AddPintToScore(int value) {
        currentPintsScore += value;

        GameStageManager.Instance.CurrentRunStats.pintsEarned++;
        GameStatsTracker.PintsEarned++;
    }

    public void StartNewRun() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame() {
        Time.timeScale = 0f;

        // Save the amount of biscuits earned
        PlayerPrefs.SetInt(BISCUITS_EARNED_AMOUNT_PLAYER_PREFS, currentBiscuitsScore);

        GameStageManager.Instance.EndGame();
    }

    public int GetCurrentBiscuitsScore() {
        return currentBiscuitsScore;
    }

    public int GetCurrentPintsScore() {
        return currentPintsScore;
    }
}