using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public Player player { get; private set; }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void ShootingEnd()
    {
        player.controller.CanShoot(false);
        player.anim.SetBool("Shooting", false);
    }
}
