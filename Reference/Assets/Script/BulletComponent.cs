using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : BulletBase
{
    public float bulletSpeed = 10f; // Speed of the bullet


    public Transform BulletRoot;

    public Transform BulletTip;

    public GameObject impactPrefab;

    BulletBase bulletModel;
    List<Collider> excludeCollider;

    Vector3 lastPos;
    Vector3 bulletVelocity;

    const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

    void OnEnable()
    {
        bulletModel = GetComponent<BulletBase>();
        bulletModel.OnFire += OnFire;
 
        if (gameObject.tag == "EnergyBullet")
        {
            Destroy(gameObject, 5f);
        }
        else
        {
            Destroy(gameObject, 1.5f);
        }      

    }

    new void OnFire()
    {
        lastPos = BulletRoot.position;
        bulletVelocity = transform.forward * bulletSpeed;
        excludeCollider = new List<Collider>();
        transform.position += bulletModel.InheritedMuzzleVelocity * Time.deltaTime;
        //Collider playerColliders = bulletModel.Owner
    }

    void Update()
    {
        transform.position += bulletVelocity * Time.deltaTime;
        transform.forward = bulletVelocity.normalized;
        {
            RaycastHit closestHit = new RaycastHit();
            closestHit.distance = Mathf.Infinity;
            bool foundHit = false;

            // Sphere cast
            Vector3 lastFrame_pos = BulletTip.position - lastPos;
            RaycastHit[] hits = Physics.SphereCastAll(lastPos, 0.01f, lastFrame_pos.normalized, lastFrame_pos.magnitude, -1, k_TriggerInteraction);
            foreach (var hit in hits)
            {
                if (hit.distance < closestHit.distance)
                {
                    foundHit = true;
                    closestHit = hit;
                }
            }

            if (foundHit)
            {
                if (closestHit.distance <= 0f)
                {
                    closestHit.point = BulletRoot.position;
                    closestHit.normal = -transform.forward;
                }

                OnHit(closestHit.point, closestHit.normal, closestHit.collider);
            }
        }

        lastPos = BulletRoot.position;
    }

    bool canHit(RaycastHit hit)
    {
        if (hit.collider.GetComponent<IgnoreHitDetection>())
        {
            return false;
        }

        if (hit.collider.isTrigger && hit.collider.GetComponent<Damageable>() == null)
        {
            return false;
        }

        if (excludeCollider != null && excludeCollider.Contains(hit.collider))
        {
            return false;
        }

        return true;
    }

    void OnHit(Vector3 point, Vector3 normal, Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f);
            }
        }
        else
        {
            AIStatus hp = collider.GetComponent<AIStatus>();

            if (hp != null) { hp.Damage(15); HUD.Instance.hitHUD(); }

        }



        GameObject sparks = Instantiate(impactPrefab, point + (normal * 0.01f),
                    Quaternion.LookRotation(normal));
        GameObject vfx = sparks.transform.GetChild(0).gameObject;

        Destroy(vfx, 0.15f);
        Destroy(sparks, 3);

        gameObject.SetActive(false);
        Destroy(gameObject, 2); // Destroy bullet after hitting anything
    }
}

