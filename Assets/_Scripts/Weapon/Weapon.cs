using UnityEngine;

public enum WeaponType { M1991, Uzi, M4, AK74, Bennel_M4, M249, M107, RPG7 }

public enum ShootType { Single, Auto, }

[System.Serializable]
public class Weapon
{
    [Header("Info weapon")]
    public WeaponType weaponType;
    public ShootType shootType;

    public float reloadTime;
    public float reloadSpeed;

    public int bulletsPerShot { get; private set; }
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public float impactForce;

    #region Regular mode variables
    private float defaulFireRate;
    public float fireRate = 1; //bullets per second
    private float lastShootTime;
    #endregion

    #region Burst mode variables
    [SerializeField] private bool burstAvailable;
    [SerializeField] private bool burstActive;
    private int burstBulletsPerShot;
    private float burstFireRate;
    public float burstFireDelay { get; private set; }
    #endregion

    [Header("Spread")]
    [SerializeField] private float baseSpread;
    [SerializeField] private float maximumSpread;
    [SerializeField] private float currentSpread;

    private float spreadIncreaseRate = .15f;
    private float lastSpreadUpdateTime;
    private float spreadCooldown = 1;

    [Header("Magazine details")]
    public int bulletsInMagazine; // Current Bullet
    public int magazineCapacity; // Sức chứa băng đạn
    public int totalReserveAmmo; // Số đạn còn lại

    public Weapon_SO weaponData;

    public Weapon(Weapon_SO weapon)
    {
        weaponData = weapon;
        bulletSpeed = weapon.bulletSpeed;
        baseSpread = weapon.baseSpreadAngle;
        maximumSpread = weapon.maximumSpreadAngle;
        bulletPrefab = weapon.bulletPrefab;
        impactForce = weapon.impactForce;

        reloadTime = weapon.reloadTime;

        fireRate = weapon.fireRate;

        bulletsPerShot = weapon.bulletsPerShot;
        shootType = weapon.shootType;
        bulletsInMagazine = weapon.bulletsInMagazine;
        magazineCapacity = weapon.magazineCapacity;
        totalReserveAmmo = weapon.totalReserveAmmo;

        burstAvailable = weapon.burstAvailable;
        burstActive = weapon.burstActive;
        burstBulletsPerShot = weapon.burstBulletsPerShot;
        burstFireRate = weapon.burstFireRate;
        burstFireDelay = weapon.burstFireDelay;

        weaponType = weapon.weaponType;
        shootType = weapon.shootType;

        reloadSpeed = weapon.reloadSpeed;
    }

    #region Burst methods

    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Bennel_M4)
        {
            burstFireDelay = 0;
            return true;
        }

        return burstActive;
    }

    public void ToggleBurst()
    {
        if (!burstAvailable) return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaulFireRate;
        }
    }

    #endregion

    public bool CanShoot()
    {
        if (HaveEnoughBullets() && ReadyToFire())
        {
            return true;
        }

        return false;
    }

    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }

        return false;
    }

    #region Reload methods
    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
            return false;

        if (totalReserveAmmo > 0)
            return true;

        return false;
    }

    public void RefillBullets()
    {
        int bulletsSpent = magazineCapacity - bulletsInMagazine;

        int bulletsToReload = Mathf.Min(magazineCapacity, totalReserveAmmo);

        totalReserveAmmo -= bulletsSpent;
        bulletsInMagazine = bulletsToReload;

        if (totalReserveAmmo <= 0)
            totalReserveAmmo = 0;
    }

    private bool HaveEnoughBullets() => bulletsInMagazine > 0;

    #endregion

    #region Spread methods

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

        lastSpreadUpdateTime = Time.time;
    }

    public void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }

    #endregion

}
