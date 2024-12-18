using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    public TMP_Text killCounterUI; // Reference to the UI Text (screen space)
    public TMP_Text killCounterWorldSpace; 

    private int killCount = 0; // Track the kill count
    public int killGoal = 5; // The goal for kills

    // Method to call whenever an enemy is killed
    public void IncrementKillCount()
    {
        // Increment the kill count
        killCount++;

        // Update the UI with the new kill count
        UpdateKillCounterUI(); 

        if (killCount >= killGoal)
        {
            // Logic when the goal is reached
            Debug.Log("Kill goal reached!");
        }
    }

    // Method to update the UI with the current kill count
    private void UpdateKillCounterUI()
    {
        if (killCounterUI != null)
        {
            killCounterUI.text = $"Kills: {killCount} / {killGoal}";
        }

        if (killCounterWorldSpace != null)
        {
            killCounterWorldSpace.text = $"Kills: {killCount} / {killGoal}";
        }
    }
}