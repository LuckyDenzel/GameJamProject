using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI scoreTextReference;

    private void Start() {
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;

        // Set the starting score to begin at zero
        scoreTextReference.text = "0";

        Show();
    }

    private void GameManager_OnScoreChanged(object sender, GameManager.OnScoreChangedEventArgs e) {
        UpdateScoreText(e.newScore);
    }

    public void UpdateScoreText(int currentScore) {
        scoreTextReference.text = string.Empty;

        scoreTextReference.text = currentScore.ToString();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}