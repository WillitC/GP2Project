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

            switch (other.tag)
            {
                case "MeleeWeapon":
                    ai_status.Damage(30f);
                    break;
                case "SolidBullet":
                    ai_status.Damage(10f);
                    print("thouched");
                    break;
                default:
                    print("touched");
                    break;
            }

    }
  
}

