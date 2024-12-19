using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Player's max health
    public float currentHealth;   // Player's current health
    public Vector3 spawnPoint;    // Player's respawn point
    public float respawnDelay = 1f; // Delay before respawning

    private void Start()
    {
        spawnPoint = transform.position; // Initialize spawn point
        currentHealth = maxHealth;      // Set current health to max
        UpdateHealthbar();              // Sync healthbar
    }

    private void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthbar();

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        StartCoroutine(ReloadSceneAfterDelay(respawnDelay));
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateHealthbar()
    {
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.SetHealth(currentHealth);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(10f);
            Destroy(collision.gameObject); // Destroy bullet on impact
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("E_MeleeWeapon"))
        {
            TakeDamage(20f);
        }
    }
}
