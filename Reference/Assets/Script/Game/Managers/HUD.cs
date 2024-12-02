using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    public static HUD Instance;

    public GameObject[] cd_windows;

    public GameObject hitCrosshair;

    private float timeScale = 0;

    private bool lerpingCD = false;

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

    public void Start_CD(float CD, int index)
    {
        RectTransform panel_ui = cd_windows[index].GetComponent<RectTransform>();
        timeScale = 0;
        if (!lerpingCD)
            StartCoroutine(LerpWindow(CD, panel_ui));
    }


    private IEnumerator LerpWindow(float duration, RectTransform window)
    {
        float speed = duration;
        float startTop = 6.5f;
        float endTop = 53f;

        lerpingCD = true;

        RectTransformExtensions.SetTop(window, startTop);

        while (timeScale < 1)
        {
            timeScale += Time.deltaTime * speed;

            float newTop = Mathf.Lerp(startTop, endTop, timeScale);
            RectTransformExtensions.SetTop(window, newTop);
            yield return null;
        }
        lerpingCD = false;
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

}
