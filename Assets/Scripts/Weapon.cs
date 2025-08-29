using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private Transform ownerTransform;

    [Header("Properties")]
    [SerializeField] private float startingFireRate = 2f;

    [SerializeField] private float startingRange = 20f;

    [SerializeField] private float startingDamage = 5f;

    [SerializeField] private int magazineSize = 20;
    [SerializeField] private int startingTotalAmmoAmount = 60;

    [SerializeField] private float reloadDuration = 2f;

    private int currentTotalAmmoAmount;
    private int currentAmmoInMagazine;
    private float currentFireRate;
    private float currentRange;
    private float currentDamage;

    private bool canReload;

    [Header("VFX")]
    [SerializeField] private GameObject muzzleFlashGameObject;
    [SerializeField] private GameObject bulletProjectileGameObject;

    [Header("Sound")]
    [SerializeField] private AudioClip gunShotAudioClip;

    private AudioSource audioSource;

    private bool canShoot = true;

    // Fields
    public float WeaponRange => currentRange;
    public int CurrentAmmoInMagazine => currentAmmoInMagazine;
    public bool CanShoot => canShoot;

    public Transform MuzzleTransform => muzzleTransform;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        GameStageManager.Instance.OnStageChanged += GameStageManager_OnStageChanged;

        currentTotalAmmoAmount = startingTotalAmmoAmount;
        currentAmmoInMagazine = magazineSize;
        currentDamage = startingDamage;
        currentFireRate = startingFireRate;
        currentRange = startingRange;
    }

    private void GameStageManager_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        ApplyThreatMultiplier(e.newGameStage.stageThreatsDamageMultiplier);
    }

    public void Shoot() {
        if (canShoot) {
            canShoot = false;

            currentAmmoInMagazine--;

            HandleVisuals();

            if (audioSource.clip != gunShotAudioClip) {
                audioSource.clip = gunShotAudioClip;
            }

            audioSource.Play();

            StartCoroutine(ResetShoot());
        }
    }

    private IEnumerator ResetShoot() {

        yield return new WaitForSeconds(currentFireRate);

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
            bulletProjectile.DefineDamageAmount(currentDamage);
            bulletProjectile.SetBulletDirection(Mathf.Sign(ownerTransform.localScale.x));
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

    private void ApplyThreatMultiplier(float multiplier) {
        currentDamage *= multiplier;
        currentRange *= multiplier;

        float inversedMultiplier = currentFireRate / multiplier;
        currentFireRate *= inversedMultiplier;
    }

    private void ResetReload() {
        canReload = true;
        canShoot = true;
    }
}