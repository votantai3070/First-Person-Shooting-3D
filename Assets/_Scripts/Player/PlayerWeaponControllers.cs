using UnityEngine;

public class PlayerWeaponControllers : MonoBehaviour
{
    public Player player { private set; get; }
    public PlayerControls controls { private set; get; }

    [Header("Elements")]
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
    }

    void SetupWeapon()
    {
        bulletSpeed = weapon.bulletSpeed;
    }

    public void Shoot()
    {
        if (CanShoot()) return;

        CanShoot(true);

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
        rbBullet.linearVelocity = bulletDirection * bulletSpeed;
    }

    public void CanShoot(bool shoot)
    {
        canShoot = shoot;
    }

    public bool CanShoot() => canShoot;

    void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Fire.performed += ctx =>
        {
            if (!CanShoot())
            {
                Shoot();
                player.anim.SetBool("Shooting", true);
            }
        };
    }
}
