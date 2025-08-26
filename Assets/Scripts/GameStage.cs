using UnityEngine;

[System.Serializable]
public class GameStage {


    public float stageDuration = 30f;

    public float stageThreatsDamageMultiplier = 1f;
    public int stageBiscuitMultiplier = 1;

    public GameStage(float stageDuration, float stageThreatsDamageMultiplier) {
        this.stageDuration = stageDuration;
        this.stageThreatsDamageMultiplier = stageThreatsDamageMultiplier;
    }
}