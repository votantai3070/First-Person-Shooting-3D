using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { private set; get; }

    private PlayerControls controls;
    public Player player { private set; get; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Start()
    {
        controls = player.controls;

        if (controls == null)
        {
            Debug.LogError("PlayerControls is NULL!");
            return;
        }
    }

    public Vector2 GetMovementInput()
    {
        if (controls == null)
        {
            Debug.LogWarning("Controls is null!");
            return Vector2.zero;
        }
        return controls.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        if (controls == null)
        {
            Debug.LogWarning("Controls is null!");
            return Vector2.zero;
        }

        return controls.Player.Look.ReadValue<Vector2>();
    }
}
