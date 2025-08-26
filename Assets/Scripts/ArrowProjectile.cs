using UnityEngine;

public class ArrowProjectile : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Rigidbody2D arrowRb;

    [Header("Values")]
    [SerializeField] private float startingDamageAmount = 30f;
    [SerializeField] private float startingDownwardForce = 50f;

    [SerializeField] private float arrowLifetimeAfterOnHit = 10f;

    private Collider2D arrowCollider;

    private float currentDamageAmount;
    private float currentDownwardForce;


    private void Awake() {
        arrowCollider = GetComponent<Collider2D>();
    }

    private void Start() {
        GameStageManager.Instance.OnStageChanged += GameStageHandler_OnStageChanged;

        arrowRb.centerOfMass = new Vector2(0, -0.6f);

        currentDamageAmount = startingDamageAmount;
        currentDownwardForce = startingDownwardForce;
    }

    private void GameStageHandler_OnStageChanged(object sender, GameStageManager.OnStageChangedEventArgs e) {
        ApplyMultiplierToCurrentValues(e.newGameStage.stageThreatsDamageMultiplier);
    }

    private void FixedUpdate() {
        Vector2 forceDirection = transform.forward * currentDownwardForce;
        arrowRb.AddForceAtPosition(forceDirection, arrowRb.worldCenterOfMass);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag(Player_Logic.PLAYER_TAG)) {
            IHealth healthScript;

            // Deal damage to the player
            if (collision.transform.TryGetComponent<IHealth>(out healthScript)) {
                healthScript.ApplyDamage(Mathf.RoundToInt(currentDamageAmount));
            }
        }

        // Make the arrow a child of the hit object so it stays connected
        transform.SetParent(collision.transform, true);

        // Set the rigidbody body type to kinematic to make it not react to physics furhter
        arrowRb.bodyType = RigidbodyType2D.Kinematic;

        // Disable collisions for this arrow so that it doesn't interfere with something
        arrowCollider.enabled = false;

        Destroy(gameObject, arrowLifetimeAfterOnHit);
    }

    private void ApplyMultiplierToCurrentValues(float multiplier) {
        currentDamageAmount *= multiplier;
        currentDownwardForce *= multiplier;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(arrowRb.centerOfMass), 0.05f);
    }
}