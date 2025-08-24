using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

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

    public void AddScore(int amount) {
        currentScore += amount;

        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs(currentScore));
    }

    public int GetCurrentScore() {
        return currentScore;
    }
}