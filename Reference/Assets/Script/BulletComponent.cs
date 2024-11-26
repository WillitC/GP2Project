using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public float bulletSpeed = 10f; // Speed of the bullet

    public GameObject impactPrefab;

    void Start()
    {
        // Add an impulse force in the forward direction
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        // Destroy the bullet after 5 seconds
        if(gameObject.tag == "EnergyBullet")
        {
            Destroy(gameObject, 5f);
        }
        else
        {
            Destroy(gameObject, 1.5f);
        }      

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f);
            }
        }

        GameObject sparks = Instantiate(impactPrefab, transform.position, transform.rotation);
        GameObject vfx = sparks.transform.GetChild(0).gameObject;

        Destroy(vfx, 0.15f);
        Destroy(sparks, 3);

        gameObject.SetActive(false);
        Destroy(gameObject, 2); // Destroy bullet after hitting anything
    }
}

