using UnityEngine;

public class GameRunStats {


    public int biscuitsEarned;
    public int enemiesKilled;
    public int pintsEarned;

    public bool gameSucceeded;


    public GameRunStats(int biscuitsEarned, int enemiesKilled, int pintsEarned, bool gameSucceeded) { 
        this.biscuitsEarned = biscuitsEarned;
        this.pintsEarned = pintsEarned;
        this.enemiesKilled = enemiesKilled;
        this.gameSucceeded = gameSucceeded;
    }
}