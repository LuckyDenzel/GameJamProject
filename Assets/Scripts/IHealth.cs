using UnityEngine;

public interface IHealth {


    public int CurrentHealth { get; }

    public void AddHealth(int amount);

    public void ApplyDamage(int amount);

    public int GetCurrentHealth() {
        return CurrentHealth;
    }
}