using UnityEngine;

public static class GameStatsTracker {


    public const string TOTAL_BISCUITS_EARNED_PLAYER_PREFS = "SavedBiscuitsEarnedPlayerPrefs";
    public const string TOTAL_PINTS_EARNED_PLAYER_PREFS = "SavedPintsEarnedPlayerPrefs";
    public const string TOTAL_ENEMIES_KILLED_PLAYER_PREFS = "SavedEnemiesKilledPlayerPrefs";
    public const string TOTAL_STAGES_PASSED_PLAYER_PREFS = "SavedTotalStagesPassedPlayerPrefs";
    public const string TOTAL_RUNS_COMPLETED_PLAYER_PREFS = "SavedTotalRunsCompletedPlayerPrefs";

    private static int biscuitsEarned;
    private static int pintsEarned;
    private static int enemiesKilled;
    private static int totalStagesPassed;
    private static int totalRunsCompleted;

    public static int BiscuitsEarned {
        get => biscuitsEarned;
        set {
            if (biscuitsEarned != value) {
                biscuitsEarned = value;

                PlayerPrefs.SetInt(TOTAL_BISCUITS_EARNED_PLAYER_PREFS, biscuitsEarned);
                PlayerPrefs.Save();
            }
        }
    }
    public static int EnemiesKilled {
        get => enemiesKilled;
        set {
            if (enemiesKilled != value) {
                enemiesKilled = value;

                PlayerPrefs.SetInt(TOTAL_ENEMIES_KILLED_PLAYER_PREFS, enemiesKilled);
                PlayerPrefs.Save();
            }
        }
    }
    public static int PintsEarned {
        get => pintsEarned;
        set {
            if (pintsEarned != value) {
                pintsEarned = value;

                PlayerPrefs.SetInt(TOTAL_PINTS_EARNED_PLAYER_PREFS, pintsEarned);
                PlayerPrefs.Save();
            }
        }
    }
    public static int TotalStagesPassed {
        get => totalStagesPassed;
        set {
            if (totalStagesPassed != value) {
                totalStagesPassed = value;

                PlayerPrefs.SetInt(TOTAL_STAGES_PASSED_PLAYER_PREFS, totalStagesPassed);
                PlayerPrefs.Save();
            }
        }
    }
    public static int TotalRunsCompleted {
        get => totalRunsCompleted;
        set {
            if (totalRunsCompleted != value) {
                totalRunsCompleted = value;

                PlayerPrefs.SetInt(TOTAL_RUNS_COMPLETED_PLAYER_PREFS, totalRunsCompleted);
                PlayerPrefs.Save();
            }
        }
    }
}