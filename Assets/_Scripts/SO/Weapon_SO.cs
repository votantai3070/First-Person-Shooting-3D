using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Weapon SO/Weapon")]
public class Weapon_SO : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject bulletPrefab;

    [Header("Shooting Stats")]
    public float damage;
    public float fireRate; // Số phát/giây hoặc delay giữa mỗi shot
    public float bulletSpeed;
    public float shootRange;
    public float impactForce;
    public int bulletsPerShot;
    public int magazineSize;
    public int maxAmmo;

    [Header("Burst")]
    public bool burstAvailable;
    public bool burstActive;
    public int burstBulletsPerShot;
    public float burstFireDelay;

    [Header("Recoil & Accuracy")]
    public float recoilForce;
    public float baseSpreadAngle;
    public float maximumSpreadAngle;

    [Header("Reload")]
    public float reloadTime;

    [Header("Effects")]
    public GameObject muzzleFlashEffect;
    public GameObject bulletImpactEffect;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    [Header("Weapon Type")]
    public WeaponType weaponType;

    [Header("Weapon Shoot Type")]
    public ShootType shootType;
}

