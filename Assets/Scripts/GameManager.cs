using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public const string GROUND_TAG = "Ground";
    private const string BISCUITS_EARNED_AMOUNT_PLAYER_PREFS = "SavedBiscuitsEarnedPlayerPrefs";
    private const string PINTS_EARNED_AMOUNT_PLAYER_PREFS = "SavedPintsEarnedPlayerPrefs";


    public static GameManager Instance { get; private set; }


    public event EventHandler<OnTotalScoreChangedEventArgs> OnTotalBiscuitsAmountChanged;
    public class OnTotalScoreChangedEventArgs : EventArgs {
        public int newAmount;

        public OnTotalScoreChangedEventArgs(int newAmount) {
            this.newAmount = newAmount;
        }
    } 
    public event EventHandler<OnCurrentScoreChangedEventArgs> OnCurrentBiscuitsAmountChanged;
    public class OnCurrentScoreChangedEventArgs : EventArgs {
        public int newAmount;

        public OnCurrentScoreChangedEventArgs(int newAmount) {
            this.newAmount = newAmount;
        }
    }

    [SerializeField] private LayerMask playerLayerMask;

    private int currentBiscuitsScore = 0;
    private int currentPintsScore = 0;

    private int totalEarnedBiscuitsScore;
    private int totalEarnedPintsScore;

    private float endRunExitTimer;
    private float endRunExitTimerDelay = 5;

    private bool isExitingGame;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        totalEarnedBiscuitsScore = PlayerPrefs.GetInt(BISCUITS_EARNED_AMOUNT_PLAYER_PREFS, 0);
        totalEarnedPintsScore = PlayerPrefs.GetInt(PINTS_EARNED_AMOUNT_PLAYER_PREFS, 0);

        OnCurrentBiscuitsAmountChanged?.Invoke(this, new OnCurrentScoreChangedEventArgs(currentBiscuitsScore));

        endRunExitTimer = endRunExitTimerDelay;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            isExitingGame = true;
        }

        if (isExitingGame) {
            if (Input.GetKeyUp(KeyCode.V)) {
                isExitingGame = false;
                Debug.Log("Canceled");

                return;
            }

            endRunExitTimer -= Time.deltaTime;

            if (endRunExitTimer <= 0) {
                endRunExitTimer = endRunExitTimerDelay;

                // End the game when the user has succeeded holding the exit button for long enough
                EndGameSuccesfully();
            }
        }
    }

    public void AddBiscuitToScore(int value) {
        currentBiscuitsScore += value;

        GameStatsTracker.BiscuitsEarned++;

        OnCurrentBiscuitsAmountChanged?.Invoke(this, new OnCurrentScoreChangedEventArgs(currentBiscuitsScore));
    }

    public void AddPintToScore(int value) {
        currentPintsScore += value;

        GameStatsTracker.PintsEarned++;
    }

    public void SpendTotalBiscuits(int amount) {
        if (totalEarnedBiscuitsScore - amount >= 0) {
            totalEarnedBiscuitsScore -= amount;

            // Save the amount of biscuits earned
            PlayerPrefs.SetInt(BISCUITS_EARNED_AMOUNT_PLAYER_PREFS, totalEarnedBiscuitsScore);

            OnTotalBiscuitsAmountChanged?.Invoke(this, new OnTotalScoreChangedEventArgs(totalEarnedBiscuitsScore));
        }
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

        OnTotalBiscuitsAmountChanged?.Invoke(this, new OnTotalScoreChangedEventArgs(totalEarnedBiscuitsScore));

        PlayerPrefs.Save();

        GameStageManager.Instance.EndGame();
    }

    public void EndGameFailed() {
        Time.timeScale = 0f;

        currentBiscuitsScore = 0;
        currentPintsScore = 0;

        GameStageManager.Instance.CurrentRunStats.biscuitsEarned = currentBiscuitsScore;
        GameStageManager.Instance.CurrentRunStats.pintsEarned = currentPintsScore;

        OnTotalBiscuitsAmountChanged?.Invoke(this, new OnTotalScoreChangedEventArgs(totalEarnedBiscuitsScore));

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

    public LayerMask GetPlayerLayerMask() {
        return playerLayerMask;
    }
}