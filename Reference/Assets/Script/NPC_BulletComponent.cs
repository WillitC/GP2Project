using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BulletComponent : MonoBehaviour
{
    private float bulletSpeed = 100f; // Speed of the bullet

    public GameObject impactPrefab;

    void Start()
    {
        // Add an impulse force in the forward direction
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);

        // Destroy the bullet after 5 seconds
        if (gameObject.tag == "EnergyBullet")
        {
            Destroy(gameObject, 5f);
        }
        else
        {
            Destroy(gameObject, 1.5f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            if (!collision.gameObject.GetComponent<IFRAME>())
            {
                PlayerStatus.Instance.Damage(5f);
            }
        }
        else if (collision.gameObject.tag == ("Shield"))
        {
            Destroy(gameObject);
        }

        GameObject sparks = Instantiate(impactPrefab, transform.position, transform.rotation);
        GameObject vfx = sparks.transform.GetChild(0).gameObject;

        Destroy(vfx, 0.15f);
        Destroy(sparks, 3);

        gameObject.SetActive(false);
        Destroy(gameObject, 2); // Destroy bullet after hitting anything
        Destroy(this);
    }
}

/*
 * 
 * 

private float bulletSpeed = 100f; // Speed of the bullet

    public GameObject impactPrefab;

    void Start()
    {
        // Add an impulse force in the forward direction
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);

        // Destroy the bullet after 5 seconds
        if (gameObject.tag == "EnergyBullet")
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
            if (!collision.GetComponent<IFRAME>())
            {
                PlayerStatus.Instance.Damage(5f);
            }
        }
        else if (collision.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }

        GameObject sparks = Instantiate(impactPrefab, transform.position, transform.rotation);
        GameObject vfx = sparks.transform.GetChild(0).gameObject;

        Destroy(vfx, 0.15f);
        Destroy(sparks, 3);

        gameObject.SetActive(false);
        Destroy(gameObject, 2); // Destroy bullet after hitting anything
        Destroy(this);
    } 


 * 
 * 
 * */