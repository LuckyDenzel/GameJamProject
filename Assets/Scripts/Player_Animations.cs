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


    private void Awake() {
        playerMovement = GetComponent<Player_Movement>();
    }

    private void Update() {
        if (playerMovement.GetPlayerMovementState() == Player_Movement.MovementState.Idle) { 
            swordAnimator.SetBool(PLAYER_IDLE_ANIMATION_STATE_BOOL, true); 

            swordAnimator.SetBool(PLAYER_WALKING_ANIMATION_STATE_BOOL, false); 
            swordAnimator.SetBool(PLAYER_RUNNING_ANIMATION_STATE_BOOL, false); 
        } else if (playerMovement.GetPlayerMovementState() == Player_Movement.MovementState.Walking) {
            swordAnimator.SetBool(PLAYER_WALKING_ANIMATION_STATE_BOOL, true);

            swordAnimator.SetBool(PLAYER_IDLE_ANIMATION_STATE_BOOL, false); 
            swordAnimator.SetBool(PLAYER_RUNNING_ANIMATION_STATE_BOOL, false); 
        } else if (playerMovement.GetPlayerMovementState() == Player_Movement.MovementState.Running) {
            swordAnimator.SetBool(PLAYER_RUNNING_ANIMATION_STATE_BOOL, true);

            swordAnimator.SetBool(PLAYER_IDLE_ANIMATION_STATE_BOOL, false); 
            swordAnimator.SetBool(PLAYER_WALKING_ANIMATION_STATE_BOOL, false); 
        } else if (playerMovement.GetPlayerMovementState() == Player_Movement.MovementState.Air) {
            swordAnimator.SetBool(PLAYER_IDLE_ANIMATION_STATE_BOOL, false);
            swordAnimator.SetBool(PLAYER_WALKING_ANIMATION_STATE_BOOL, false);
            swordAnimator.SetBool(PLAYER_RUNNING_ANIMATION_STATE_BOOL, false);
        }
    }
}