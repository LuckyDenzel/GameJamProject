using UnityEngine;

public class Player_Logic : MonoBehaviour {

    public const string PLAYER_TAG = "Player";

    public static Player_Logic Instance { get; private set; }
    public static Player_Health PlayerHealthInstance { get; private set; }
    public static Player_Movement PlayerMovementInstance { get; private set; }
    public static Player_Combat PlayerCombatIntance { get; private set; }

    [SerializeField] private Player_Health playerHealthReference;
    [SerializeField] private Player_Movement playerMovementReference;
    [SerializeField] private Player_Combat playerCombatReference;

    private void Awake() {
        Instance = this;

        PlayerHealthInstance = playerHealthReference;
        PlayerMovementInstance = playerMovementReference;
        PlayerCombatIntance = playerCombatReference;
    }
}