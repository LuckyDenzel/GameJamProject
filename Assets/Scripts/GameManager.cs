using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public const string GROUND_TAG = "Ground";
    private const string BISCUITS_EARNED_AMOUNT_PLAYER_PREFS = "SavedBiscuitsEarnedPlayerPrefs";
    private const string PINTS_EARNED_AMOUNT_PLAYER_PREFS = "SavedPintsEarnedPlayerPrefs";
    private const string RUN_EXIT_TIME_DELAY_PLAYER_PREFS = "SavedRunExitTimeDelayPlayerPrefs";


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

    public event EventHandler<OnExitRunChangedEventArgs> OnExitRunChanged;
    public class OnExitRunChangedEventArgs : EventArgs {
        public float exitTimeDelay;
        public bool hasStarted;

        public OnExitRunChangedEventArgs(float exitTimeDelay, bool hasStarted) {
            this.exitTimeDelay = exitTimeDelay;
            this.hasStarted = hasStarted;
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
        endRunExitTimerDelay = PlayerPrefs.GetFloat(RUN_EXIT_TIME_DELAY_PLAYER_PREFS, endRunExitTimerDelay);

        OnCurrentBiscuitsAmountChanged?.Invoke(this, new OnCurrentScoreChangedEventArgs(currentBiscuitsScore));

        endRunExitTimer = endRunExitTimerDelay;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            isExitingGame = true;

            OnExitRunChanged?.Invoke(this, new OnExitRunChangedEventArgs(endRunExitTimerDelay, true));
        }

        if (isExitingGame) {
            if (Input.GetKeyUp(KeyCode.V)) {
                isExitingGame = false;
                Debug.Log("Canceled");

                OnExitRunChanged?.Invoke(this, new OnExitRunChangedEventArgs(endRunExitTimerDelay, false));
                return;
            }

            endRunExitTimer -= Time.deltaTime;

            if (endRunExitTimer <= 0) {
                endRunExitTimer = endRunExitTimerDelay;

                // End the game when the user has succeeded holding the exit button for long enough
                EndGameSuccesfully();

                isExitingGame = false;
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

    public void DecreaseRunExitTimeDelay(float amount) {
        Debug.Log($"Attempting to decrease by {amount}. Current value: {endRunExitTimerDelay}");

        if (endRunExitTimerDelay - amount >= 0) {
            endRunExitTimerDelay -= amount;

            PlayerPrefs.SetFloat(RUN_EXIT_TIME_DELAY_PLAYER_PREFS, endRunExitTimerDelay);
        } else {
            Debug.LogError("You can't set the exit time to a negative number!");
        }
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