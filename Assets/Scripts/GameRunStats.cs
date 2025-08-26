using UnityEngine;

public class GameRunStats {


    public int biscuitsEarned;
    public int enemiesKilled;
    public int pintsEarned;


    public GameRunStats(int biscuitsEarned, int enemiesKilled, int pintsEarned) { 
        this.biscuitsEarned = biscuitsEarned;
        this.pintsEarned = pintsEarned;
        this.enemiesKilled = enemiesKilled;
    }
}