using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damageable : MonoBehaviour
{

    public float DamageMultiplier = 1f;

    [Range(0, 1)]

    public float SensibilityToSelfdamage = 0.5f;

    public Health Health { get; private set; }

    void Awake()
    {
        Health = GetComponent<Health>();
        if (!Health)
        {
            Health = GetComponentInParent<Health>();
        }
    }

    public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
    {
        if (Health)
        {
            var totalDamage = damage;

            if (!isExplosionDamage)
            {
                totalDamage *= DamageMultiplier;
            }

            if (Health.gameObject == damageSource)
            {
                totalDamage *= SensibilityToSelfdamage;
            }

            Health.TakeDamage(totalDamage);
        }
    }
}
