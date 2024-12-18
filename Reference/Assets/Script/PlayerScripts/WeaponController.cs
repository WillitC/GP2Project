using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Owner { get; set; }

    public GameObject RangedWeapon;
    public GameObject MeleeWeapon;

    public BulletBase cBulletPrefab;
    public BulletBase rBulletPrefab;

    public Transform bulletSpawnPoint;
    public float chargeSpeed = 10f;
    public float maxChargeTime = 3f;
    private float chargeTime = 0f;

    public Transform WeaponParentSocket;

    public bool isRifle = true;
    public string rangedType = "charge";


    public float fireRate = 2f;

    [Header("Weapon Recoil")]
    [Tooltip("This will affect how fast the recoil moves the weapon, the bigger the value, the fastest")]
    public float RecoilSharpness = 50f;

    [Tooltip("Maximum distance the recoil can affect the weapon")]
    public float MaxRecoilDistance = 0.5f;

    [Tooltip("How fast the weapon goes back to it's original position after the recoil is finished")]
    public float RecoilRestitutionSharpness = 10f;

    [Header("Misc")]
    [Tooltip("Speed at which the aiming animatoin is played")]
    public float AimingAnimationSpeed = 10f;

    [Tooltip("Field of view when not aiming")]
    public float DefaultFov = 60f;

    [Tooltip("Portion of the regular FOV to apply to the weapon camera")]
    public float WeaponFovMultiplier = 1f;

    [Tooltip("Delay before switching weapon a second time, to avoid recieving multiple inputs from mouse wheel")]
    public float WeaponSwitchDelay = 1f;

    private float spread = 7.5f; 

    Vector3 m_LastCharacterPosition;
    Vector3 m_WeaponMainLocalPosition;
    Vector3 m_WeaponBobLocalPosition;
    Vector3 m_WeaponRecoilLocalPosition;
    Vector3 m_AccumulatedRecoil;

    public Vector3 MuzzleWorldVelocity { get; private set; }

    Vector3 m_LastMuzzlePosition;



    private float lastFired;

    private GameObject parti1;
    private GameObject parti2;
    private GameObject parti3;

    public ParticleSystem vfx1;
    public ParticleSystem vfx2;
    public ParticleSystem vfx3;

    public AudioSource[] fireSFX;

    private Light lightVFX;

    public GameObject muzzleFlash;

    void Start()
    {
        Owner = gameObject;

        parti1 = GameObject.Find("VFX1");
        parti2 = GameObject.Find("VFX2");
        parti3 = GameObject.Find("VFX3");

        vfx1 = parti1.GetComponent<ParticleSystem>();
        vfx2 = parti2.GetComponent<ParticleSystem>();
        vfx3 = parti3.GetComponent<ParticleSystem>();

        lightVFX = parti1.GetComponent<Light>();

        RangedWeapon.SetActive(true);
        MeleeWeapon.SetActive(false);

        m_LastMuzzlePosition = bulletSpawnPoint.position;

    }
    void Update()
    {

        if (Time.deltaTime > 0)
        {
            MuzzleWorldVelocity = (bulletSpawnPoint.position - m_LastMuzzlePosition) / Time.deltaTime;
            m_LastMuzzlePosition = bulletSpawnPoint.position;
        }

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
                    singleRanged();
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (RangedWeapon && MeleeWeapon)
            {
                RangedWeapon.SetActive(!RangedWeapon.activeSelf);
                MeleeWeapon.SetActive(!MeleeWeapon.activeSelf);
                isRifle = !isRifle;
            }
        }
    }

    void LateUpdate()
    {
        UpdateWeaponRecoil();

        WeaponParentSocket.localPosition = m_WeaponMainLocalPosition + m_WeaponBobLocalPosition + m_WeaponRecoilLocalPosition;
    }

    public void singleRanged()
    {
        if (Input.GetButtonDown("Fire1") && !gameObject.GetComponent<IFRAME>())
        {
            {
                m_AccumulatedRecoil += Vector3.back * 1.15f;
                m_AccumulatedRecoil = Vector3.ClampMagnitude(m_AccumulatedRecoil, MaxRecoilDistance);
            }
            GameObject flash = Instantiate(muzzleFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Destroy(flash, 3);
            for (int i = 0; i < 35; i++)
            {
                Vector3 bulletDirection = spreadAngle(bulletSpawnPoint);
                
                BulletBase bullet = Instantiate(rBulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(bulletDirection));
                BulletComponent bulletComp = bullet.GetComponent<BulletComponent>();
                bulletComp.bulletSpeed = 175;
                bullet.Fire(this);

            }
        }
    }

    public void chargeRanged()
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
            /*GameObject bullet = Instantiate(cBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            BulletComponent bulletComp = bullet.GetComponent<BulletComponent>();
            bulletComp.bulletSpeed = chargeTime * 5;
            chargeTime = 0;
            vfx1.Stop();
            vfx2.Stop();
            vfx3.Stop();
            lightVFX.enabled = false;*/
        }
    }

    public void autoRanged()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Time.time - lastFired > 1 / fireRate)
            {
                {
                    m_AccumulatedRecoil += Vector3.back * 0.05f;
                    m_AccumulatedRecoil = Vector3.ClampMagnitude(m_AccumulatedRecoil, MaxRecoilDistance);
                }
                lastFired = Time.time;
                float rngSpread()
                {
                    float result = (Random.Range(-spread, spread));
                    print(result + " | " + spread);
                    return result;
                }
                Quaternion rot = bulletSpawnPoint.rotation;
                rot *= Quaternion.Euler(rngSpread() ,rngSpread(), 0);
                BulletBase bullet = Instantiate(rBulletPrefab, bulletSpawnPoint.position, rot);
                BulletComponent bulletComp = bullet.GetComponent<BulletComponent>();
                bulletComp.bulletSpeed = 75;
                bullet.Fire(this);
            }
        }
    }

    void UpdateWeaponRecoil()
    {
        // if the accumulated recoil is further away from the current position, make the current position move towards the recoil target
        if (m_WeaponRecoilLocalPosition.z >= m_AccumulatedRecoil.z * 0.29f)
        {
            m_WeaponRecoilLocalPosition = Vector3.Lerp(m_WeaponRecoilLocalPosition, m_AccumulatedRecoil,
                RecoilSharpness * Time.deltaTime);
        }
        // otherwise, move recoil position to make it recover towards its resting pose
        else
        {
            m_WeaponRecoilLocalPosition = Vector3.Lerp(m_WeaponRecoilLocalPosition, Vector3.zero,
                RecoilRestitutionSharpness * Time.deltaTime);
            m_AccumulatedRecoil = m_WeaponRecoilLocalPosition;
        }
    }

    public Vector3 spreadAngle(Transform tip)
    {
        float ratio = spread / 180f;
        Vector3 angle = Vector3.Slerp(tip.forward, UnityEngine.Random.insideUnitSphere,
            ratio);

        return angle;
    }
}
