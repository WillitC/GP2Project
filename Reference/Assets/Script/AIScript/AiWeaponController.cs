using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float fireRate = 1f; // Shots per second
    public float maxRange = 50f; // Maximum shooting distance
    public int damage = 10; // Damage dealt per shot
    public LayerMask targetLayer; // Layer to detect the player
    public GameObject bulletPrefab;  // The bullet prefab to spawn
    public Transform bulletSpawnPoint; // The position where bullets are spawned
    private float fireCooldown = 0f; // Time until next shot
    private float lastShotTime = 0f;

    public void ShootAtTarget(Vector3 targetPosition)
    {
        if (Time.time - lastShotTime < 1f / fireRate)
            return; // Enforce fire rate

        lastShotTime = Time.time;

        // Calculate the shooting direction
        Vector3 direction = (targetPosition - bulletSpawnPoint.position).normalized;

        // Perform a raycast to determine if the target is hit
        if (Physics.Raycast(bulletSpawnPoint.position, direction, out RaycastHit hit, maxRange, targetLayer))
        {
            Debug.DrawRay(bulletSpawnPoint.position, direction * hit.distance, Color.red, 1f);

            // Direct hit logic: Apply damage if the hit target has a PlayerHealth component
            PlayerHealth health = hit.collider.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Spawn impact effects at the hit location
            SpawnImpactEffect(hit.point, hit.normal);
        }

        // Fire a bullet projectile for visual effect
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        BulletComponent bulletComp = bullet.GetComponent<BulletComponent>();
        if (bulletComp != null)
        {
            bulletComp.bulletSpeed = 30f; // Set bullet speed
            bulletComp.OnFire(); // Trigger bullet movement
        }

        // Play visual and audio feedback
        PlayMuzzleFlash();
    }

    private void SpawnImpactEffect(Vector3 position, Vector3 normal)
    {
        // Ensure you have an impact prefab set
        if (bulletPrefab != null)
        {
            GameObject impactEffect = Instantiate(bulletPrefab, position + (normal * 0.01f), Quaternion.LookRotation(normal));
            Destroy(impactEffect, 2f); // Cleanup after 2 seconds
        }
    }

    private void PlayMuzzleFlash()
    {
        // Add muzzle flash effect or sound logic
        Debug.Log("AI fired a shot with muzzle flash!");
    }
}
