using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {

            switch (gameObject.tag)
            {
                case "HealingCell":
                    PlayerStatus.Instance.Heal();
                    break;
                case "EBullet":
                    PlayerStatus.Instance.Damage(30f);
                    print("thouched");
                    break;
                default:
                    print("touched");
                    break;
            }

        }
    }

}
