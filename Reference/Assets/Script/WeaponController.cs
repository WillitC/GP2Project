using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject RangedWeapon;
    public GameObject MeleeWeapon;

    public GameObject cBulletPrefab;
    public GameObject rBulletPrefab;

    public Transform bulletSpawnPoint;
    public float chargeSpeed = 10f;
    public float maxChargeTime = 3f;
    private float chargeTime = 0f;

    public bool isRifle = true;
    public string rangedType = "charge";

    public float fireRate = 2f;

    private float lastFired;

    private GameObject parti1;
    private GameObject parti2;
    private GameObject parti3;

    public ParticleSystem vfx1;
    public ParticleSystem vfx2;
    public ParticleSystem vfx3;

    private Light lightVFX;
    void Start()
    {
        parti1 = GameObject.Find("VFX1");
        parti2 = GameObject.Find("VFX2");
        parti3 = GameObject.Find("VFX3");

        vfx1 = parti1.GetComponent<ParticleSystem>();
        vfx2 = parti2.GetComponent<ParticleSystem>();
        vfx3 = parti3.GetComponent<ParticleSystem>();

        lightVFX = parti1.GetComponent<Light>();
    }
    void Update()
    {
        if (isRifle)
        {
            switch (rangedType)
            {
                case "charge":
                    chargeRanged();
                    break;
                case "auto":
                    autoRanged();
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (RangedWeapon && MeleeWeapon)
            {
                RangedWeapon.SetActive(!RangedWeapon.activeSelf);
                MeleeWeapon.SetActive(!MeleeWeapon.activeSelf);
                isRifle = !isRifle;
            }
        }
    }

    void chargeRanged()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            chargeTime = 0;

            vfx1.Play();
            vfx2.Play();
            vfx3.Play();
            lightVFX.enabled = true;
        }
        if (Input.GetButton("Fire1"))
        {
            float currentCharge = chargeTime + (Time.deltaTime * chargeSpeed);
            chargeTime = Mathf.Clamp(currentCharge, 0, maxChargeTime);

            float currentRange = lightVFX.range + Time.deltaTime * 5;
            lightVFX.range = Mathf.Clamp(currentRange, 0, 15);
            //        Debug.Log(chargeTime);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            // Spawn bullet when Fire1 is released        
            GameObject bullet = Instantiate(cBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            BulletComponent bulletComp = bullet.GetComponent<BulletComponent>();
            bulletComp.bulletSpeed = chargeTime * 5;
            chargeTime = 0;
            vfx1.Stop();
            vfx2.Stop();
            vfx3.Stop();
            lightVFX.enabled = false;
        }
    }

    void autoRanged()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Time.time - lastFired > 1 / fireRate)
            {
                lastFired = Time.time;
                GameObject bullet = Instantiate(rBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                BulletComponent bulletComp = bullet.GetComponent<BulletComponent>();
                bulletComp.bulletSpeed = 50;
            }
        }
    }
}
