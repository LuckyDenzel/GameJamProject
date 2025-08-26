using UnityEngine;

public class BulletProjectile : MonoBehaviour {


    [SerializeField] private Rigidbody2D bulletRb;

    [SerializeField] private float bulletSpeed = 100f;

    private float damageAmount;

    private void Start() {
        if (damageAmount == 0) {
            Debug.LogError("The damage amount isn't specified on this bullet projectile!", this);
        }
    }

    private void FixedUpdate() {
        Vector2 forceDirection = transform.forward * bulletSpeed;

        bulletRb.AddForce(forceDirection, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.TryGetComponent<IHealth>(out IHealth objectHealthComponent)) {
            objectHealthComponent.ApplyDamage(Mathf.RoundToInt(damageAmount));
        }
    }

    public void DefineDamageAmount(float amount) {
        damageAmount = amount;
    }
}