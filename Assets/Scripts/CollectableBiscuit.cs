using UnityEngine;

public class CollectableBiscuit : MonoBehaviour {


    [SerializeField] private int startBiscuitPickupValue = 1;

    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private Collider2D collisionCollider;

    private Rigidbody2D biscuitRb;

    private int currentBiscuitPickupValue;


    private void Awake() {
        biscuitRb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        if (GameStageManager.Instance != null) {
            GameStageManager.Instance.OnStageChanged += GameStageHandler_OnStageChanged;
        }

        currentBiscuitPickupValue = startBiscuitPickupValue;
    }

    private void GameStageHandler_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        ApplyMultiplierToBiscuitValue(e.newGameStage.stageBiscuitMultiplier);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag(GameManager.GROUND_TAG)) {
            biscuitRb.bodyType = RigidbodyType2D.Kinematic;

            collisionCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag(Player_Logic.PLAYER_TAG)) {
            // Add 1 score to the current score of the player
            GameManager.Instance.AddBiscuitToScore(currentBiscuitPickupValue);

            // Destroy the collected biscuit
            Destroy(gameObject);
        }
    }

    private void ApplyMultiplierToBiscuitValue(int multiplier) {
        currentBiscuitPickupValue *= multiplier;
    }
}