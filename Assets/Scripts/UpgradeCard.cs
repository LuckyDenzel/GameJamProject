using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour {

    private string GetPlayerPrefsKey => $"UpgradeCard_Unlocked_{upgradeID}";

    [Header("Upgrade")]
    [SerializeField] private UnityEvent OnUpgrade;

    [Tooltip("Deschribes the keyword for the upgrade")]
    [SerializeField] private string upgradeID;

    [Header("Cost")]
    [SerializeField] private Currency neededCurrency;
    [SerializeField] private int upgradeCost = 10;

    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Upgrade Stages")]
    [SerializeField] private int upgradeStagesAmount = 1;

    [SerializeField] private Transform[] cardToRevealAfterUpgradeTransformsArray;

    private Button cardButton;

    private bool isUnlocked;

    private enum Currency {
        Biscuit,
        BloodyBiscuit
    }

    private void Awake() {
        cardButton = GetComponent<Button>();

        if (GetPlayerPrefsKey == string.Empty) Debug.LogError("No UpgradeID was entered!", this);
        isUnlocked = PlayerPrefs.GetInt(GetPlayerPrefsKey, 0) == 1;

        if (!isUnlocked) {
            currencyText.text = upgradeCost.ToString();
        }

        if (cardToRevealAfterUpgradeTransformsArray != null) {
            foreach (var card in cardToRevealAfterUpgradeTransformsArray) {
                card.gameObject.SetActive(isUnlocked);
            }
        }
        
        cardButton.onClick.AddListener(() => {
            if (neededCurrency == Currency.Biscuit) {
                if (GameManager.Instance.GetTotalBiscuitsScore() >= upgradeCost) {
                    UnlockUpgrade();
                } else {
                    Debug.Log("Not enough currency!");
                }
            } else if (neededCurrency == Currency.BloodyBiscuit) {
                // For now pints, later bloody cookies
                if (GameManager.Instance.GetTotalPintsScore() >= upgradeCost) {
                    UnlockUpgrade();
                } else {
                    Debug.Log("Not enough currency!");
                }
            }
        });
    }

    private void UnlockUpgrade() {
        if (!isUnlocked) {
            GameManager.Instance.SpendTotalBiscuits(upgradeCost);

            OnUpgrade.Invoke();

            isUnlocked = true;

            PlayerPrefs.SetInt(GetPlayerPrefsKey, isUnlocked ? 1 : 0);

            if (cardToRevealAfterUpgradeTransformsArray != null) {
                foreach (var card in cardToRevealAfterUpgradeTransformsArray) {
                    card.gameObject.SetActive(true);
                }
            }

            Debug.Log("Unlocked!");
        } else {
            Debug.Log("Already unlocked!");
        }
    }
}