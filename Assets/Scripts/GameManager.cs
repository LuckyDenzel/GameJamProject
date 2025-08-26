using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public const string GROUND_TAG = "Ground";
    private const string BISCUITS_EARNED_AMOUNT_PLAYER_PREFS = "SavedBiscuitsEarnedPlayerPrefs";

    public static GameManager Instance { get; private set; }


    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public class OnScoreChangedEventArgs : EventArgs {
        public int newScore;

        public OnScoreChangedEventArgs(int newScore) {
            this.newScore = newScore;
        }
    }

    private int currentScore = 0;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        currentScore = PlayerPrefs.GetInt(BISCUITS_EARNED_AMOUNT_PLAYER_PREFS, 0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            // Disable all the stage handling
            GameStageManager.Instance.EndGame();
        }
    }

    public void AddScore(int amount) {
        currentScore += amount;

        GameStageManager.Instance.CurrentRunStats.biscuitsEarned++;
        GameStatsTracker.BiscuitsEarned++;

        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs(currentScore));
    }

    public void StartNewRun() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame() {
        Time.timeScale = 0f;

        // Save the amount of biscuits earned
        PlayerPrefs.SetInt(BISCUITS_EARNED_AMOUNT_PLAYER_PREFS, currentScore);

        GameStageManager.Instance.EndGame();
    }

    public int GetCurrentScore() {
        return currentScore;
    }
}