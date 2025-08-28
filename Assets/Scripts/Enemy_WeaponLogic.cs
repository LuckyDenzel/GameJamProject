using UnityEngine;

public class Enemy_WeaponLogic : MonoBehaviour {


    [SerializeField] private Weapon weaponReference;

    private Enemy_Logic enemyLogic;


    private void Awake() {
        enemyLogic = GetComponent<Enemy_Logic>();
    }

    private void Update() {
        HandleShooting();
    }

    private void HandleShooting() {
        if (IsPlayerInRange() && weaponReference.CanShoot) {
            weaponReference.Shoot();
        }

        if (weaponReference.CurrentAmmoInMagazine == 0) {
            weaponReference.Reload();
        }
    }

    private bool IsPlayerInRange() {
        if (Physics2D.Raycast(weaponReference.MuzzleTransform.position, enemyLogic.IsFacingRight() ? Vector2.right : Vector2.left, 
            weaponReference.WeaponRange, GameManager.Instance.GetPlayerLayerMask())) {

            return true;
        }

        return false;
    }

}