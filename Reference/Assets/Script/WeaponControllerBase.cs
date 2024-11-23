using UnityEngine;

public abstract class WeaponControllerBase : MonoBehaviour
{
    public GameObject RangedWeapon;
    public GameObject MeleeWeapon;
    public Transform BulletSpawnPoint;

    public abstract void Attack();
}
