using UnityEngine;

public class UpgradesManager : MonoBehaviour {


    public static UpgradesManager Instance { get; private set; }


    private void Awake() {
        Instance = this;
    }

    public void UpgradePlayerMaxHealth(int amount) {
        Player_Logic.PlayerHealthInstance.IncreasePlayerMaxHealthSaved(amount);
    }

    public void UpgradePlayerStartingHealth(int amount) {
        Player_Logic.PlayerHealthInstance.IncreasePlayerStartingHealthSaved(amount);
    }
}