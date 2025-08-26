using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndResultUI : MonoBehaviour {


    public static GameEndResultUI Instance { get; private set; }


    [SerializeField] private GameUpgradesUI upgradesUI;

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI biscuitsEarnedAmountText;
    [SerializeField] private TextMeshProUGUI totalEnemiesKilledText;

    [Header("Button References")]
    [SerializeField] private Button newRunButton;
    [SerializeField] private Button upgradesButton;


    private void Awake() {
        Instance = this;

        newRunButton.onClick.AddListener(() => {
            GameManager.Instance.StartNewRun();

            Hide();
        });
        upgradesButton.onClick.AddListener(() => {
            upgradesUI.Show();
        });
    }

    private void Start() {
        Hide();
    }

    public void ShowGameEndResultStats(GameRunStats gameRunStats) {
        Show();

        biscuitsEarnedAmountText.text = $"Total Earned Biscuits: {gameRunStats.biscuitsEarned}";
        totalEnemiesKilledText.text = $"Total Enemies Killed: {gameRunStats.enemiesKilled}";
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}