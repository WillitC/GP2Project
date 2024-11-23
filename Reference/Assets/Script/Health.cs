using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Optional: Add logic here for health bar UI updates or other effects.
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0.0f)
        {
            Debug.Log($"{gameObject.name} has died.");
            Destroy(gameObject); // Destroy the enemy when health reaches 0
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided is tagged as "SolidBullet"
        if (collision.gameObject.CompareTag("SolidBullet"))
        {
            TakeDamage(10f); // Reduce health by 10

            // Optionally destroy the bullet on impact
            Destroy(collision.gameObject);
        }
    }
}
