using UnityEngine;

public enum WeaponModelType { M1991, Uzi, M4, AK74, Bennel_M4, M249, M107, RPG7 }
public enum LayerAnimationType { RifleBody = 1, PistolBody }

public enum PlayerViewPoint { PistolAim, RifleAim }


public class WeaponModels : MonoBehaviour
{
    public WeaponModelType weaponModelType;
    public LayerAnimationType layerAnimationType;
    public PlayerViewPoint playerViewPoint;

    public Transform leftHandIK;
    public Transform leftHandElbow;
    public Transform playerViewPointTransform;
    public Transform gunPoint;
    public float gunDistance = 100f;

}
