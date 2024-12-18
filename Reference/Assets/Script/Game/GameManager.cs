using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int targetFPS = 60;
    public int score = 0;

    // Reference to the kill count UI text (TextMeshPro)
    public TMP_Text killCounterUI;

    private int killCount = 0; // Keep track of the kill count
    public int killGoal = 5; // Goal for the number of kills

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplySettings();
        UpdateKillCounterUI(); // Initialize the UI on start
    }

    public void SetTargetFPS(int fps)
    {
        targetFPS = fps;
        ApplySettings();
    }

    private void ApplySettings()
    {
        Application.targetFrameRate = targetFPS;
    }

    public void IncrementScore(int amount = 1)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    // This method will be called to increment the kill count
    public void IncrementKillCount()
    {
        killCount++; // Increase the kill count
        UpdateKillCounterUI(); // Update the UI with the new kill count

        // Check if the kill goal is met
        if (killCount >= killGoal)
        {
            Debug.Log("Kill goal reached!");
            // Add any other logic you'd like to trigger when the goal is met
        }
    }

    // Method to update the kill counter UI
    private void UpdateKillCounterUI()
    {
        if (killCounterUI != null)
        {
            killCounterUI.text = $"Kills: {killCount} / {killGoal}";
        }
    }

    public void RestartLevel()
    {
        // Restart the current level
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*public void LoadScene(object sceneNameOrIndex)
    {
        if (sceneNameOrIndex is int)
        {
            SceneManager.LoadScene((int)sceneNameOrIndex);
        }
        else if (sceneNameOrIndex is string)
        {
            SceneManager.LoadScene((string)sceneNameOrIndex);
        }
    }
    */
    public IEnumerator Wait(float timeUnit = 0.1f)
    {
        yield return new WaitForSeconds(timeUnit);
    }
}
