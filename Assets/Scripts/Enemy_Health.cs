using UnityEngine;

public class Enemy_Health : MonoBehaviour {


    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int startingHealth = 100;

    private int currentHealth;

    private void Start() {
        currentHealth = startingHealth;
    }

    public void AddHealth(int amount) {
        if (currentHealth > 0) {
            currentHealth += amount;
        } else {
            Debug.LogError("You're trying to add health to an enemy, but the enemy is already dead. This should never happen!", this);
        }
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        Debug.Log($"Enemy took damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        // Set the health to 0 to make it non negative
        currentHealth = 0;

        // For now destroy the enemy, later use a object pool
        Destroy(gameObject);
    }
}