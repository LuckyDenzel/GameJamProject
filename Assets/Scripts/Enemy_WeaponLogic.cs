using UnityEngine;

public class Enemy_WeaponLogic : MonoBehaviour {


    [SerializeField] private Weapon weaponReference;


    private void Update() {
        HandleShooting();
    }

    private void HandleShooting() {
        if (weaponReference.CanShoot) {
            weaponReference.Shoot();
        } else if (weaponReference.CurrentAmmoInMagazine == 0) {
            weaponReference.Reload();

            Debug.Log("Enemy reload");
        }
    }

}