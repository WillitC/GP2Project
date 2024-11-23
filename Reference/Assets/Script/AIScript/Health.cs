using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    RagDoll ragdoll;

    void Start()
    {
        ragdoll = GetComponent<RagDoll>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Optional: Add logic here for health bar UI updates or other effects.
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        ragdoll.ActivateRagDoll();
    }
}
