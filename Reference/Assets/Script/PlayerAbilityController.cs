using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{

    public GameObject Shield;

    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shield.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            Shield.SetActive(false);
        }

    }
}
