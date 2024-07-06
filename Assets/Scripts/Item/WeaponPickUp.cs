using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] public Weapon weaponItem;


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weaponItem); 
    }
}
