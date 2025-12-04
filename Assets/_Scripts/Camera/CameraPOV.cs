using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform cameraTarget;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private bool invertY = false;

    [Header("Vertical Limits")]
    [SerializeField] private float minVerticalAngle = -30f; // Nhìn xuống
    [SerializeField] private float maxVerticalAngle = 70f;  // Nhìn lên

    private CinemachineOrbitalFollow orbitalFollow;
    private CinemachineThirdPersonFollow thirdPersonFollow;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;

    private void Start()
    {
        // Auto-find components
        if (virtualCamera == null)
        {
            virtualCamera = FindAnyObjectByType<CinemachineCamera>();
        }

        if (virtualCamera != null)
        {
            orbitalFollow = virtualCamera.GetComponent<CinemachineOrbitalFollow>();
            thirdPersonFollow = virtualCamera.GetComponent<CinemachineThirdPersonFollow>();

            if (cameraTarget == null)
            {
                cameraTarget = virtualCamera.Follow;
            }
        }

        // Initialize rotations
        if (cameraTarget != null)
        {
            horizontalRotation = cameraTarget.eulerAngles.y;
            verticalRotation = cameraTarget.eulerAngles.x;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateCameraRotation();
        HandleCursorToggle();
    }

    void UpdateCameraRotation()
    {
        if (cameraTarget == null) return;

        Vector2 mouseDelta = InputManager.instance.GetMouseDelta();

        if (mouseDelta.magnitude < 0.001f) return;

        // ========== HORIZONTAL (Trái/Phải) ==========
        horizontalRotation += mouseDelta.x * mouseSensitivity * Time.deltaTime;

        // ========== VERTICAL (Lên/Xuống) ==========
        float verticalInput = mouseDelta.y * mouseSensitivity * Time.deltaTime;
        if (invertY) verticalInput = -verticalInput;

        verticalRotation -= verticalInput;

        // Clamp vertical rotation
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        // Apply rotation to camera target
        cameraTarget.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool locked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = locked;
        }
    }

    // Debug visualization
    private void OnGUI()
    {
        if (cameraTarget == null) return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(10, 10, 300, 30),
            $"Horizontal: {horizontalRotation:F1}°", style);
        GUI.Label(new Rect(10, 35, 300, 30),
            $"Vertical: {verticalRotation:F1}°", style);
        GUI.Label(new Rect(10, 60, 300, 30),
            $"Camera Rotation: {cameraTarget.eulerAngles}", style);
    }
}
