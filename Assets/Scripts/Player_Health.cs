using System;
using UnityEngine;

public class Player_Health : MonoBehaviour {


    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs {
        public int newHealthAmount;

        public OnHealthChangedEventArgs(int newHealthAmount) {
            this.newHealthAmount = newHealthAmount;
        }
    }

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int startingHealth = 100;

    private int currentHealth;


    private void OnValidate() {
        if (startingHealth > maxHealth) {
            Debug.LogWarning("The set starting health of the player is higher than the max health!");
        }
    }

    private void Awake() {
        currentHealth = startingHealth;

        // Add a safety check to make sure the starting health isn't above the max health
        if (currentHealth > maxHealth) {
            Debug.LogError("The current health of the player is higher than the max possible health!");
        }
    }

    public void AddHealth(int amount) {
        if (currentHealth > 0) {
            currentHealth += amount;

            OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs(currentHealth));
        } else {
            Debug.LogError("The player is already dead, can't add health!");
        }
    }

    public void DealDamage(int amount) {
        currentHealth -= amount;

        if (currentHealth <= 0) {
            Die();
        }

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs(currentHealth));
    }

    private void Die() {
        // Set the health back to zero for a cleaner look
        currentHealth = 0;

        Debug.Log("Player is dead!");
    }

    public int GetCurrentHealthAmount() {
        return currentHealth;
    }
}