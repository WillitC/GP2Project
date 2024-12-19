using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;
    private float MaxHealth = 100f;
    private float Health;

    private string currentRifle = "AR";

    private GameObject healthbar;
    private Slider HP_Slider;

    private float targetHealth;
    private float timeScale = 0;
    private bool lerpingHealth = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeHealthSlider();
    }

    void Start()
    {
        Health = MaxHealth;
        if (HP_Slider != null)
        {
            HP_Slider.value = MaxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (healthbar != null)
        {
            Slider HP_Slider = healthbar.GetComponent<Slider>();
            HP_Slider.value = Mathf.Lerp(0f, Health, Time.deltaTime);
        }*/
        if (Health > MaxHealth) { Health = MaxHealth; }
    }

    public float GetHealth() { return Health; }
    public float GetMaxHealth() { return MaxHealth; }
    public string GetRifle() { return currentRifle; }

    public void setRifle(string rifle)
    {
        currentRifle = rifle;
    }

    public void Damage(float damage) { Health = Mathf.Clamp((Health - damage), 0f, MaxHealth); HP_Slider.value = Health; print("damaged"); }

    public void Heal(float heal = 50f) { Health += heal; SetHealth(Health); }

    public void SetHealth(float health)
    {
        targetHealth = health;
        timeScale = 0;

        if (!lerpingHealth)
            StartCoroutine(LerpHealth());
    }
    private IEnumerator LerpHealth()
    {
        float speed = 5f;
        float startHealth = HP_Slider.value;

        lerpingHealth = true;

        while (timeScale < 1)
        {
            timeScale += Time.deltaTime * speed;
            HP_Slider.value = Mathf.Lerp(startHealth, targetHealth, timeScale);
            yield return null;
        }
        lerpingHealth = false;
        
    }

    void OnEnable()
    {
        // Subscribe to scene-loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from scene-loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinitialize the health slider when the scene changes
        InitializeHealthSlider();
        SetHealth(MaxHealth);
    }

    private void InitializeHealthSlider()
    {
        healthbar = GameObject.Find("PlayerHealthBar");
        if (healthbar != null)
        {
            HP_Slider = healthbar.GetComponent<Slider>();
            HP_Slider.maxValue = MaxHealth;
        }
    }

    //public void ResetHealth()
    //{
    //    Health = MaxHealth; // Reset health to max
    //    if (HP_Slider != null)
    //    {
    //        HP_Slider.value = MaxHealth; // Reset slider to max
    //    }
    //}

}
