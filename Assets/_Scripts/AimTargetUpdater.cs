using UnityEngine;

public class AimTargetUpdater : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform aimTarget; // AimTarget GameObject
    [SerializeField] private Camera playerCamera;

    [Header("Settings")]
    [SerializeField] private LayerMask aimLayerMask = ~0; // Aim vào all layers
    [SerializeField] private float maxAimDistance = 1000f;
    [SerializeField] private float defaultAimDistance = 50f; // Nếu không hit gì

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        UpdateAimTargetPosition();
    }

    void UpdateAimTargetPosition()
    {
        if (aimTarget == null || playerCamera == null) return;

        // Raycast từ center màn hình (0.5, 0.5)
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPosition;

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimLayerMask))
        {
            // Aim vào điểm raycast hit
            targetPosition = hit.point;

            // Debug
            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
        else
        {
            // Không hit gì - aim về phía trước xa
            targetPosition = ray.origin + ray.direction * defaultAimDistance;

            // Debug
            Debug.DrawLine(ray.origin, targetPosition, Color.red);
        }

        // Update aim target position
        aimTarget.position = targetPosition;
    }
}
