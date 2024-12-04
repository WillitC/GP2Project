using UnityEngine;

public class AiWeaponHandler : MonoBehaviour
{
    public Transform weaponSocket; // Assign this in the Inspector to the WeaponSocket
    public GameObject meleeWeaponPrefab; // Assign the melee weapon prefab
    public GameObject gunWeaponPrefab; // Assign the gun weapon prefab
    private GameObject currentWeapon; // Store the currently equipped weapon

    // Equip a weapon (either melee or gun)
    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon); // Remove any previously attached weapon
        }

        // Instantiate the new weapon and parent it to the WeaponSocket
        currentWeapon = Instantiate(weaponPrefab, weaponSocket);
        currentWeapon.transform.localPosition = Vector3.zero; // Reset position
        currentWeapon.transform.localRotation = Quaternion.identity; // Reset rotation
        currentWeapon.transform.localScale = Vector3.one;

        //meleeWeaponPrefab.tag = "MeleeWeapon";
        if (currentWeapon.GetComponent<Collider>() == null)
        {
            BoxCollider collider = currentWeapon.AddComponent<BoxCollider>();
            collider.isTrigger = true; // For hit detection
        }
        if (currentWeapon.GetComponent<DamageDealer>() == null)
        {
            currentWeapon.AddComponent<DamageDealer>();
        }
    }
}
