using UnityEngine;

public class Player_Combat : MonoBehaviour {


    [SerializeField] private Player_Movement playerMovement;

    [SerializeField] private int meleeAttackDamage = 60;
    [SerializeField] private float meleeAttackRange = 3f;

    [SerializeField] private LayerMask enemyLayerMask;

    private float closestEnemyCheckTimer;
    private float closestEnemyCheckTimerDuration = 0.2f;

    private Collider2D currentClosestEnemy;
    private Collider2D[] currentClosestEnemiesArray;

    private void Update() {
        // Save performance by checking every 0.2 seconds for enemies instead of every frame
        closestEnemyCheckTimer -= Time.deltaTime;

        if (closestEnemyCheckTimer <= 0) {
            closestEnemyCheckTimer = closestEnemyCheckTimerDuration;

            if (IsEnemyClose()) {
                currentClosestEnemy = GetClosestEnemyInMeleeRange(currentClosestEnemiesArray);
            } else {
                currentClosestEnemy = null;
            }
        }

        // Listen to melee input
        if (currentClosestEnemy != null) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                PerformMeleeAttack(currentClosestEnemy.GetComponent<Enemy_Health>());
            }
        }
    }

    private void PerformMeleeAttack(Enemy_Health enemyToAttack) {
        enemyToAttack.TakeDamage(meleeAttackDamage);
    }

    private bool IsEnemyClose() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, meleeAttackRange, enemyLayerMask);

        if (colliders.Length > 0) {
            currentClosestEnemiesArray = colliders;

            return true;
        }

        currentClosestEnemiesArray = null;
        return false;
    }

    private Collider2D GetClosestEnemyInMeleeRange(Collider2D[] enemyCollidersArray) {
        // Get the origin of the player
        Vector2 origin = transform.position;

        // Get the current facing direction of the player
        Vector2 facingDirection = playerMovement.IsFacingRight ? Vector2.right : Vector2.left;

        Collider2D closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D enemy in enemyCollidersArray) { // Loop thru all the enemies that are close and see which one's closest and facing the player
            Vector2 toEnemy = ((Vector2)enemy.transform.position - origin).normalized;

            // Check if the enemy is in front of the player, if not, continue
            if (Vector2.Dot(toEnemy, facingDirection) <= 0) continue;

            // Get the distance between the player and the enemy
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            // Check if the distance is the closest distance so far
            if (distance < closestDistance) {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}