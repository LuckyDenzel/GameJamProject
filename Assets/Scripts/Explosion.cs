using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Explosion : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Rigidbody2D eplosiveRb;
    [SerializeField] private ParticleSystem explosionParticleSystem;

    [Header("Values")]
    [SerializeField] private float startingExplosionRadius = 3f;
    [SerializeField] private float startingExplosionDamage = 10f;

    [SerializeField] private float explosionLifetimeDuration = 7f;

    private float currentExplosionRadius;
    private float currentExplosionDamage;

    private bool hasExploded;

    private void Start() {
        explosionParticleSystem.gameObject.SetActive(false);

        currentExplosionRadius = startingExplosionRadius;
        currentExplosionDamage = startingExplosionDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!hasExploded) {
            Explode();

            hasExploded = true;
        }
    }

    private void Explode() {
        Collider2D[] objectColliders = Physics2D.OverlapCircleAll(transform.position, currentExplosionRadius);

        if (objectColliders != null) {
            foreach (Collider2D obj in objectColliders) {
                Rigidbody2D objectRb = obj.attachedRigidbody;

                if (objectRb != null) {
                    Vector2 forceDirection = objectRb.position - (Vector2)transform.position;

                    objectRb.AddForce(forceDirection, ForceMode2D.Impulse);
                }
            }
        }

        explosionParticleSystem.gameObject.SetActive(true);
        explosionParticleSystem.Play();

        // Handle deletion of this object, later store in a pooler
        Destroy(gameObject);

        // Handle deletion of the explosion particle system
        StartCoroutine(HandleExplosionLifetime());
    }

    private IEnumerator HandleExplosionLifetime() {
        // Decouple the particle system so that it doesn't rol with the object
        explosionParticleSystem.transform.SetParent(null);

        yield return new WaitForSeconds(explosionLifetimeDuration);

        // For now destroy the explosion, later maybe use a pooler
        Destroy(explosionParticleSystem.gameObject);
    }
}