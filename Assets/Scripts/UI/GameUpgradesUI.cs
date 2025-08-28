using UnityEngine;
using UnityEngine.UI;

public class GameUpgradesUI : MonoBehaviour {


    [Header("References")]
    [SerializeField] private StatisticsUI statisticsUI;

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
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}