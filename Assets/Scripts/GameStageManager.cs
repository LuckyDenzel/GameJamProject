using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStageManager : MonoBehaviour {


    public static GameStageManager Instance { get; private set; }


    public event EventHandler<OnStageChangedEventArgs> OnStageChanged;
    public class OnStageChangedEventArgs : EventArgs {
        public GameStage newGameStage;

        public OnStageChangedEventArgs(GameStage newGameStage) { 
            this.newGameStage = newGameStage;
        }
    }

    [SerializeField] private List<GameStage> stagesList;

    private int currentStageIndex;

    private float currentStageTimer;

    private GameStage currentStage;
    private GameRunStats currentRunStats;

    private bool hasReachedFinalStage = false;
    private bool hasGameEnded = false;

    public GameRunStats CurrentRunStats { get => currentRunStats; set => currentRunStats = value; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        // Start at stage index 0
        currentStageIndex = 0;

        currentStage = stagesList[currentStageIndex];
        currentStageTimer = currentStage.stageDuration;

        // Initialize the run stats
        currentRunStats = new GameRunStats(GameManager.Instance.GetCurrentScore(), Player_Logic.PlayerCombatIntance.GetCurrentEnemiesKilledAmount());
    }

    public void Update() {
        if (hasReachedFinalStage || hasGameEnded) {
            return;
        }

        currentStageTimer -= Time.deltaTime;

        if (currentStageTimer <= 0) {
            HandleNextStage();
        }
    }

    private void HandleNextStage() {
        currentStageIndex++;

        currentStage = stagesList[currentStageIndex];

        currentStageTimer = currentStage.stageDuration;

        if (currentStageIndex >= stagesList.Count - 1) {
            hasReachedFinalStage = true;
        }

        OnStageChanged?.Invoke(this, new OnStageChangedEventArgs(currentStage));

        Debug.Log($"Current stage: stage {currentStageIndex}");
    }

    public void EndGame() {
        hasGameEnded = true;

        GameEndResultUI.Instance.ShowGameEndResultStats(currentRunStats);
    }
}