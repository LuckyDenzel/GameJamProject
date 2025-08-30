using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour {


    [SerializeField] private Enemy_Health enemyHealth;
    [SerializeField] private Image enemyHealthImage;
    [SerializeField] private Image enemyHealthBackgroundImage;


    private void Start() {
        enemyHealth.OnHealthChanged += EnemyHealth_OnHealthChanged;

        enemyHealthImage.fillAmount = 100;
        enemyHealthImage.gameObject.SetActive(false);
        enemyHealthBackgroundImage.gameObject.SetActive(false);
    }

    private void EnemyHealth_OnHealthChanged(object sender, Enemy_Health.OnHealthChangedEventArgs e) {
        float maxValue = 100;
        float normalizedHealth = e.newHealth / maxValue;

        enemyHealthImage.fillAmount = normalizedHealth;

        enemyHealthImage.gameObject.SetActive(true);
        enemyHealthBackgroundImage.gameObject.SetActive(true);
    }
}