using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveTextController : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    void Start()
    {
        objectiveText.gameObject.SetActive(true); 
        Invoke("HideText", 3f);
    }

    void HideText()
    {
        objectiveText.gameObject.SetActive(false);
    }
}