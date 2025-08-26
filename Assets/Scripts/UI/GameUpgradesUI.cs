using UnityEngine;

public class GameUpgradesUI : MonoBehaviour {

    private void Start() {
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}