using System.Collections;
using UnityEngine;

public class Enemy_Logic : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Rigidbody2D enemyRb;

    [Header("Values")]
    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("The minimum distance the enemy needs to be from the player to be able to move closer to the player.")]
    [SerializeField] private float minDistanceFromPlayerBeforeMove = 2f;

    [Header("Player related")]
    [Tooltip("The force of the knockback the enemy deals to the player on collision.")]
    [SerializeField] private float playerCollisionForceAmount = 20f;

    [Tooltip("The damage the enemy deals to the player at the beginning of the run (Later in run gets heavier).")]
    [SerializeField] private int startDamage = 10;

    private Vector2 currentPlayerPosition;

    private float playerPositionCheckTimer = 0.2f;
    private float currentDamageAmount;

    private bool isFacingRight;
    private bool previousIsFacingRight;

    private void Start() {
        GameStageManager.Instance.OnStageChanged += GameStageHandler_OnStageChanged;

        currentDamageAmount = startDamage;

        previousIsFacingRight = isFacingRight;
    }

    // Listen to the OnStageChanged event to apply the current stage's multiplier to the current damage
    private void GameStageHandler_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        ApplyMultiplierToCurrentDamageAmount(e.newGameStage.stageThreatsDamageMultiplier);
    }

    private void Update() {
        playerPositionCheckTimer -= Time.deltaTime;

        // Update the target transform 5 times per second
        if (playerPositionCheckTimer <= 0f) {
            playerPositionCheckTimer = 0.2f;

            currentPlayerPosition = Player_Logic.Instance.transform.position;
        }
    }

    private void FixedUpdate() {
        MoveToPlayer();
        ClampVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Add a bounce back force to the player when the enemy hits the player
        if (collision.transform.CompareTag(Player_Logic.PLAYER_TAG)) {
            // Call the ApplyKnockbackToPlayer method on the player movement class
            Player_Logic.PlayerMovementInstance.ApplyKnockbackToPlayer();

            // Deal damage to the player
            DealDamageToPlayer(Player_Logic.PlayerHealthInstance);

            // Calculate the force direction (pushing the player away from the enemy
            Vector2 forceDirection = (transform.position - collision.transform.position).normalized * playerCollisionForceAmount;

            // Apply the calculated force direction to the rigidbody of the player
            collision.transform.GetComponent<Rigidbody2D>().AddForce(-forceDirection, ForceMode2D.Impulse);
        }
    }

    private void MoveToPlayer() {
        // Calculate the direction of where the player is with Mathf.Sign which returns 0, -, or +.
        float forceDirection = Mathf.Sign(currentPlayerPosition.x - transform.position.x) * moveSpeed;

        enemyRb.AddForceX(forceDirection, ForceMode2D.Force);

        if (forceDirection > 0) { // The enemy is moving right
            isFacingRight = true;
        } else if (forceDirection < 0) { // The enemy is moving left
            isFacingRight = false;
        }

        // Flip only when the direction changes
        if (isFacingRight != previousIsFacingRight) {
            TurnAround();
            previousIsFacingRight = isFacingRight;
        }
    }

    private void TurnAround() {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    private void DealDamageToPlayer(Player_Health playerHealthScript) {
        playerHealthScript.ApplyDamage(Mathf.RoundToInt(currentDamageAmount));
    }

    private void ApplyMultiplierToCurrentDamageAmount(float multiplier) {
        currentDamageAmount *= multiplier;
    }

    private void ClampVelocity() {
        if (enemyRb.linearVelocityX > moveSpeed) {
            enemyRb.linearVelocityX = moveSpeed;
        } else if (enemyRb.linearVelocityX < -moveSpeed) {
            enemyRb.linearVelocityX = -moveSpeed;
        }
    }
}