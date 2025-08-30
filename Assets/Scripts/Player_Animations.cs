using UnityEngine;

public class Player_Animations : MonoBehaviour {


    private const string PLAYER_IDLE_ANIMATION_STATE_BOOL = "isIdle";
    private const string PLAYER_WALKING_ANIMATION_STATE_BOOL = "isWalking";
    private const string PLAYER_RUNNING_ANIMATION_STATE_BOOL = "isRunning";


    [SerializeField] private Animator swordAnimator;
    [SerializeField] private AnimationClip playerIdleAnimationClip;
    [SerializeField] private AnimationClip playerWalkingAnimationClip;
    [SerializeField] private AnimationClip playerRunningAnimationClip;

    private Player_Movement playerMovement;

    private bool isIdle, isWalking, isRunning;

    private bool wasIdle, wasWalking, wasRunning;


    private void Awake() {
        playerMovement = GetComponent<Player_Movement>();
    }

    private void Update() {
        isIdle = playerMovement.GetPlayerMovementState() == Player_Movement.MovementState.Idle;
        isWalking = playerMovement.GetPlayerMovementState() == Player_Movement.MovementState.Walking;
        isRunning = playerMovement.GetPlayerMovementState() == Player_Movement.MovementState.Running;

        if (wasIdle != isIdle) {
            swordAnimator.SetBool(PLAYER_IDLE_ANIMATION_STATE_BOOL, isIdle);

            wasIdle = isIdle;
        }

        if (wasWalking != isWalking) {
            swordAnimator.SetBool(PLAYER_WALKING_ANIMATION_STATE_BOOL, isWalking);

            wasWalking = isWalking;
        }

        if (wasRunning != isRunning) {
            swordAnimator.SetBool(PLAYER_RUNNING_ANIMATION_STATE_BOOL, isRunning);

            wasRunning = isRunning;
        }
    }
}