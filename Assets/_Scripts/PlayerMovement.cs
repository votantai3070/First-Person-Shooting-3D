using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerControls controls;
    Player player;

    Vector2 moveInput;
    Vector3 moveDirection;
    CharacterController characterController;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;

    private void Awake()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();

        // Lock và ẩn cursor cho FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        AssignInput();
    }

    private void Update()
    {
        ApplyMovement();
    }

    void ApplyMovement()
    {
        // Movement relative to camera direction
        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        moveDirection = (forward + right).normalized;

        characterController.Move(moveSpeed * Time.deltaTime * moveDirection);

        // Gravity (nếu cần)
        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
        }

        player.visuals.SetRunning(moveDirection);
    }

    void AssignInput()
    {
        controls = player.controls;
        controls.Enable(); // QUAN TRỌNG!

        // Movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

    }
}
