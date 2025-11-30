using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls;
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponVisuals visuals { get; private set; }


    private void Awake()
    {
        controls = new PlayerControls();

        movement = GetComponent<PlayerMovement>();
        visuals = GetComponent<PlayerWeaponVisuals>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
