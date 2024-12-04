using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    public GameObject[] cd_windows;

    public GameObject hitCrosshair;

    private List<string> lerpingCD = new List<string>();


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
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void hitHUD()
    {
        hitCrosshair.SetActive(true);
        StartCoroutine(HitTimer());
    }

    public void Start_CD(float CD, string index)
    {
        RectTransform panel_ui = getCDWindow(index).GetComponent<RectTransform>();
        
        if (!hasCD(index))
            StartCoroutine(LerpWindow(CD, panel_ui, index));
    }


    private IEnumerator LerpWindow(float duration, RectTransform window, string index)
    {
        float speed = 1/duration;
        float startTop = 6.5f;
        float endTop = 53f;
        float timeScale = 0;
        
        lerpingCD.Add(index);


        RectTransformExtensions.SetTop(window, startTop);

        while (timeScale < 1)
        {
            timeScale += Time.deltaTime * speed;

            float newTop = Mathf.Lerp(startTop, endTop, timeScale);
            RectTransformExtensions.SetTop(window, newTop);
            yield return null;
        }
        lerpingCD.Remove(index);
    }

    private IEnumerator HitTimer()
    {
        float currentTime = Time.time + 0.1f;

        while (Time.time < currentTime)
        {
            yield return null;
        }
        hitCrosshair.SetActive(false);
    }

    private GameObject getCDWindow(string index) {
        foreach(GameObject cd in cd_windows)
        {
            if (cd.name == index)
            {
                return cd;
            }
        }
        return null;
    }

    private bool hasCD(string index)
    {
        foreach (var cd in lerpingCD)
        {
            if (cd == index)
            {
                return true;
            }
        }

        return false;
    }

}
