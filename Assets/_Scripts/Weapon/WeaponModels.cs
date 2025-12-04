using UnityEngine;

public enum LayerAnimationType { RifleBody = 1, PistolBody }

public enum PlayerViewPoint { PistolAim, RifleAim }


public class WeaponModels : MonoBehaviour
{
    public Weapon_SO weaponData;

    public WeaponType weaponModelType;
    public LayerAnimationType layerAnimationType;
    public PlayerViewPoint playerViewPoint;

    public Transform leftHandIK;
    public Transform leftHandElbow;
    public Transform playerViewPointTransform;
    public Transform gunPoint;

    public float gunDistance = 100f;

}
