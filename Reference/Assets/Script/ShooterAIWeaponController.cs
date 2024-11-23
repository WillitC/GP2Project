using UnityEngine;

public class ShooterAIWeaponController : WeaponControllerBase
{
    public float fireRate = 1f;
    private float nextFireTime;
    public GameObject bulletPrefab; // Assign in the Unity Inspector
    public Transform muzzlePoint;  // Assign the muzzle transform
    public float bulletSpeed = 20f; // Adjust the speed of the bullet

    public override void Attack()
    {
        if (bulletPrefab != null && muzzlePoint != null)
        {
            // Instantiate the bullet prefab at the muzzle position and rotation
            GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);

            // Add forward force to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = muzzlePoint.forward * bulletSpeed;
            }
        }
    }
    public void PerformMeleeAttack() { }
}