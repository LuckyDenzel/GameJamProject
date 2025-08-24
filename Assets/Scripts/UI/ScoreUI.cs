using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;

        // Set the starting score to begin at zero
        scoreText.text = "0";

        Show();
    }

    private void GameManager_OnScoreChanged(object sender, GameManager.OnScoreChangedEventArgs e) {
        UpdateScoreText(e.newScore);
    }

    public void UpdateScoreText(int currentScore) {
        scoreText.text = string.Empty;

        scoreText.text = currentScore.ToString();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}