using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    [Header("Weapons")]
    [SerializeField] Weapon currentWeapon;
    [SerializeField] List<Weapon> weaponSlots;

    [Header("Refferences")]
    [SerializeField] Animator playerAnimator;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerAim playerAim;
    [SerializeField] PlayerWeaponVisuals playerWeaponVisuals;

    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform gunPoint;

    [Header("Weapon Debug")]
    [SerializeField] Transform weaponHolder;
    [SerializeField] Transform aim;

    private const float REFERRECE_BULLET_SPEED = 20f;
    //Formula to calculate bullet mass

    void Start()
    {
        AssignInputEvents();
    }

    private void AssignInputEvents()
    {
        playerInput.PlayerActionsControls.Character.Fire.performed += context => Shoot();
        playerInput.PlayerActionsControls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        playerInput.PlayerActionsControls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        playerInput.PlayerActionsControls.Character.DropCurrentWeapon.performed += context => DropWeapon();
        playerInput.PlayerActionsControls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload()) { 
                Reload();
                currentWeapon.ReloadBullets();
            }
        };
    }

    private void Reload()
    {
        playerWeaponVisuals.PlayReloadAnimation();
    }

    void EquipWeapon(int index)
    {
        currentWeapon = weaponSlots[index];
    }
    void DropWeapon()
    {
        if (weaponSlots.Count <= 1)
            return;

        weaponSlots.Remove(currentWeapon);
        currentWeapon = weaponSlots[0];
    }

    public void PickupWeapon(Weapon weapon)
    {
        weaponSlots.Add(weapon);
    }

    private void Shoot()
    {

        if (!currentWeapon.CanShoot())
            return;

        Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        bullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;
        bullet.GetComponent<Rigidbody>().mass = REFERRECE_BULLET_SPEED / bulletSpeed;

        playerAnimator.SetTrigger("Fire");
    }

    public Vector3 BulletDirection()
    {
        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (playerAim.Target() == null)
            direction.y = 0;

        return direction;
    }

    public Transform GunPoint()
    {
        return gunPoint;
    }
}
