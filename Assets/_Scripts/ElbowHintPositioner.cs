using UnityEngine;

public class ElbowHintPositioner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform shoulder; // arm_stretch.l
    [SerializeField] private Transform hand; // hand.l
    [SerializeField] private Transform hint; // LeftHandIK_Hint

    [Header("Settings")]
    [SerializeField] private float hintDistance = 0.3f; // Khoảng cách hint từ mid point
    [SerializeField] private Vector3 hintDirection = new Vector3(-1, 0, 0.5f); // Local direction

    private void LateUpdate()
    {
        if (shoulder == null || hand == null || hint == null) return;

        // Mid point giữa shoulder và hand
        Vector3 midPoint = (shoulder.position + hand.position) / 2f;

        // Direction cho hint (thường là outward từ body)
        Vector3 direction = transform.TransformDirection(hintDirection.normalized);

        // Set hint position
        hint.position = midPoint + direction * hintDistance;
    }
}
