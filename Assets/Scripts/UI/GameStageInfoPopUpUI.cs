using TMPro;
using UnityEngine;

public class GameStageInfoPopUpUI : MonoBehaviour {


    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI gameStageDurationText;
    [SerializeField] private TextMeshProUGUI gameStageThreatsMultiplierText;
    [SerializeField] private TextMeshProUGUI gameStageBiscuitMultiplierText;

    [Header("Animation Clips")]
    [SerializeField] private AnimationClip gameStageShowAnimation;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        GameStageManager.Instance.OnStageChanged += GameStageManager_OnStageChanged;

        animator.Play(gameStageShowAnimation.name);

        GameStage stage = GameStageManager.Instance.GetCurrentGameStage();
        UpdateUI(stage.stageDuration.ToString(), stage.stageThreatsDamageMultiplier.ToString(), stage.stageBiscuitMultiplier.ToString());
    }

    private void GameStageManager_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        Show();

        GameStage stage = e.newGameStage;
        UpdateUI(stage.stageDuration.ToString(), 
            stage.stageThreatsDamageMultiplier.ToString(),
            stage.stageBiscuitMultiplier.ToString()
        );

        animator.Play(gameStageShowAnimation.name);
    }

    private void UpdateUI(string stageDuration, string stageThreatsMultiplier, string stageBiscuitMultiplier) {
        gameStageDurationText.text = $"Stage Duration: {stageDuration}";
        gameStageThreatsMultiplierText.text = $"Threats Damage Multiplier {stageThreatsMultiplier}";
        gameStageBiscuitMultiplierText.text = $"Biscuits Value Multiplier {stageBiscuitMultiplier}";
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    
    private void Hide() {
        gameObject.SetActive(false);
    }
}