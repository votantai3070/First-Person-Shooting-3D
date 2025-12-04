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
        Debug.Log("Can Shoot");

        player.attack.CanShoot(false);
        player.anim.SetBool("Shooting", false);
    }
}
