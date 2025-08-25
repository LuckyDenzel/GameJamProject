using UnityEngine;

public class Player_Movement : MonoBehaviour {


    [SerializeField] private Rigidbody2D playerRb;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;


    [SerializeField] private LayerMask groundLayerMask;

    private float playerHeight = 1f;

    // Timers
    private float knockBackTimer;
    private float knockBackDuration = 0.2f;

    private bool isBeingKnockedBack = false;

    // Other
    private bool isFacingRight;

    private bool grounded;

    // Field
    public bool IsFacingRight => isFacingRight;


    private void Update() {


        if (isBeingKnockedBack) {
            knockBackTimer -= Time.deltaTime;

            if (knockBackTimer <= 0f) {
                isBeingKnockedBack = false;
            }

            // Disable jumping until the knockback is over
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        grounded = IsGrounded();
    }

    private void FixedUpdate() {
        if (!isBeingKnockedBack) {
            Move();
            ClampVelocity();
        }
    }

    public bool IsGrounded() {
        float playerHeight = this.playerHeight + 0.5f;
        if (Physics2D.Raycast(transform.position, Vector3.down, playerHeight, groundLayerMask)) {
            return true;
        }

        return false;
    }

    private void Move() {
        if (Input.GetKey(KeyCode.A)) {
            Vector2 forceDirection = new Vector2(-moveSpeed, 0);
            playerRb.AddForce(forceDirection, ForceMode2D.Force);

            isFacingRight = false;
        }
        if (Input.GetKey(KeyCode.D)) {
            Vector2 forceDirection = new Vector2(moveSpeed, 0);
            playerRb.AddForce(forceDirection, ForceMode2D.Force);

            isFacingRight = true;
        }
    }

    private void Jump() {
        if (grounded) {
            Vector2 jumpForceDirection = new Vector2(0, jumpForce);
            playerRb.AddForce(jumpForceDirection, ForceMode2D.Impulse);
        } else {
            Debug.Log("Not grounded");
        }
    }

    public void ApplyKnockbackToPlayer() {
        isBeingKnockedBack = true;
        knockBackTimer = knockBackDuration;
    }

    private void ClampVelocity() {
        if (playerRb.linearVelocityX > moveSpeed) {
            playerRb.linearVelocityX = moveSpeed;
        }
        else if (playerRb.linearVelocityX < -moveSpeed) {
            playerRb.linearVelocityX = -moveSpeed;
        }
    }
}