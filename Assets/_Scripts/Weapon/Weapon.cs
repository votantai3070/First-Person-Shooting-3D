using UnityEngine;

public enum WeaponType { M1991, Uzi, M4, AK74, Bennel_M4, M249, M107, RPG7 }

[System.Serializable]
public class Weapon
{
    [Header("Info weapon")]
    public float bulletSpeed;

    [Header("Spread")]
    [SerializeField] private float baseSpread;
    [SerializeField] private float maximumSpread;
    private float currentSpread;

    private float spreadIncreaseRate = .15f;
    private float lastSpreadUpdateTime;
    private float spreadCooldown = 1;

    public Weapon_SO weaponData;

    public Weapon(Weapon_SO weapon)
    {
        weaponData = weapon;
        this.bulletSpeed = weapon.bulletSpeed;
        this.baseSpread = weapon.baseSpreadAngle;
        this.maximumSpread = weapon.maximumSpreadAngle;
    }

    #region Spread methods

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

        lastSpreadUpdateTime = Time.time;
    }

    public void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }

    #endregion

}
