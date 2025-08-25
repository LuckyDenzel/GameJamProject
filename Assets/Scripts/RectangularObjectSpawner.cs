using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RectangularObjectSpawner : MonoBehaviour {

    // Each element contains 1 object
    [SerializeField] private List<SpawnableObject> objectsToSpawnList;

    private float spawnDelayTimer;

    private BoxCollider2D boxCollider2D;

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        spawnDelayTimer -= Time.deltaTime;

        if (spawnDelayTimer <= 0f) {
            SpawnRandomObjectInRandomBounds();
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

    public SpawnableObject(GameObject gameObject, float spawnDelay) {
        this.gameObject = gameObject;
        this.spawnDelay = spawnDelay;
    }

    public void AddMultiplierToSpawnDelay(float multiplier) {
        spawnDelay *= multiplier;
    }
}