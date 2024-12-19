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

    // References for the invisible wall (door) and arrow UI element
    public GameObject invisibleWall;    // The invisible wall (door) object
    public RectTransform arrowUIElement;     // The UI arrow element (make sure this is a RectTransform)
    public Transform doorPosition;      // Position of the door (for arrow to point at)

    private Canvas canvas;              // The canvas where the arrow is displayed
    private bool isArrowActive = false; // Flag to check if the arrow should be active

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
        arrowUIElement.gameObject.SetActive(false);  // Ensure arrow is hidden initially
        canvas = arrowUIElement.GetComponentInParent<Canvas>();  // Get the canvas reference
    }

    private void Update()
    {
        if (isArrowActive && arrowUIElement != null && doorPosition != null)
        {
            UpdateArrowDirection();
        }
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

    public void IncrementKillCount()
    {
        // Ensure kill count does not exceed the goal
        if (killCount >= killGoal)
        {
            Debug.Log("Kill count is already at the goal. No further increment.");
            return;
        }

        killCount++; // Increase the kill count
        UpdateKillCounterUI(); // Update the UI with the new kill count

        // Check if the kill goal is met
        if (killCount >= killGoal)
        {
            Debug.Log("Kill goal reached!");

            // Remove the invisible wall (door)
            if (invisibleWall != null)
            {
                invisibleWall.SetActive(false);  // Hide the invisible wall
            }

            // Activate the arrow to guide the player
            if (arrowUIElement != null && doorPosition != null)
            {
                arrowUIElement.gameObject.SetActive(true);  // Show the arrow
                isArrowActive = true; // Enable arrow updates
            }
        }
    }


    private void UpdateArrowDirection()
    {
        // Calculate direction from the player to the door (world space)
        Vector3 directionToDoor = doorPosition.position - Camera.main.transform.position;

        // Project this direction to 2D screen space
        Vector3 screenDirection = Camera.main.WorldToScreenPoint(doorPosition.position);

        // Check if the player is close enough to the door to consider it "crossed"
        float distanceToDoor = Vector3.Distance(Camera.main.transform.position, doorPosition.position);
        float doorThreshold = 2.0f; // Distance threshold to consider the door crossed (adjust as needed)

        // Check if the door is crossed
        if (distanceToDoor <= doorThreshold)
        {
            // Player has crossed the door; hide the arrow
            arrowUIElement.gameObject.SetActive(false);
            isArrowActive = false; // Stop further arrow updates
            return;
        }

        // Check if the door is behind the camera
        if (screenDirection.z < 0)
        {
            // Recalculate screenDirection to avoid flipping the arrow unexpectedly
            screenDirection *= -1; // Mirror the direction to handle behind-camera calculations
        }

        // Arrow should always be visible when active and the door is not crossed
        arrowUIElement.gameObject.SetActive(true);

        // Calculate the angle between the center of the screen and the door
        Vector3 arrowDirection = screenDirection - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;

        // Rotate the arrow in the UI to point towards the door
        arrowUIElement.rotation = Quaternion.Euler(0, 0, angle);
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