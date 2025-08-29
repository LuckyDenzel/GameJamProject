using UnityEngine;

public class UpgradesManager : MonoBehaviour {


    public static UpgradesManager Instance { get; private set; }


    private void Awake() {
        Instance = this;
    }

    // Player related
    public void UpgradePlayerMaxHealth(int amount) {
        Player_Logic.PlayerHealthInstance.IncreasePlayerMaxHealthSaved(amount);
    }

    public void UpgradePlayerStartingHealth(int amount) {
        Player_Logic.PlayerHealthInstance.IncreasePlayerStartingHealthSaved(amount);
    }

    public void UpgradePlayerMoveSpeed(float amount) {

    }


    public void UpgradeRunExitTimeDelay(float amount) {
        GameManager.Instance.DecreaseRunExitTimeDelay(amount);
    }
}