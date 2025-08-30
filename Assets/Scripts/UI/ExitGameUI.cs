using UnityEngine;
using UnityEngine.UI;

public class ExitGameUI : MonoBehaviour {


    [SerializeField] private Image timerImage;

    float exitTimeTimer;
    float exitTimeTimerDelay;

    bool shouldCountdown;

    private void Start() {
        GameManager.Instance.OnExitRunChanged += GameManager_OnExitRunChanged;

        timerImage.fillAmount = 0;

        Hide();
    }

    private void Update() {
        if (shouldCountdown) {
            exitTimeTimer += Time.deltaTime;
            float timerNormalized = exitTimeTimer / exitTimeTimerDelay;
            timerImage.fillAmount = timerNormalized;

            if (exitTimeTimer >= exitTimeTimerDelay) {
                shouldCountdown = false;
                timerImage.fillAmount = 0;

                Hide();
            }
        }
    }

    private void GameManager_OnExitRunChanged(object sender, GameManager.OnExitRunChangedEventArgs e) {
        exitTimeTimerDelay = e.exitTimeDelay;
        exitTimeTimer = 0;
        shouldCountdown = e.hasStarted;

        if (shouldCountdown) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}