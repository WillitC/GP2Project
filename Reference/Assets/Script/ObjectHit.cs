using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour
{

    public GameObject impactVFX;

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {

            switch (gameObject.tag)
            {
                case "HealingCell":
                    PlayerStatus.Instance.Heal();
                    GameObject takenVFX = Instantiate(impactVFX, transform.position, transform.rotation);
                    gameObject.SetActive(false);
                    Destroy(takenVFX, 2);
                    Invoke("Respawn", 5);
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

    private void Respawn()
    {
        gameObject.SetActive(true);
    }
}
