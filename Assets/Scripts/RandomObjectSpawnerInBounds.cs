using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawnerInBounds : MonoBehaviour {

    // Each element contains 1 object
    [SerializeField] private List<SpawnableObject> objectsToSpawnList;

    private float spawnDelayTimer;

    private BoxCollider2D boxCollider2D;

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        GameStageManager.Instance.OnStageChanged += GameStageHandler_OnStageChanged;
    }

    // Increase the multiplier on the values of the objects
    private void GameStageHandler_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        foreach (var obj in objectsToSpawnList) {
            obj.AddMultiplierToSpawnDelay(e.newGameStage.stageThreatsDamageMultiplier);
        }
    }

    private void Update() {
        foreach (var obj in objectsToSpawnList) {
            obj.currentTimer -= Time.deltaTime;

            if (obj.currentTimer <= 0) {
                Instantiate(obj.gameObject, GetRandomPointInSpawner(), Quaternion.identity);
                obj.currentTimer = obj.spawnDelay;
            }
        }
    }

    private void SpawnRandomObjectInRandomBounds() {
        int randomObjectIndex = Random.Range(0, objectsToSpawnList.Count);

        SpawnableObject randomGameObject = objectsToSpawnList[randomObjectIndex];

        // For now Instantiate the object, later I can use a pooler
        Instantiate(randomGameObject.gameObject, GetRandomPointInSpawner(), Quaternion.identity); // For now rotation 0

        spawnDelayTimer = randomGameObject.spawnDelay;
    }

    public Vector2 GetRandomPointInSpawner() {
        Bounds bounds = boxCollider2D.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    } 

}

[System.Serializable]
public class SpawnableObject {
    public GameObject gameObject;
    public float spawnDelay;

    [HideInInspector] public float currentTimer;

    public SpawnableObject(GameObject gameObject, float spawnDelay) {
        this.gameObject = gameObject;
        this.spawnDelay = spawnDelay;
        currentTimer = spawnDelay;
    }

    public void AddMultiplierToSpawnDelay(float multiplier) {
        spawnDelay *= multiplier;
        currentTimer = spawnDelay;
    }
}