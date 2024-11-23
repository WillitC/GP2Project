using UnityEngine;

public class MeleeAIWeaponController : WeaponControllerBase
{
    public GameObject MeleeWeapon;
    public float attackCooldown = 2f;
    private float nextAttackTime;
    public float attackRange = 1.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayer;

    public override void Attack()
    {
        // Add attack animation or logic here
        Debug.Log("Performing melee attack!");

        // Optionally check for collisions or apply damage
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            // Apply damage to enemy
            //enemy.GetComponent<Health>()?.TakeDamage(attackDamage);
        }
    }

    private void DisableWeapon()
    {
        MeleeWeapon.SetActive(false);
    }
}