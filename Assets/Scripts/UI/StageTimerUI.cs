using TMPro;
using UnityEngine;

public class StageTimerUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI timerText;

    private float stageTimer;
    private float textUpdateTimer;


    private void Start() {
        GameStageManager.Instance.OnStageChanged += GameStageManager_OnStageChanged;

        stageTimer = GameStageManager.Instance.GetCurrentGameStage().stageDuration;
    }

    private void Update() {
        stageTimer -= Time.deltaTime;

        if (stageTimer <= 0) {
            stageTimer = 0;
        }

        // Avoid updating the text every frame, instead update it every second
        textUpdateTimer -= Time.deltaTime;

        if (textUpdateTimer <= 0) {
            textUpdateTimer = 1f;

            timerText.text = $"Time: {Mathf.RoundToInt(stageTimer)}";
        }
    }

    private void GameStageManager_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        stageTimer = e.newGameStage.stageDuration;
    }
}