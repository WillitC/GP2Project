using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class load : MonoBehaviour
{

    public GameObject MenuCanvas;
    // This function will be called when the Play button is clicked
    public void PlayGame()
    {
        // Load the first level scene (replace "FirstLevel" with your actual scene name)
        SceneManager.LoadScene("UrbanComplex");
    }

    // This function will be called when the Options button is clicked
    public void ToggleOptions()
    {
        // Toggle the visibility of the options canvas
        MenuCanvas.SetActive(!MenuCanvas.activeSelf);
    }
}