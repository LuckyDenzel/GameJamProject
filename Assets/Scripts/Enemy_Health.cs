using System;
using UnityEngine;

public class Enemy_Health : MonoBehaviour, IHealth {


    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs {
        public int newHealth;

        public OnHealthChangedEventArgs(int newHealth) {
            this.newHealth = newHealth;
        }
    }

    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int startingHealth = 100;

    public int CurrentHealth { get; private set; }

    private void Start() {
        CurrentHealth = startingHealth;
    }

    public void AddHealth(int amount) {
        if (CurrentHealth > 0 && CurrentHealth + amount <= maxHealth) {
            CurrentHealth += amount;

            OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs(CurrentHealth));
        } else {
            Debug.LogError("You're trying to add health to an enemy, but the enemy is already dead. This should never happen!", this);
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
        // Set the health to 0 to make it non negative
        CurrentHealth = 0;

        // For now destroy the enemy, later use a object pool
        Destroy(gameObject);
    }
}