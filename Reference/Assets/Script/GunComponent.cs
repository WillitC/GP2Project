using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent : MonoBehaviour
{
    public GameObject RangedWeapon;
    public GameObject MeleeWeapon;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float chargeSpeed = 10f;
    public float maxChargeTime = 3f;
    private float chargeTime = 0f;

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
            
            float currentRange = lightVFX.range + Time.deltaTime*5;
            lightVFX.range = Mathf.Clamp(currentRange, 0, 15);
    //        Debug.Log(chargeTime);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            // Spawn bullet when Fire1 is released        
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            BulletComponent bulletComp = bullet.GetComponent<BulletComponent>();
            bulletComp.bulletSpeed = chargeTime*5;
            chargeTime = 0;
            vfx1.Stop();
            vfx2.Stop();
            vfx3.Stop();
            lightVFX.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (RangedWeapon && MeleeWeapon)
            {
                RangedWeapon.SetActive(!RangedWeapon.activeSelf);
                MeleeWeapon.SetActive(!MeleeWeapon.activeSelf);
            }
        }
    }
}
