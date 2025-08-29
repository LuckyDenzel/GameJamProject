using UnityEngine;

public class Player_Movement : MonoBehaviour {


    private const string PLAYER_MOVE_SPEED_PLAYER_PREFS = "SavedPlayerMoveSpeedPlayerPrefs";

    [Header("References")]
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Values")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] movementClipsAudioClipArray;

    [SerializeField] private LayerMask groundLayerMask;

    private float playerHeight = 1f;

    // Timers
    private float stunnedTimer;
    private float stunnedDuration = 0.3f;

    private bool isStunned = false;

    // Other
    private bool isFacingRight;
    private bool isMoving;

    private bool grounded;
    private bool canJump = true;

    // Field
    public bool IsFacingRight => isFacingRight;


    private void Start() {
        moveSpeed = PlayerPrefs.GetFloat(PLAYER_MOVE_SPEED_PLAYER_PREFS, moveSpeed);
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

            return;
        }
        if (Input.GetKey(KeyCode.D)) {
            Vector2 forceDirection = new Vector2(moveSpeed, 0);
            playerRb.AddForce(forceDirection, ForceMode2D.Force);

            isFacingRight = true;
            isMoving = true;

            return;
        }

        isMoving = false;
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

    private void ClampVelocity() {
        if (playerRb.linearVelocityX > moveSpeed) {
            playerRb.linearVelocityX = moveSpeed;
        }
        else if (playerRb.linearVelocityX < -moveSpeed) {
            playerRb.linearVelocityX = -moveSpeed;
        }
    }
}