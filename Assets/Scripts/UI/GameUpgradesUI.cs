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


    private void Awake() {
        statisticsButton.onClick.AddListener(() => {
            statisticsUI.Show();
        });
        newRunButton.onClick.AddListener(() => {
            GameManager.Instance.StartNewRun();
        });
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
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}