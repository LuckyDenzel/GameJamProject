using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStageHandler : MonoBehaviour {


    public static GameStageHandler Instance { get; private set; }


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


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        // Start at stage index 0
        currentStageIndex = 0;

        currentStage = stagesList[currentStageIndex];
    }

    public void Update() {
        currentStageTimer -= Time.deltaTime;

        if (currentStageTimer <= 0) {
            currentStageIndex++;

            currentStage = stagesList[currentStageIndex];

            currentStageTimer = currentStage.stageDuration;

            OnStageChanged?.Invoke(this, new OnStageChangedEventArgs(currentStage));

            Debug.Log($"Current stage: stage {currentStageIndex}");
        }
    }
}