using System;
using UnityEngine;

public class Player_Health : MonoBehaviour, IHealth {


    public const string PLAYER_MAX_HEALTH_PLAYER_PREFS = "SavedPlayerMaxHealthPlayerPrefs";
    public const string PLAYER_STARTING_HEALTH_PLAYER_PREFS = "SavedPlayerStartingHealthPlayerPrefs";

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

        // Add a safety check to make sure the starting health isn't above the max health
        if (CurrentHealth > maxHealth) {
            Debug.LogError("The current health of the player is higher than the max possible health!");
        }
    }

    private void Start() {
        maxHealth = PlayerPrefs.GetInt(PLAYER_MAX_HEALTH_PLAYER_PREFS, maxHealth);
        startingHealth = PlayerPrefs.GetInt(PLAYER_STARTING_HEALTH_PLAYER_PREFS, startingHealth);

        CurrentHealth = startingHealth;

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs(CurrentHealth));
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

    /// <summary>
    /// Saves the player max health in player prefs.
    /// </summary>
    public void IncreasePlayerMaxHealthSaved(int increaseAmount) {
        maxHealth += increaseAmount;

        PlayerPrefs.SetInt(PLAYER_MAX_HEALTH_PLAYER_PREFS, maxHealth);
    }
    
    public void IncreasePlayerStartingHealthSaved(int increaseAmount) {
        startingHealth += increaseAmount;

        PlayerPrefs.SetInt(PLAYER_STARTING_HEALTH_PLAYER_PREFS, startingHealth);
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