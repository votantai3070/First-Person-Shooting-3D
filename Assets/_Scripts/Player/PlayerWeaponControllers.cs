using System.Collections;
using UnityEngine;

public class PlayerWeaponControllers : MonoBehaviour
{
    public Player player { private set; get; }
    public PlayerControls controls { private set; get; }

    [Header("Elements")]
    private float averageMass;
    private bool weaponReady;
    private bool isShooting;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float distanceShot = 1000f;
    private Transform gunPoint;

    WeaponModels currentWeaponModel;
    Weapon_SO weaponData;
    [SerializeField] Weapon weapon;
    bool canShoot;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        AssignInputEvents();

        currentWeaponModel = player.visuals.GetCurrentWeapon();
        weaponData = player.visuals.GetCurrentWeapon().weaponData;
        weapon = new Weapon(weaponData);

        gunPoint = currentWeaponModel.gunPoint;

        SetupWeapon();

        SetWeaponReady(true);
    }

    private void Update()
    {
        if (isShooting)
            Shoot();
    }

    void SetupWeapon()
    {
        bulletSpeed = weapon.bulletSpeed;
        bulletPrefab = weapon.bulletPrefab;
        averageMass = weapon.impactForce;
    }

    private void Shoot()
    {
        if (!WeaponReady()) return;

        if (!weapon.CanShoot()) return;

        if (weapon.shootType == ShootType.Single)
            isShooting = false;

        player.visuals.PlayFireAnimation();

        if (weapon.BurstActivated())
        {
            Debug.Log("Vao day!");
            StartCoroutine(BurstFire());
            return;
        }

        FireSingleBullet();
    }

    IEnumerator BurstFire()
    {
        Debug.Log("Vao day!!");
        SetWeaponReady(false);
        Debug.Log("Vao day!!!");

        for (int i = 1; i <= weapon.bulletsPerShot; i++)
        {
            FireSingleBullet();
            yield return new WaitForSeconds(weapon.burstFireDelay);

            if (i >= weapon.bulletsPerShot)
                SetWeaponReady(true);
        }
    }

    public void FireSingleBullet()
    {
        weapon.bulletsInMagazine--;

        // Raycast từ center màn hình để tìm điểm bắn
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // center screen
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, distanceShot)) // 100f là range tối đa
        {
            targetPoint = hit.point; // Nếu raycast hit object
        }
        else
        {
            targetPoint = ray.GetPoint(distanceShot); // Nếu không hit, lấy điểm xa về phía trước
        }

        // Tính direction từ gunPoint đến targetPoint
        Vector3 direction = (targetPoint - gunPoint.position).normalized;

        // Spread
        Vector3 bulletDirection = weapon.ApplySpread(direction);

        // Spawn bullet và bắn
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position,
     Quaternion.LookRotation(bulletDirection) * Quaternion.Euler(90, 0, 0));
        Rigidbody rbBullet = newBullet.GetComponent<Rigidbody>();
        rbBullet.mass = averageMass / bulletSpeed;
        rbBullet.linearVelocity = bulletDirection * bulletSpeed;
    }

    IEnumerator ReloadWeapon()
    {
        player.visuals.PlayReloadAnimation();

        yield return new WaitForSeconds(weapon.reloadTime);

        weapon.RefillBullets();
    }

    public void SetWeaponReady(bool ready) => weaponReady = ready;
    public bool WeaponReady() => weaponReady;

    public Weapon CurrentWeapon() => weapon;

    void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Fire.performed += ctx => isShooting = true;
        controls.Player.Fire.canceled += ctx => isShooting = false;

        controls.Player.BurstMode.performed += ctx => weapon.ToggleBurst();
        controls.Player.Reload.performed += ctx => StartCoroutine(ReloadWeapon());
    }
}
