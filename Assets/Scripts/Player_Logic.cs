using UnityEngine;

public class Player_Logic : MonoBehaviour {

    public const string PLAYER_TAG = "Player";

    public static Player_Logic Instance { get; private set; }
    public static Player_Health PlayerHealthInstance { get; private set; }

    [SerializeField] private Player_Health playerHealthReference;

    private void Awake() {
        Instance = this;

        PlayerHealthInstance = playerHealthReference;
    }
}