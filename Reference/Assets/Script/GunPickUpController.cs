using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUpController : MonoBehaviour
{

    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;

    public float pickupRange;

    public bool equipped;
    public static bool slotFull;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distToPlayer = player.position - transform.position;
        if (!equipped &&  distToPlayer.magnitude <= pickupRange && Input.GetKeyDown(KeyCode.X) && !slotFull)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        coll.isTrigger = true;

        //guncomponent.enabled = true
    }

    private void SwapOut()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);



        rb.isKinematic = false;
        coll.isTrigger = false;
    }
}
