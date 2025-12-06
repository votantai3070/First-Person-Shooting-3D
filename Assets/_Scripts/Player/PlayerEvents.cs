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
        player.anim.SetBool("Shooting", false);
    }

    public void ReloadIsOver()
    {
        player.visuals.MaximizeRigWeight();
        //Refill bullets
        player.controller.CurrentWeapon().RefillBullets();

        player.controller.SetWeaponReady(true);
    }

    public void ReturnRig()
    {
        player.visuals.MaximizeRigWeight();
        player.visuals.MaximizeLeftHandWeight();
    }
}
