using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private WeaponModelType weaponType;

    private Animator animator;
    public Player player { get; private set; }
    public Rig rig { get; private set; }
    public Transform leftHandIK;
    public Transform leftHandElbow;
    public Transform aim;

    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Start()
    {
        AttachLeftHand();
    }

    private void Update()
    {
    }

    public void SetRunning(Vector3 direction)
    {
        bool isRunning = direction.magnitude > 0;
        animator.SetBool("Running", isRunning);
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.z);
    }

    private void AttachLeftHand()
    {
        Transform leftHandIK = GetCurrentWeapon().leftHandIK;

        this.leftHandIK.localPosition = leftHandIK.localPosition;
        this.leftHandIK.localRotation = leftHandIK.localRotation;
    }

    public WeaponModels GetCurrentWeapon()
    {
        WeaponModels currentWeapon = null;

        WeaponModels[] weaponModels = GetComponentsInChildren<WeaponModels>();

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponModelType == weaponType)
            {
                return currentWeapon = weaponModel;
            }
        }

        return currentWeapon;
    }
}
