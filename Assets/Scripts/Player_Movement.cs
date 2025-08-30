using UnityEngine;

public class Player_Movement : MonoBehaviour {


    private const string PLAYER_MOVE_SPEED_PLAYER_PREFS = "SavedPlayerMoveSpeedPlayerPrefs";
    private const string PLAYER_STUNNED_PLAYER_PREFS = "SavedPlayerStunnedPlayerPrefs";

    [Header("References")]
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Values")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] movementClipsAudioClipArray;

    [SerializeField] private LayerMask groundLayerMask;

    private MovementState movementState;

    private float playerHeight = 1f;

    // Timers
    private float stunnedTimer;
    private float stunnedDuration = 0.3f;

    private bool isStunned = false;

    // Other
    private bool isFacingRight;
    private bool previousIsFacingRight;

    private bool isMoving;

    private bool grounded;
    private bool canJump = true;

    // Field
    public bool IsFacingRight => isFacingRight;

    public enum MovementState {
        Idle,
        Walking,
        Running,
        Air
    }

    private void Start() {
        moveSpeed = PlayerPrefs.GetFloat(PLAYER_MOVE_SPEED_PLAYER_PREFS, moveSpeed);
        stunnedDuration = PlayerPrefs.GetFloat(PLAYER_STUNNED_PLAYER_PREFS, stunnedDuration);

        previousIsFacingRight = isFacingRight;
    }

    private void Update() {
        if (isStunned) {
            stunnedTimer -= Time.deltaTime;

            if (stunnedTimer <= 0f) {
                isStunned = false;
            }

            // Disable jumping until the knockback is over
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        grounded = IsGrounded();

        HandleSound();
        HandleMovementState();
    }

    private void FixedUpdate() {
        if (!isStunned) {
            Move();
            ClampVelocity();
        }
    }

    public bool IsGrounded() {
        float playerHeight = this.playerHeight;
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
            isMoving = true;

            // Flip only when the direction changes
            if (isFacingRight != previousIsFacingRight) {
                TurnAround();
                previousIsFacingRight = isFacingRight;
            }

            return;
        }
        if (Input.GetKey(KeyCode.D)) {
            Vector2 forceDirection = new Vector2(moveSpeed, 0);
            playerRb.AddForce(forceDirection, ForceMode2D.Force);

            isFacingRight = true;
            isMoving = true;

            // Flip only when the direction changes
            if (isFacingRight != previousIsFacingRight) {
                TurnAround();
                previousIsFacingRight = isFacingRight;
            }

            return;
        }

        isMoving = false;
    }

    private void HandleSound() {
        if (Time.timeScale == 0) return;

        if (grounded && playerRb.linearVelocity != Vector2.zero) { // Check if the player is grounded and not standing still
            if (!audioSource.isPlaying && !audioSource.loop) { // Check if the current audio source is playing anything else
                int randomAudioClipIndex = Random.Range(0, movementClipsAudioClipArray.Length);

                // Assign the random audio clip to the audio source
                audioSource.clip = movementClipsAudioClipArray[randomAudioClipIndex];
                audioSource.Play();
            }

            // Adjust the pitch based on the movement speed of the player
            if (playerRb.linearVelocityX >= moveSpeed && isMoving || playerRb.linearVelocityX <= -moveSpeed && isMoving) {
                audioSource.pitch = 2f;
            } else {
                audioSource.pitch = 1f;
            }
        }
    }

    private void HandleMovementState() {
        if (playerRb.linearVelocityX == 0f && IsGrounded()) movementState = MovementState.Idle;
        else if (playerRb.linearVelocityX >= moveSpeed && IsGrounded()|| playerRb.linearVelocityX <= -moveSpeed && IsGrounded()) movementState = MovementState.Running;
        else if (playerRb.linearVelocityX < moveSpeed && IsGrounded() || playerRb.linearVelocityX > -moveSpeed && IsGrounded()) movementState = MovementState.Walking;
        else movementState = MovementState.Air;
    }

    private void TurnAround() {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Jump() {
        if (grounded && canJump) {
            canJump = false;

            Vector2 jumpForceDirection = new Vector2(0, jumpForce);
            playerRb.AddForce(jumpForceDirection, ForceMode2D.Impulse);

            Invoke(nameof(ResetJump), 0.3f);
        }
    }

    private void ResetJump() {
        canJump = true;
    }

    public void StunPlayer() {
        isStunned = true;
        stunnedTimer = stunnedDuration;
    }

    public void IncreasePlayerMoveSpeed(float amount) {
        moveSpeed += amount;

        PlayerPrefs.SetFloat(PLAYER_MOVE_SPEED_PLAYER_PREFS, moveSpeed);
    }

    public void SetNewStunDuration(float newDuration) {
        stunnedDuration = newDuration;

        PlayerPrefs.SetFloat(PLAYER_STUNNED_PLAYER_PREFS, stunnedDuration);
    }

    private void ClampVelocity() {
        if (playerRb.linearVelocityX > moveSpeed) {
            playerRb.linearVelocityX = moveSpeed;
        }
        else if (playerRb.linearVelocityX < -moveSpeed) {
            playerRb.linearVelocityX = -moveSpeed;
        }
    }

    public MovementState GetPlayerMovementState() {
        return movementState;
    }
}