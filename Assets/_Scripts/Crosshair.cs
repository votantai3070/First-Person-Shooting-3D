using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image crosshairImage;

    [Header("Crosshair Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color enemyColor = Color.red;
    [SerializeField] private float crosshairSize = 32f;

    [Header("Dynamic Spread (Optional)")]
    [SerializeField] private bool useDynamicSpread = false;
    [SerializeField] private float minSpread = 32f;
    [SerializeField] private float maxSpread = 64f;

    private Camera playerCamera;
    private RectTransform crosshairRect;

    private void Start()
    {
        playerCamera = Camera.main;

        if (crosshairImage != null)
        {
            crosshairRect = crosshairImage.GetComponent<RectTransform>();
        }

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateCrosshairColor();

        if (useDynamicSpread)
        {
            UpdateCrosshairSpread();
        }
    }

    void UpdateCrosshairColor()
    {
        if (crosshairImage == null) return;

        // Raycast từ center màn hình
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        //if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        //{
        //    // Đổi màu nếu aim vào enemy
        //    if (hit.collider.CompareTag("Enemy"))
        //    {
        //        crosshairImage.color = enemyColor;
        //    }
        //    else
        //    {
        //        crosshairImage.color = normalColor;
        //    }
        //}
        //else
        //{
        //    crosshairImage.color = normalColor;
        //}
    }

    void UpdateCrosshairSpread()
    {
        // Tăng spread khi di chuyển (optional)
        Vector2 moveInput = InputManager.instance.GetMovementInput();
        bool isMoving = moveInput.magnitude > 0.1f;

        float targetSize = isMoving ? maxSpread : minSpread;
        float currentSize = crosshairRect.sizeDelta.x;
        float newSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * 10f);

        crosshairRect.sizeDelta = new Vector2(newSize, newSize);
    }

    // Toggle crosshair visibility
    public void SetVisible(bool visible)
    {
        crosshairImage.enabled = visible;
    }
}
