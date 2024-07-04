using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] PlayerInput playerInput;

    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform gunPoint;

    [Header("Weapon Debug")]
    [SerializeField] Transform weaponHolder;
    [SerializeField] Transform aim;


    void Start()
    {
        playerInput.PlayerActionsControls.Character.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        bullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;

        playerAnimator.SetTrigger("Fire");
    }

    Vector3 BulletDirection()
    {
        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);

        Vector3 direction = (aim.position - gunPoint.position).normalized;
        direction.y = 0;

        return direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
        Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
        Gizmos.color = Color.red;
    }

}
