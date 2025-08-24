using UnityEngine;

public class CollectableBiscuit : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag(Player_Logic.PLAYER_TAG)) {
            // Add 1 score to the current score of the player
            GameManager.Instance.AddScore(1);

            // Destroy the collected biscuit
            Destroy(gameObject);
        }
    }
}