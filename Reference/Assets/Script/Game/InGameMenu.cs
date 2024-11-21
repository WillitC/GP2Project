using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro Dropdown

public class InGameMenuManager : MonoBehaviour
{
    // Reference to the in-game menu UI
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject menuRoot; 
    [Tooltip("Toggle component for framerate display")]

    // Toggle for showing FPS counter
    public Toggle framerateToggle;
    [Tooltip("Dropdown component for FPS settings")]
    public TMP_Dropdown fpsDropdown;

    // To track if the menu is active or not
    private bool isMenuActive = false; 
    private FramerateCounter framerateCounter;

    void Start()
    {
        // Set target FPS to 60 when the game starts
        GameManager.Instance.SetTargetFPS(60);

        // Initially hide the menu
        menuRoot.SetActive(false); 

        // Find the FramerateCounter component
        framerateCounter = FindObjectOfType<FramerateCounter>();
        if (framerateCounter == null)
        {
            Debug.LogError("FramerateCounter script not found in the scene.");
            return;
        }

        // Set up the FPS toggle based on whether the FPS counter is active
        framerateToggle.isOn = framerateCounter.uiText.gameObject.activeSelf;
        framerateToggle.onValueChanged.AddListener(OnFramerateCounterChanged);

        // Set up the FPS dropdown options
        fpsDropdown.onValueChanged.AddListener(OnFPSDropdownChanged);

        // Set the initial dropdown value
        fpsDropdown.value = GetCurrentFPSIndex();
    }

    void Update()
    {
        // Open/close the pause menu when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        // Make sure dropdown and other UI elements are interactable during pause
        if (isMenuActive && Time.timeScale == 0)
        {
            // Keep the dropdown and UI interactive during the pause state
            HandleDropdownInteraction();
        }
    }

    // Handle dropdown interactions and other UI updates when the game is paused
    private void HandleDropdownInteraction()
    {
        // Unity's UI input should still work during pause, but time scale must not affect UI elements.
        // Force the dropdown and toggle to work with unscaled time
        if (fpsDropdown != null && !fpsDropdown.IsInteractable())
        {
            fpsDropdown.interactable = true;
        }

        
    }

    // Toggle the pause menu and game time scale
    public void TogglePauseMenu()
    {
        // Toggle the menu state
        isMenuActive = !isMenuActive;

        // Show or hide the menu
        menuRoot.SetActive(isMenuActive);

        // Freeze or unfreeze the game
        Time.timeScale = isMenuActive ? 0 : 1;

        // Make cursor visible when the menu is open
        Cursor.visible = isMenuActive;

        // Unlock or lock the cursor
        Cursor.lockState = isMenuActive ? CursorLockMode.None : CursorLockMode.Locked; 
    }

    // Handle changes in the FPS counter toggle
    private void OnFramerateCounterChanged(bool newValue)
    {
        // Show or hide the FPS counter based on the toggle
        framerateCounter.uiText.gameObject.SetActive(newValue);
    }

    // Handle changes in the FPS dropdown selection
    private void OnFPSDropdownChanged(int value)
    {
        // Convert dropdown value to actual FPS setting
        int targetFPS = DropdownValueToFPS(value);

        // Set the target FPS in the GameManager
        GameManager.Instance.SetTargetFPS(targetFPS); 
    }

    // Convert dropdown value to FPS
    private int DropdownValueToFPS(int dropdownValue)
    {
        switch (dropdownValue)
        {
            //30 fps, 60fps, 120 fps, and unlimited fps
            case 0: return 30;
            case 1: return 60;
            case 2: return 120;
            case 3: return -1; 
            default: return 60;
        }
    }

    // Get the current FPS index from the GameManager's target FPS
    private int GetCurrentFPSIndex()
    {
        int fps = GameManager.Instance.targetFPS;
        if (fps == 30) return 0;
        if (fps == 60) return 1;
        if (fps == 120) return 2;
        return 3; 
    }
}
