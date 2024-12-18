using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIStatus : MonoBehaviour
{
    private float MaxHealth = 100f;
    private float Health = 100f;

    public GameObject NPC_Corpse;

    public Slider HP_Slider;

    // Start is called before the first frame update
    void Start()
    {
        if (HP_Slider != null)
        {
            HP_Slider.maxValue = MaxHealth; // Set slider max value
            HP_Slider.value = Health;      // Initialize slider value
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HP_Slider != null)
        {
            HP_Slider.value = Health;
        }

        if (Health <= 0)
        {
            Die();
        }

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public void Damage(float damage)
    {
        Health -= damage;
        if (HP_Slider != null)
        {
            HP_Slider.value = Health;
        }
        print("damaged");
    }

    private void Die()
    {
        // Notify the GameManager about the kill
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncrementKillCount();
        }
        else
        {
            Debug.LogWarning("GameManager.Instance is null. Ensure GameManager exists in the scene.");
        }

        // Spawn the corpse
        if (NPC_Corpse != null)
        {
            GameObject corpse = Instantiate(NPC_Corpse, transform.position, transform.rotation);
            Destroy(corpse, 5); // Destroy the corpse after 5 seconds
        }

        // Deactivate the AI
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        Health = MaxHealth;
        if (HP_Slider != null)
        {
            HP_Slider.value = Health;
        }
        gameObject.SetActive(true);
    }
}
