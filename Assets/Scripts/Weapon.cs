using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private Transform ownerTransform;

    [Header("Properties")]
    [SerializeField] private float fireRate = 1f;

    [SerializeField] private float range = 20f;

    [SerializeField] private float damage = 10f;

    [SerializeField] private int magazineSize = 20;
    [SerializeField] private int startingTotalAmmoAmount = 60;

    [SerializeField] private float reloadDuration = 2f;

    private int currentTotalAmmoAmount;
    private int currentAmmoInMagazine;

    private bool canReload;

    [Header("VFX")]
    [SerializeField] private GameObject muzzleFlashGameObject;
    [SerializeField] private GameObject bulletProjectileGameObject;

    private bool canShoot = true;

    // Fields
    public int CurrentAmmoInMagazine => currentAmmoInMagazine;
    public bool CanShoot => canShoot;

    private void Start() {
        currentTotalAmmoAmount = startingTotalAmmoAmount;
        currentAmmoInMagazine = magazineSize;
    }

    public void Shoot() {
        if (canShoot) {
            canShoot = false;

            currentAmmoInMagazine--;

            HandleVisuals();

            StartCoroutine(ResetShoot());

            Debug.Log("Shoot");
        }
    }

    private IEnumerator ResetShoot() {

        yield return new WaitForSeconds(fireRate);

        canShoot = true;
    }

    private void HandleVisuals() {
        if (muzzleFlashGameObject != null) {
            // For now use instantiate, later use an object pooler
            GameObject muzzleFlashGameObject = Instantiate(this.muzzleFlashGameObject, muzzleTransform.position, Quaternion.identity);
        }

        if (bulletProjectileGameObject != null) {
            // For now use instantiate. later use an object pooler
            GameObject bulletProjectileGameObject = Instantiate(this.bulletProjectileGameObject, muzzleTransform.position, Quaternion.identity);

            BulletProjectile bulletProjectile = bulletProjectileGameObject.GetComponent<BulletProjectile>();

            bulletProjectile.AssignOwner(ownerTransform);
            bulletProjectile.DefineDamageAmount(damage);
        }
    }

    public void Reload() {
        if (canReload && currentAmmoInMagazine < magazineSize) {
            canReload = false;
            canShoot = false;

            int bulletAmountToReload;

            if (currentTotalAmmoAmount - currentAmmoInMagazine > 0) {
                bulletAmountToReload = currentTotalAmmoAmount - currentAmmoInMagazine;

                currentTotalAmmoAmount -= bulletAmountToReload;
            }
            else {
                bulletAmountToReload = currentTotalAmmoAmount;

                currentTotalAmmoAmount = 0;
            }


            currentAmmoInMagazine = magazineSize;

            Invoke(nameof(ResetReload), reloadDuration);
        }
    }

    private void ResetReload() {
        canReload = true;
        canShoot = true;
    }
}