using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RectangularObjectSpawner : MonoBehaviour {

    // Each element contains 1 object
    [SerializeField] private List<SpawnableObject> objectsToSpawnList;

    [SerializeField] private float startingSpawnDelay = 15f;

    private float currentSpawnDelay;
    private float spawnDelayTimer;

    private BoxCollider2D boxCollider2D;

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        currentSpawnDelay = startingSpawnDelay;
    }

    private void Update() {
        spawnDelayTimer -= Time.deltaTime;

        if (spawnDelayTimer <= 0f) {
            spawnDelayTimer = currentSpawnDelay;

            SpawnRandomObjectInRandomBounds();
        }
    }

    private void SpawnRandomObjectInRandomBounds() {
        int randomObjectIndex = Random.Range(0, objectsToSpawnList.Count);

        GameObject randomGameObject = objectsToSpawnList[randomObjectIndex].gameObject;

        // For now Instantiate the object, later I can use a pooler
        Instantiate(randomGameObject, GetRandomPointInSpawner(), Quaternion.identity); // For now rotation 0
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

    public SpawnableObject(GameObject gameObject) {
        this.gameObject = gameObject;
    }
}