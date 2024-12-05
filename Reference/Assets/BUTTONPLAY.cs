using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BUTTONPLAY : MonoBehaviour
{
    public void StartGame()
    {
        print("starting...");
        SceneManager.LoadScene("UrbanComplex"); // Replace with your scene name
    }
}