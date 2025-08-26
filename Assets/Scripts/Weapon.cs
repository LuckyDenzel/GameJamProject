using UnityEngine;

public class Weapon : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Transform muzzleTransform;

    [Header("Properties")]
    [SerializeField] private float fireRate = 1f;

    [SerializeField] private float range = 20f;

    [SerializeField] private float damage = 10f;

    [SerializeField] private int magazineSize = 20;
    [SerializeField] private int startingTotalAmmoAmount = 60;

    private int currentTotalAmmoAmount;
    private int currentAmmoInMagazine;

    [Header("VFX")]
    [SerializeField] private GameObject muzzleFlashGameObject;
    [SerializeField] private GameObject bulletProjectileGameObject;

    // The hit result of the most recent shot
    private RaycastHit2D weaponHit;

    private void Start() {
        currentTotalAmmoAmount = startingTotalAmmoAmount;
    }

    public void Shoot() {
        currentAmmoInMagazine--;

        weaponHit = Physics2D.Raycast(muzzleTransform.position, muzzleTransform.forward, range);

        HandleVisuals();
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
            bulletProjectile.DefineDamageAmount(damage);
        }
    }
}