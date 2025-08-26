using System;
using UnityEngine;

public class Player_Health : MonoBehaviour, IHealth {


    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs {
        public int newHealthAmount;

        public OnHealthChangedEventArgs(int newHealthAmount) {
            this.newHealthAmount = newHealthAmount;
        }
    }

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int startingHealth = 100;

    public int CurrentHealth { get; private set; }


    private void OnValidate() {
        if (startingHealth > maxHealth) {
            Debug.LogWarning("The set starting health of the player is higher than the max health!");
        }
    }

    private void Awake() {
        CurrentHealth = startingHealth;

        // Add a safety check to make sure the starting health isn't above the max health
        if (CurrentHealth > maxHealth) {
            Debug.LogError("The current health of the player is higher than the max possible health!");
        }
    }

    public void AddHealth(int amount) {
        if (CurrentHealth > 0) {
            CurrentHealth += amount;

            OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs(CurrentHealth));
        } else {
            Debug.LogError("The player is already dead, can't add health!");
        }
    }

    public void ApplyDamage(int amount) {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0) {
            Die();
        }

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs(CurrentHealth));
    }

    private void Die() {
        // Set the health back to zero for a cleaner look
        CurrentHealth = 0;

        GameManager.Instance.EndGameFailed();
    }

    public int GetCurrentHealthAmount() {
        return CurrentHealth;
    }
}