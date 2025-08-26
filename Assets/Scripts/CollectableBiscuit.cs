using UnityEngine;

public class CollectableBiscuit : MonoBehaviour {


    [SerializeField] private int startBiscuitPickupValue = 1;

    private int currentBiscuitPickupValue;

    private void Start() {
        GameStageHandler.Instance.OnStageChanged += GameStageHandler_OnStageChanged;

        currentBiscuitPickupValue = startBiscuitPickupValue;
    }

    private void GameStageHandler_OnStageChanged(object sender, GameStageHandler.OnStageChangedEventArgs e) {
        ApplyMultiplierToBiscuitValue(e.newGameStage.stageBiscuitMultiplier);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag(Player_Logic.PLAYER_TAG)) {
            // Add 1 score to the current score of the player
            GameManager.Instance.AddScore(currentBiscuitPickupValue);

            // Destroy the collected biscuit
            Destroy(gameObject);
        }
    }

    private void ApplyMultiplierToBiscuitValue(int multiplier) {
        currentBiscuitPickupValue *= multiplier;
    }
}