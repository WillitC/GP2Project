using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    // Reference to the in-game menu UI
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject menuRoot;
    [Tooltip("Toggle component for framerate display")]
    public Toggle framerateToggle;
    [Tooltip("Dropdown component for FPS settings")]
    public TMP_Dropdown fpsDropdown;

    // Keybinding UI Elements (current keybinding display)
    public TextMeshProUGUI moveForwardCurrent;
    public TextMeshProUGUI moveBackwardCurrent;
    public TextMeshProUGUI moveRightCurrent;
    public TextMeshProUGUI moveLeftCurrent;
    public TextMeshProUGUI jumpCurrent;
    public TextMeshProUGUI sprintDashCurrent;
    public TextMeshProUGUI shootCurrent;

    // Buttons for changing keybindings
    public Button moveForwardButton;
    public Button moveBackwardButton;
    public Button moveRightButton;
    public Button moveLeftButton;
    public Button jumpButton;
    public Button sprintDashButton;
    public Button shootButton;
    public Button returnToDefaultButton;
    public Button applyButton;

    // Reference to the main menu and keybinding panels
    public GameObject mainMenuPanel;
    public GameObject keybindingPanel;

    // Back button for returning to the main menu
    public Button backButton;

    // Keybinding Storage (initial defaults)
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>
    {
        { "MoveForward", KeyCode.W },
        { "MoveBackward", KeyCode.S },
        { "MoveRight", KeyCode.D },
        { "MoveLeft", KeyCode.A },
        { "Jump", KeyCode.Space },
        { "SprintDash", KeyCode.LeftShift },
        { "Shoot", KeyCode.Mouse0 }
    };

    // To track if the menu is active or not
    private bool isMenuActive = true;
    private FramerateCounter framerateCounter;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "UrbanComplex")
        {
            keybindingPanel.SetActive(false); // Hide menu on load
        }
    }

    void Start()
    {
        // Initially hide the menu
        menuRoot.SetActive(false);

        // Set target FPS to 60 when the game starts
        GameManager.Instance.SetTargetFPS(60);

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

        // Initialize keybinding display
        UpdateKeyBindingDisplay();

        // Add listeners to the buttons for changing keybindings
        moveForwardButton.onClick.AddListener(() => StartCoroutine(ChangeKeyBinding("MoveForward", moveForwardCurrent)));
        moveBackwardButton.onClick.AddListener(() => StartCoroutine(ChangeKeyBinding("MoveBackward", moveBackwardCurrent)));
        moveRightButton.onClick.AddListener(() => StartCoroutine(ChangeKeyBinding("MoveRight", moveRightCurrent)));
        moveLeftButton.onClick.AddListener(() => StartCoroutine(ChangeKeyBinding("MoveLeft", moveLeftCurrent)));
        jumpButton.onClick.AddListener(() => StartCoroutine(ChangeKeyBinding("Jump", jumpCurrent)));
        sprintDashButton.onClick.AddListener(() => StartCoroutine(ChangeKeyBinding("SprintDash", sprintDashCurrent)));
        shootButton.onClick.AddListener(() => StartCoroutine(ChangeKeyBinding("Shoot", shootCurrent)));

        // Add listener for resetting to default keybindings
        returnToDefaultButton.onClick.AddListener(ResetToDefaults);

        // Add listener for applying the keybinding changes
        applyButton.onClick.AddListener(ApplyChanges);

        // Add listener for "Back" button
        backButton.onClick.AddListener(ReturnToMainMenu);

        // Ensure the menu panels are set up correctly
        mainMenuPanel.SetActive(true);
        keybindingPanel.SetActive(false);

        // Add listener for "Change Controls" button
        Button keyBindingButton = mainMenuPanel.GetComponentInChildren<Button>();
        keyBindingButton.onClick.AddListener(SwitchToKeybindingPanel);
    }

    void Update()
    {
        // Check if the Escape key is pressed and if the menu is not open
        if (Input.GetKeyDown(KeyCode.Escape) && !keybindingPanel.activeSelf)
        {
            // Check if any TMP_InputField is focused using the EventSystem
            TMP_InputField inputField = EventSystem.current.currentSelectedGameObject?.GetComponent<TMP_InputField>();

            // Check if the current TMP_Dropdown is open (focused)
            // Checking if the dropdown is expanded (open)
            bool isDropdownFocused = fpsDropdown.IsExpanded;

            // Ignore Escape if an input field or dropdown is focused
            if (inputField == null && !isDropdownFocused)
            {
                TogglePauseMenu();
            }
        }

        // Make sure dropdown and other UI elements are interactable during pause
        if (isMenuActive && Time.timeScale == 0)
        {
            HandleDropdownInteraction();
        }
    }

    // Handle dropdown interactions and other UI updates when the game is paused
    private void HandleDropdownInteraction()
    {
        if (fpsDropdown != null && !fpsDropdown.IsInteractable())
        {
            fpsDropdown.interactable = true;
        }
    }

    // Toggle the pause menu and game time scale
    public void TogglePauseMenu()
    {
        isMenuActive = !isMenuActive;
        menuRoot.SetActive(isMenuActive);
        Time.timeScale = isMenuActive ? 0 : 1;
        Cursor.visible = isMenuActive;
        Cursor.lockState = isMenuActive ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Handle changes in the FPS counter toggle
    private void OnFramerateCounterChanged(bool newValue)
    {
        framerateCounter.uiText.gameObject.SetActive(newValue);
    }

    // Handle changes in the FPS dropdown selection
    private void OnFPSDropdownChanged(int value)
    {
        int targetFPS = DropdownValueToFPS(value);
        GameManager.Instance.SetTargetFPS(targetFPS);
    }

    // Convert dropdown value to FPS
    private int DropdownValueToFPS(int dropdownValue)
    {
        switch (dropdownValue)
        {
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

    // Update the keybinding display texts
    private void UpdateKeyBindingDisplay()
    {
        moveForwardCurrent.text = keyBindings["MoveForward"].ToString();
        moveBackwardCurrent.text = keyBindings["MoveBackward"].ToString();
        moveRightCurrent.text = keyBindings["MoveRight"].ToString();
        moveLeftCurrent.text = keyBindings["MoveLeft"].ToString();
        jumpCurrent.text = keyBindings["Jump"].ToString();
        sprintDashCurrent.text = keyBindings["SprintDash"].ToString();
        shootCurrent.text = keyBindings["Shoot"].ToString();
    }

    // Coroutine to change the keybinding
    private IEnumerator ChangeKeyBinding(string action, TextMeshProUGUI keyText)
    {
        // Wait for key press
        yield return new WaitUntil(() => Input.anyKeyDown);
        KeyCode newKey = GetKeyFromInput();
        keyBindings[action] = newKey;

        // Update display with the new keybinding
        UpdateKeyBindingDisplay();

        // Update the specific text UI
        keyText.text = newKey.ToString();
    }

    // Get the key that was pressed
    private KeyCode GetKeyFromInput()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode)) return keyCode;
        }
        return KeyCode.None;
    }

    // Reset to default keybindings
    private void ResetToDefaults()
    {
        keyBindings["MoveForward"] = KeyCode.W;
        keyBindings["MoveBackward"] = KeyCode.S;
        keyBindings["MoveRight"] = KeyCode.D;
        keyBindings["MoveLeft"] = KeyCode.A;
        keyBindings["Jump"] = KeyCode.Space;
        keyBindings["SprintDash"] = KeyCode.LeftShift;
        keyBindings["Shoot"] = KeyCode.Mouse0;

        UpdateKeyBindingDisplay();
    }

     private void ApplyChanges()
    {
        Debug.Log("Apply Changes clicked!");

        // Save the changes to persistent storage (e.g., PlayerPrefs).
        foreach (var keyBinding in keyBindings)
        {
            // Only save if the keybinding is different from the saved value
            if (!PlayerPrefs.HasKey(keyBinding.Key) || PlayerPrefs.GetInt(keyBinding.Key) != (int)keyBinding.Value)
            {
                PlayerPrefs.SetInt(keyBinding.Key, (int)keyBinding.Value);
            }
        }

        PlayerPrefs.Save(); // Save the PlayerPrefs

        Debug.Log("Keybindings applied successfully!");
    }

    // Function to switch to the keybinding panel
    private void SwitchToKeybindingPanel()
    {
        mainMenuPanel.SetActive(false);
        keybindingPanel.SetActive(true);
    }

    // Function to return to the main menu (from keybinding panel)
    public void ReturnToMainMenu()
    {
        keybindingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}