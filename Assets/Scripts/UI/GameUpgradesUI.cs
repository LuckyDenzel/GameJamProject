using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUpgradesUI : MonoBehaviour {


    [Header("References")]
    [SerializeField] private StatisticsUI statisticsUI;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI biscuitsAmountText;

    [Header("Buttons")]
    [SerializeField] private Button newRunButton;
    [SerializeField] private Button statisticsButton;

    [Header("Animation")]
    [SerializeField] private AnimationClip popInAnimationClip;
    [SerializeField] private AnimationClip popOutAnimationClip;

    private Animator animator;


    private void Awake() {
        animator = GetComponent<Animator>();

        statisticsButton.onClick.AddListener(() => {
            statisticsUI.Show();
        });
        newRunButton.onClick.AddListener(() => {
            animator.Play(popOutAnimationClip.name);
        });

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Start() {
        Hide();

        GameManager.Instance.OnTotalBiscuitsAmountChanged += GameManager_OnBiscuitsAmountChanged;
    }

    private void GameManager_OnBiscuitsAmountChanged(object sender, GameManager.OnTotalScoreChangedEventArgs e) {
        if (gameObject.activeInHierarchy) {
            biscuitsAmountText.text = $": {e.newAmount}";
        }
    }

    public void Show() {
        biscuitsAmountText.text = $": {GameManager.Instance.GetTotalBiscuitsScore()}";

        gameObject.SetActive(true);

        animator.Play(popInAnimationClip.name);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void PopOutAnimationCallback() {
        GameManager.Instance.StartNewRun();
    }
}