using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int targetFPS = 60;
    public int score = 0;

    // UI References
    public TMP_Text killCounterUI;
    public TMP_Text objectiveUI;
    public TMP_Text rushTimerUI;

    // Kill count for the first objective
    private int killCount = 0;

    // Kill goal for the first objective
    public int killGoal = 5; 

    // Objective 2 specific variables
    private int objective2KillCount = 0; 
    private int objective2KillGoal = 20; 

    // Object References
    public GameObject invisibleWall;
    public GameObject invisiblePlane2; 
    public RectTransform arrowUIElement;
    public Transform doorPosition; 
    public Transform nextDoorPosition; 

    //Tracking objective states
    private Canvas canvas;
    private bool isArrowActive = false;
    private bool objective1Complete = false;
    private bool objective2Active = false;
    private bool objective2Complete = false;
    private bool objective2Reached = false; 

    // Rush timer variables
    private float rushTime = 30f; 
    private bool isRushActive = false;

    // New Portal Point Variable
    public Transform portalPoint; 

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
        UpdateKillCounterUI();

        // Ensure arrow is hidden initially
        arrowUIElement.gameObject.SetActive(false);

        // Hide objective text
        if (objectiveUI != null) objectiveUI.gameObject.SetActive(false);

        // Hide rush timer initially
        if (rushTimerUI != null) rushTimerUI.gameObject.SetActive(false); 
        canvas = arrowUIElement.GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        // Update arrow visibility and direction
        if (isArrowActive && arrowUIElement != null)
        {
            if (objective1Complete && !objective2Complete)
            {
                // Arrow should point to the second objective only after the first is complete
                UpdateArrowDirection(doorPosition); 
            }

            if (objective2Complete && !objective2Reached)
            {
                // After completing objective 2, update the arrow to point to the next door
                UpdateArrowDirection(nextDoorPosition); 
            }
        }

        // Handle rush timer countdown
        if (isRushActive)
        {
            if (rushTime > 0)
            {
                rushTime -= Time.deltaTime;
                if (rushTimerUI != null)

                    // Update timer display
                    rushTimerUI.text = $"RUN! {Mathf.CeilToInt(rushTime)}"; 
            }
            else
            {
                if (rushTimerUI != null)
                    rushTimerUI.text = "Time's up!";
                isRushActive = false;

                // Reset the scene when the timer runs out
                RestartLevel(); 
            }
        }

        // Check if the player has passed through the portal point
        CheckForPortal();
    }

    // method to check if the player has passed through the portal point
    private void CheckForPortal()
    {
        // logic for detecting the portal 
        if (Vector3.Distance(Camera.main.transform.position, portalPoint.position) < 2.0f) 
        {
            // Load the boss arena scene when the player reaches the portal point
            SceneManager.LoadScene("Boss"); 
        }
    }

    //method to increment kill count (used for objectives)
    public void IncrementKillCount()
    {
        if (objective1Complete && objective2Active && !objective2Complete)
        {
            IncrementObjective2KillCount();
            return;
        }

        if (!objective1Complete)
        {
            killCount++;
            UpdateKillCounterUI();

            if (killCount >= killGoal)
            {
                CompleteFirstObjective();
            }
        }
    }

    //method to increment kill count for objective 2
    private void IncrementObjective2KillCount()
    {
        objective2KillCount++;
        Debug.Log($"Objective 2 Kills: {objective2KillCount} / {objective2KillGoal}");

        if (objective2KillCount >= objective2KillGoal)
        {
            CompleteSecondObjective();
        }
    }

    private void CompleteFirstObjective()
    {
        objective1Complete = true;
        Debug.Log("First Objective Complete!");

        if (killCounterUI != null) killCounterUI.gameObject.SetActive(false);

        // Remove the first invisible wall
        if (invisibleWall != null) invisibleWall.SetActive(false);

        if (arrowUIElement != null && doorPosition != null)
        {
            // Activate the arrow
            arrowUIElement.gameObject.SetActive(true); 
            isArrowActive = true; 
        }

        // Once first objective is complete, hide the arrow when the target is reached
        if (objectiveUI != null)
        {
            objectiveUI.gameObject.SetActive(true);

            // Text update for the first objective
            objectiveUI.text = "Reach the Door!"; 
        }
    }

    private void UpdateArrowDirection(Transform target)
    {
        // Get the screen position of the target object
        Vector3 screenDirection = Camera.main.WorldToScreenPoint(target.position);

        // Calculate angle to target
        Vector3 arrowDirection = screenDirection - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;

        // Rotate the arrow UI element towards the target
        arrowUIElement.rotation = Quaternion.Euler(0, 0, angle);

        // Debug information to see the position and distance
        float distanceToTarget = Vector3.Distance(Camera.main.transform.position, target.position);

        // Update arrow visibility based on distance to target
        if (distanceToTarget <= 2.0f) 
        {
            // Once reached, deactivate the arrow and update UI for the next objective
            if (arrowUIElement.gameObject.activeSelf)
            {
                // Hide the arrow
                arrowUIElement.gameObject.SetActive(false);
                isArrowActive = false;

                // For the second objective, mark as reached
                if (objective2Complete && !objective2Reached)
                {
                    // Mark the second objective as reached
                    objective2Reached = true;

                    // Hide the arrow for objective 2 once the target is reached
                    arrowUIElement.gameObject.SetActive(false);
                    isArrowActive = false;

                }

                // If the first objective is completed, activate the second objective
                if (objective1Complete && !objective2Active)
                {
                    ActivateSecondObjective();
                }
            }
        }
        else
        {
            // Ensure the arrow is visible and active if the player is far
            if (!arrowUIElement.gameObject.activeSelf)
            {
                arrowUIElement.gameObject.SetActive(true);
                isArrowActive = true;
            }
        }
    }

    //method to activate 2nd objective
    private void ActivateSecondObjective()
    {
        Debug.Log("Second Objective Activated!");
        objective2Active = true;

        if (objectiveUI != null)
        {
            objectiveUI.gameObject.SetActive(true);

            // Text update for second objective
            objectiveUI.text = "Defeat All Enemies!"; 
        }

        // Ensure the arrow doesn't show before objective 2 is reached
        if (arrowUIElement != null)
        {
            // Hide arrow until objectives are completed
            arrowUIElement.gameObject.SetActive(false); 
            isArrowActive = false;
        }
    }

    //method to detect if second objective is done
    private void CompleteSecondObjective()
    {
        Debug.Log("Second Objective Complete!");
        objective2Complete = true;

        // Hide objective text once the second objective is completed
        if (objectiveUI != null)
        {
            objectiveUI.gameObject.SetActive(false);
        }

        // Remove the second invisible wall
        if (invisiblePlane2 != null)
        {

            // Remove second invisible wall
            invisiblePlane2.SetActive(false); 
        }

        // Show the arrow to point to the next door after all objectives
        if (arrowUIElement != null && nextDoorPosition != null)
        {
            arrowUIElement.gameObject.SetActive(true);

            // Activate arrow updates
            isArrowActive = true; 
        }

        // Start the rush timer
        StartRushTimer();
    }

    //method to start the timer
    private void StartRushTimer()
    {
        if (rushTimerUI != null)
        {
            // Show the timer UI
            rushTimerUI.gameObject.SetActive(true);

            // Display the initial message
            rushTimerUI.text = $"RUN! {rushTime}"; 
        }

        isRushActive = true;
    }

    //method to apply fps settings
    private void ApplySettings()
    {
        Application.targetFrameRate = targetFPS;
    }

    //method to update the kill counter for first objective
    private void UpdateKillCounterUI()
    {
        if (killCounterUI != null)
        {
            killCounterUI.text = $"Kills: {killCount} / {killGoal}";
        }
    }

    //method to increment scores
    public void IncrementScore(int amount = 1)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    //method to restart the level
    public void RestartLevel()
    {
        // Reset the scene to restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    //method to  wait
    public IEnumerator Wait(float timeUnit = 0.1f)
    {
        yield return new WaitForSeconds(timeUnit);
    }


    //method to set fps by default to 60
    public void SetTargetFPS(int fps)
    {
        targetFPS = fps;
        ApplySettings();
    }
}
