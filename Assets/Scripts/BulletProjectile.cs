using UnityEngine;

public class BulletProjectile : MonoBehaviour {


    [SerializeField] private Rigidbody2D bulletRb;

    [SerializeField] private float bulletSpeed = 100f;

    private Transform ownerTransform;

    private float damageAmount;

    private float direction;

    private string ownerTag;

    private void Start() {
        if (damageAmount == 0) {
            damageAmount = 10f;

            Debug.LogWarning("The damage amount isn't specified on this bullet projectile! Defaulting to 10", this);
        }
        if (direction == 0) {
            Debug.LogError("There's no direction set on this bullet!", this);
        }

        if (ownerTransform != null) {
            ownerTag = ownerTransform.tag;
        }

        GameStageManager.Instance.OnStageChanged += GameStageManager_OnStageChanged;
    }

    private void GameStageManager_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        ApplyMultiplierOnBulletSpeed(e.newGameStage.stageThreatsDamageMultiplier);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Only run hit logic when it's not from the owner's tag (making all the others of the same race no affected by the bullet logic)
        if (ownerTag != string.Empty && !collision.transform.CompareTag(ownerTag)) {
            if (collision.transform.TryGetComponent<IHealth>(out IHealth objectHealthComponent)) {
                objectHealthComponent.ApplyDamage(Mathf.RoundToInt(damageAmount));

                // For now destroy the bullet, later use a pooler
                Destroy(gameObject);
            }
        }
    }

    private void ApplyMultiplierOnBulletSpeed(float multiplier) {
        bulletSpeed *= multiplier;
    }

    public void AssignOwner(Transform owner) {
        ownerTransform = owner;
    }

    public void DefineDamageAmount(float amount) {
        damageAmount = amount;
    }

    public void SetBulletDirection(float dir) {
        direction = dir;

        // Inverse the direction since the bullet should shoot away from the owner
        bulletRb.linearVelocity = new Vector2(-direction * bulletSpeed, 0f);
    }
}