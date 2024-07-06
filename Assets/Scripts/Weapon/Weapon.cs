using System;

public enum WeaponType
{
    Pistol, Revolver, AutoRifle, Shotgun, Rifle
}

[Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public string name;
    public int bulletsInMagazine;
    public int totalReservedAmmo;
    public int magazineCapacity;


    public bool CanShoot()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }
        return false;

    }

    public bool CanReload()
    {
        if (totalReservedAmmo > 0)
        {
            return true;
        }

        return false;
    }

    public void ReloadBullets()
    {
        int bulletsToReload = magazineCapacity;
        if (bulletsToReload > totalReservedAmmo)
            bulletsToReload = totalReservedAmmo;

        totalReservedAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if(totalReservedAmmo < 0)
            totalReservedAmmo = 0;
    }
}
