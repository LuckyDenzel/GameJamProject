using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndResultUI : MonoBehaviour {


    public static GameEndResultUI Instance { get; private set; }


    [SerializeField] private GameUpgradesUI upgradesUI;

    [SerializeField] private Transform canvasTransform;

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI biscuitsEarnedAmountText;
    [SerializeField] private TextMeshProUGUI totalEarnedPintsText;

    [SerializeField] private TextMeshProUGUI totalCookiesText;
    [SerializeField] private TextMeshProUGUI totalPintsText;

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
        totalEarnedPintsText.text = $"Total Earned Pints: {gameRunStats.pintsEarned}";
        totalCookiesText.text = $"Total Cookies: {GameManager.Instance.GetTotalBiscuitsScore()}";
        totalPintsText.text = $"Total Pints: {GameManager.Instance.GetTotalPintsScore()}";
    }

    private void Show() {
        gameObject.SetActive(true);

        foreach (Transform UI in canvasTransform) {
            if (UI == transform) continue;

            UI.gameObject.SetActive(false);
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}