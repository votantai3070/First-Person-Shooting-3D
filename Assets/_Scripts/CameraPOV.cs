using Unity.Cinemachine;
using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform playerBody; // Player root
    [SerializeField] private Transform cameraTransform; // Camera holder hoặc camera itself

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Look Limits")]
    [SerializeField] private float minYRotation = -60f; // Nhìn xuống
    [SerializeField] private float maxYRotation = 80f;  // Nhìn lên

    private float verticalRotation = 0f;
    private float currentVerticalVelocity;
    private float smoothVerticalRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Auto-assign camera nếu chưa có
        if (cameraTransform == null)
        {
            cameraTransform = transform;
        }

        // Verify setup
        if (playerBody == null)
        {
            Debug.LogError("Player Body not assigned!");
        }

        Debug.Log($"FPS Camera setup: Camera on '{gameObject.name}', PlayerBody: '{playerBody?.name}'");
    }

    private void LateUpdate()
    {
        ApplyMouseRotation();
    }

    void ApplyMouseRotation()
    {
        if (InputManager.instance == null)
        {
            Debug.LogWarning("InputManager.instance is null!");
            return;
        }

        Vector2 mouseDelta = InputManager.instance.GetMouseDelta();

        if (mouseDelta.magnitude < 0.01f) return;

        // ========== HORIZONTAL (Trái/Phải) - Xoay PLAYER BODY ==========
        float horizontalRotation = mouseDelta.x * mouseSensitivity;
        playerBody.Rotate(Vector3.up * horizontalRotation);

        // ========== VERTICAL (Lên/Xuống) - Xoay CAMERA ==========
        verticalRotation -= mouseDelta.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minYRotation, maxYRotation);

        // Smooth damping
        smoothVerticalRotation = Mathf.SmoothDamp(
            smoothVerticalRotation,
            verticalRotation,
            ref currentVerticalVelocity,
            smoothTime
        );

        // Apply rotation to camera
        cameraTransform.localRotation = Quaternion.Euler(smoothVerticalRotation, 0f, 0f);

        // Debug
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 5f, Color.green);
    }

    private void Update()
    {
        // Toggle cursor lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
                ? CursorLockMode.None
                : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }

    // Debug visualization
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 18;
        style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(10, 10, 400, 30),
            $"Player Y Rotation: {playerBody.eulerAngles.y:F1}°", style);
        GUI.Label(new Rect(10, 35, 400, 30),
            $"Camera X Rotation: {smoothVerticalRotation:F1}°", style);
        GUI.Label(new Rect(10, 60, 400, 30),
            $"Mouse Delta: {InputManager.instance?.GetMouseDelta()}", style);
    }
}
