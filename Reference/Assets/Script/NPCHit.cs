using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHit : MonoBehaviour
{
    // Start is called before the first frame update
    private AIStatus ai_status;

    void Start()
    {
        ai_status = GetComponent<AIStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MeleeWeapon")
        {
            ai_status.Damage(30f);
            if (other.GetComponent<SPECIAL>())
            {
                ai_status.Damage(50f);
                PlayerStatus.Instance.Heal(50f);
            }
        }
    }

}

