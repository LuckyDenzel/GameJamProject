using TMPro;
using UnityEngine;

public class StageTimerUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Animations")]
    [SerializeField] private AnimationClip timerPopInAnimationClip;
    [SerializeField] private AnimationClip timerPopOutAnimationClip;

    private Animator animator;

    private float stageTimer;
    private float stageTimerDelay;

    private float textUpdateTimer;



    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        GameStageManager.Instance.OnStageChanged += GameStageManager_OnStageChanged;

        stageTimer = GameStageManager.Instance.GetCurrentGameStage().stageDuration;

        animator.Play(timerPopInAnimationClip.name);
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
        animator.Play(timerPopOutAnimationClip.name);

        stageTimerDelay = e.newGameStage.stageDuration;
    }

    private void PlayPopInAnimation() {
        animator.Play(timerPopInAnimationClip.name);

        stageTimer = stageTimerDelay;
    }
}