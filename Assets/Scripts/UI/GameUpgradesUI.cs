using UnityEngine;
using UnityEngine.UI;

public class GameUpgradesUI : MonoBehaviour {


    [Header("References")]
    [SerializeField] private StatisticsUI statisticsUI;

    [Header("Button")]
    [SerializeField] private Button statisticsButton;


    private void Awake() {
        statisticsButton.onClick.AddListener(() => {
            statisticsUI.Show();
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