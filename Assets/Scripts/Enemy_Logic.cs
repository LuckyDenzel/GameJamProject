using UnityEngine;

public class Enemy_Logic : MonoBehaviour {


    [SerializeField] private Rigidbody2D enemyRb;

    [SerializeField] private float moveSpeed = 2f;

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

    private void ClampVelocity() {
        if (enemyRb.linearVelocityX > moveSpeed) {
            enemyRb.linearVelocityX = moveSpeed;
        }
        else if (enemyRb.linearVelocityX < -moveSpeed) {
            enemyRb.linearVelocityX = -moveSpeed;
        }
    }
}