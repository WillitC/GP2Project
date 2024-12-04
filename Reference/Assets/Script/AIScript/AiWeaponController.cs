using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    private AiWeaponHandler weapon_handler;

    [Header("Weapon Settings")]
    public float fireRate = 1f; // Shots per second
    public float maxRange = 50f; // Maximum shooting distance
    public int damage = 10; // Damage dealt per shot
    public int spread = 5;
    public LayerMask targetLayer; // Layer to detect the player
    public GameObject bulletPrefab;  // The bullet prefab to spawn
    public Transform bulletSpawnPoint; // The position where bullets are spawned
    private float fireCooldown = 0f; // Time until next shot
    private float lastShotTime = 0f;

    public int Ammo = 50;

    private float _currentTime;

    void Start()
    {
        _currentTime = Time.time;
    }

    public void ShootAtTarget(Vector3 targetPosition)
    {

        if (Ammo > 0)
        {
            if (Time.time - lastShotTime < 1f / fireRate)
                return; // Enforce fire rate

            lastShotTime = Time.time;

            // Calculate the shooting direction
            Vector3 direction = ((targetPosition + new Vector3(0, 1, 0)) - bulletSpawnPoint.position).normalized;

            // Perform a raycast to determine if the target is hit
            if (Physics.Raycast(bulletSpawnPoint.position, direction, out RaycastHit hit, maxRange, targetLayer))
            {
                Debug.DrawRay(bulletSpawnPoint.position, direction * hit.distance, Color.red, 1f);

                // Direct hit logic: Apply damage if the hit target has a PlayerHealth component
                // PlayerStatus.Instance.Damage(damage);

                print("hit");
                // Spawn impact effects at the hit location
                SpawnImpactEffect(hit.point, hit.normal, hit.transform);
            }

            float rngSpread()
            {
                float result = (Random.Range(-spread, spread));
                print(result + " | " + spread);
                return result;
            }


            Quaternion lookRotation = Quaternion.LookRotation(direction);

            lookRotation *= Quaternion.Euler(rngSpread(), rngSpread(), 0);
            // Fire a bullet projectile for visual effect
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, lookRotation);


            // Play visual and audio feedback
            PlayMuzzleFlash();
            Ammo--;
            _currentTime = Time.time;
        }

        else {
        
            if (Time.time > _currentTime + 7)
            {
                _currentTime = Time.time;
                Ammo = 50;
            }

        }
    }

    private void SpawnImpactEffect(Vector3 position, Vector3 normal, Transform target)
    {
        // Ensure you have an impact prefab set
        if (bulletPrefab != null)
        {
            GameObject impactEffect = Instantiate(bulletPrefab, position + (normal * 0.01f), Quaternion.LookRotation(normal));
            impactEffect.transform.SetParent(target);
            Destroy(impactEffect, 2f); // Cleanup after 2 seconds
        }
    }

    private void PlayMuzzleFlash()
    {
        // Add muzzle flash effect or sound logic
        Debug.Log("AI fired a shot with muzzle flash!");
    }
}
