using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int targetFPS = 60;
    public int score = 0;

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

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(object sceneNameOrIndex)
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

    public IEnumerator Wait(float timeUnit = 0.1f)
    {
        yield return new WaitForSeconds(timeUnit);
    }
}
