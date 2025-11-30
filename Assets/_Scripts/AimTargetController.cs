using UnityEngine;

public class AimTargetController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform aimTarget; // AimTarget GameObject
    [SerializeField] private Camera playerCamera;

    [Header("Settings")]
    [SerializeField] private float maxDistance = 1000f;
    [SerializeField] private float defaultDistance = 50f;
    [SerializeField] private LayerMask aimMask = ~0;

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        UpdateAimTarget();
    }

    void UpdateAimTarget()
    {
        if (aimTarget == null) return;

        // Raycast từ center screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, aimMask))
        {
            aimTarget.position = hit.point;
            Debug.DrawLine(playerCamera.transform.position, hit.point, Color.green);
        }
        else
        {
            aimTarget.position = ray.origin + ray.direction * defaultDistance;
            Debug.DrawRay(playerCamera.transform.position, ray.direction * defaultDistance, Color.red);
        }
    }
}
