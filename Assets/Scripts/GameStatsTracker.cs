using UnityEngine;

public static class GameStatsTracker {


    private const string BISCUITS_EARNED_PLAYER_PREFS = "SavedBiscuitsEarnedPlayerPrefs";
    private const string ENEMIES_KILLED_PLAYER_PREFS = "SavedEnemiesKilledPlayerPrefs";

    private static int biscuitsEarned;
    private static int enemiesKilled;

    public static int BiscuitsEarned {
        get => biscuitsEarned;
        set {
            if (biscuitsEarned != value) {
                biscuitsEarned = value;

                PlayerPrefs.SetInt(BISCUITS_EARNED_PLAYER_PREFS, biscuitsEarned);
                PlayerPrefs.Save();
            }
        }
    }
    public static int EnemiesKilled {
        get => enemiesKilled;
        set {
            if (enemiesKilled != value) {
                enemiesKilled = value;

                PlayerPrefs.SetInt(ENEMIES_KILLED_PLAYER_PREFS, enemiesKilled);
                PlayerPrefs.Save();
            }
        }
    }
}