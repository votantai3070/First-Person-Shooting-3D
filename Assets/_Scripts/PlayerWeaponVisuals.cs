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
        SwitchAnimationLayer();
    }

    public Transform GetPlayerViewPointTransform()
    {
        return GetCurrentWeapon().playerViewPointTransform;
    }

    public void SetRunning(Vector3 direction)
    {
        bool isRunning = direction.magnitude > 0.01;

        float xVelocity = Vector3.Dot(direction.normalized, transform.right);
        float zVelocity = Vector3.Dot(direction.normalized, transform.forward);

        animator.SetBool("Running", isRunning);
        animator.SetFloat("x", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("z", zVelocity, .1f, Time.deltaTime);
    }

    private void AttachLeftHand()
    {
        Transform leftHandIK = GetCurrentWeapon().leftHandIK;
        Transform leftHandElbow = GetCurrentWeapon().leftHandElbow;

        this.leftHandIK.localPosition = leftHandIK.localPosition;
        this.leftHandIK.localRotation = leftHandIK.localRotation;

        this.leftHandElbow.localPosition = leftHandElbow.localPosition;
        this.leftHandElbow.localRotation = leftHandElbow.localRotation;

    }

    private void SwitchAnimationLayer()
    {
        int layerIndex = (int)GetCurrentWeapon().layerAnimationType;

        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }

    public WeaponModels GetCurrentWeapon()
    {
        WeaponModels currentWeapon = null;

        WeaponModels[] weaponModels = GetComponentsInChildren<WeaponModels>();

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponModelType == weaponType)
            {
                return weaponModel;
            }
        }

        return currentWeapon;
    }
}
