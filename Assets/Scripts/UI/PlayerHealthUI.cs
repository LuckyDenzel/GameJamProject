using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI healthTextReference;

    private void Start() {
        Player_Logic.PlayerHealthInstance.OnHealthChanged += PlayerHealth_OnHealthChanged;

        Show();

        // Get the starting amount health to update the text to
        UpdateHealthText(Player_Logic.PlayerHealthInstance.GetCurrentHealthAmount());
    }

    private void PlayerHealth_OnHealthChanged(object sender, Player_Health.OnHealthChangedEventArgs e) {
        UpdateHealthText(e.newHealthAmount);
    }

    private void UpdateHealthText(int healthAmount) {
        healthTextReference.text = string.Empty;

        healthTextReference.text = healthAmount.ToString();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}