using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { private set; get; }

    private PlayerControls controls;
    private Vector2 mouseDelta;
    public Player player { private set; get; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        player = FindAnyObjectByType<Player>();
    }

    public void Start()
    {
        AssignInputEvents();
    }

    public Vector2 GetMouseDelta() => mouseDelta;

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Look.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
    }
}
