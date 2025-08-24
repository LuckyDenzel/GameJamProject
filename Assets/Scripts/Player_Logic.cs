using UnityEngine;

public class Player_Logic : MonoBehaviour {

    public const string PLAYER_TAG = "Player";

    public static Player_Logic Instance { get; private set; }


    private void Awake() {
        Instance = this;
    }
}