using System.Collections;
using UnityEngine;

public class Enemy_Logic : MonoBehaviour {


    [SerializeField] private Rigidbody2D enemyRb;

    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("The minimum distance the enemy needs to be from the player to be able to move closer to the player")]
    [SerializeField] private float minDistanceFromPlayerBeforeMove = 2f;

    [SerializeField] private float playerCollisionForceAmount = 20f;

    private Vector2 currentPlayerPosition;

    private float playerPositionCheckTimer = 0.2f;

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

    private void MoveToPlayer() {
        // Calculate the direction of where the player is.
        Vector2 forceDirection = (currentPlayerPosition - (Vector2)transform.position).normalized * moveSpeed;

        enemyRb.AddForce(forceDirection, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Add a bounce back force to the player when the enemy hits the player
        if (collision.transform.CompareTag(Player_Logic.PLAYER_TAG)) {
            // Get the player movement component to call the ApplyKnockbackToPlayer method
            Player_Movement playerMovement = collision.transform.GetComponent<Player_Movement>();
            playerMovement.ApplyKnockbackToPlayer();

            // Calculate the force direction (pushing the player away from the enemy
            Vector2 forceDirection = (transform.position - collision.transform.position).normalized * playerCollisionForceAmount;

            // Apply the calculated force direction to the rigidbody of the player
            collision.transform.GetComponent<Rigidbody2D>().AddForce(-forceDirection, ForceMode2D.Impulse);
        }
    }

    private void ClampVelocity() {
        if (enemyRb.linearVelocityX > moveSpeed) {
            enemyRb.linearVelocityX = moveSpeed;
        } else if (enemyRb.linearVelocityX < -moveSpeed) {
            enemyRb.linearVelocityX = -moveSpeed;
        }
    }
}