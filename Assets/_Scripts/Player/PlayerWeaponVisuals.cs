using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private WeaponModels[] weaponModels;

    private Transform characterModel;

    public Player player { get; private set; }
    public Rig rig { get; private set; }
    public Transform leftHandIK;
    public Transform leftHandElbow;
    public Transform aim;

    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;

    private WeaponModels currentWeapon;

    private void Awake()
    {
        player = GetComponent<Player>();

        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModels>(true);

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Start()
    {
        if (player.anim != null)
        {
            characterModel = player.anim.transform;
        }

        currentWeapon = GetCurrentWeapon();
        AttachLeftHand();
        SwitchAnimationLayer();
    }

    private void LateUpdate()
    {
        UpdateLeftHandIK();
    }

    public Transform GetPlayerViewPointTransform()
    {
        if (currentWeapon == null)
            currentWeapon = GetCurrentWeapon();

        return currentWeapon?.playerViewPointTransform;
    }

    public void SetRunning(Vector3 worldDirection)
    {
        Vector3 localDirection = Vector3.zero;

        if (characterModel != null && worldDirection.magnitude > 0.01f)
        {
            localDirection = characterModel.InverseTransformDirection(worldDirection);
        }

        bool isRunning = worldDirection.magnitude > 0.01f;

        // Set animator parameters
        player.anim.SetBool("Running", isRunning);
        player.anim.SetFloat("x", localDirection.x, 0.1f, Time.deltaTime);
        player.anim.SetFloat("z", localDirection.z, 0.1f, Time.deltaTime);
    }

    public void PlayFireAnimation() => player.anim.SetTrigger("Shooting");

    private void AttachLeftHand()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("No current weapon!");
            return;
        }

        if (currentWeapon.leftHandIK == null)
        {
            Debug.LogWarning($"Weapon {currentWeapon.name} has no leftHandIK!");
            return;
        }

        if (leftHandIK != null && currentWeapon.leftHandIK != null)
        {
            leftHandIK.localPosition = currentWeapon.leftHandIK.localPosition;
            leftHandIK.localRotation = currentWeapon.leftHandIK.localRotation;
        }

        if (leftHandElbow != null && currentWeapon.leftHandElbow != null)
        {
            leftHandElbow.localPosition = currentWeapon.leftHandElbow.localPosition;
            leftHandElbow.localRotation = currentWeapon.leftHandElbow.localRotation;
        }
    }

    void UpdateLeftHandIK()
    {
        if (currentWeapon == null || !currentWeapon.gameObject.activeInHierarchy)
            return;

        if (leftHandIK != null && currentWeapon.leftHandIK != null)
        {
            leftHandIK.localPosition = currentWeapon.leftHandIK.localPosition;
            leftHandIK.localRotation = currentWeapon.leftHandIK.localRotation;
        }

        if (leftHandElbow != null && currentWeapon.leftHandElbow != null)
        {
            leftHandElbow.localPosition = currentWeapon.leftHandElbow.localPosition;
            leftHandElbow.localRotation = currentWeapon.leftHandElbow.localRotation;
        }
    }

    private void SwitchAnimationLayer()
    {
        if (currentWeapon == null)
            return;

        int layerIndex = (int)currentWeapon.layerAnimationType;

        for (int i = 0; i < player.anim.layerCount; i++)
        {
            player.anim.SetLayerWeight(i, 0);
        }

        if (layerIndex < player.anim.layerCount)
        {
            player.anim.SetLayerWeight(layerIndex, 1);
        }
    }

    public WeaponModels GetCurrentWeapon()
    {
        foreach (var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);

            if (weaponModel.weaponModelType == weaponType)
            {
                weaponModel.gameObject.SetActive(true);
                currentWeapon = weaponModel;
                return weaponModel;
            }
        }

        return null;
    }

    // Debug để check giá trị
    private void OnGUI()
    {
        if (player.anim == null) return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(10, 200, 400, 25), $"Running: {player.anim.GetBool("Running")}", style);
        GUI.Label(new Rect(10, 225, 400, 25), $"X: {player.anim.GetFloat("x"):F2}", style);
        GUI.Label(new Rect(10, 250, 400, 25), $"Z: {player.anim.GetFloat("z"):F2}", style);

        if (characterModel != null)
        {
            GUI.Label(new Rect(10, 275, 400, 25), $"Model Rotation: {characterModel.eulerAngles.y:F1}°", style);
        }
    }
}
