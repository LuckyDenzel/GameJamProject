using System.Collections;
using UnityEngine;

public class Explosive : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Rigidbody2D eplosiveRb;
    [SerializeField] private ParticleSystem explosionParticleSystem;
    [SerializeField] private Transform explosiveModelTransform;

    private Collider2D explosiveCollider;

    [Header("Values")]
    [SerializeField] private float startingExplosionRadius = 3f;
    [SerializeField] private float startingExplosionDamage = 10f;

    [SerializeField] private float startingExplosionDownwardForce = 2f;

    [SerializeField] private float explosionLifetimeDuration = 7f;

    [Header("Sound")]
    [SerializeField] private AudioClip explosionAudioClip;

    private AudioSource audioSource;

    private float currentExplosionRadius;
    private float currentExplosionDamage;
    private float currentExplosionDownwardForce;

    private bool hasExploded;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();

        explosiveCollider = GetComponent<Collider2D>();
    }

    private void Start() {
        GameStageManager.Instance.OnStageChanged += GameStageHandler_OnStageChanged;

        explosionParticleSystem.gameObject.SetActive(false);

        currentExplosionRadius = startingExplosionRadius;
        currentExplosionDamage = startingExplosionDamage;
        currentExplosionDownwardForce = startingExplosionDownwardForce;
    }

    private void FixedUpdate() {
        eplosiveRb.AddForce(Vector2.down * currentExplosionDownwardForce, ForceMode2D.Force);
    }

    // Listen to the OnStageChanged event to apply the current stage's multiplier to the current damage
    private void GameStageHandler_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        ApplyMultiplierToCurrentDamageValues(e.newGameStage.stageThreatsDamageMultiplier);
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

                    IHealth healthInterface;
                    if (objectRb.transform.TryGetComponent<IHealth>(out healthInterface)) {
                        healthInterface.ApplyDamage(Mathf.RoundToInt(currentExplosionDamage));
                    }

                    objectRb.AddForce(forceDirection, ForceMode2D.Impulse);
                }
            }

            if (audioSource.clip != explosionAudioClip) {
                audioSource.clip = explosionAudioClip;
            }

            audioSource.Play();
        }

        explosionParticleSystem.gameObject.SetActive(true);
        explosionParticleSystem.Play();

        explosiveCollider.enabled = false;
        explosiveModelTransform.gameObject.SetActive(false);

        // Handle deletion of this object, later store in a pooler
        Destroy(gameObject, 2f);

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

    private void ApplyMultiplierToCurrentDamageValues(float multiplier) {
        currentExplosionDamage *= multiplier;
        currentExplosionRadius *= multiplier;
        currentExplosionDownwardForce *= multiplier;
    }
}