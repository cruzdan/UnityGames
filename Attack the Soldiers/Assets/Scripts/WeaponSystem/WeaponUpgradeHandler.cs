using UnityEngine;

public class WeaponUpgradeHandler : MonoBehaviour
{
    #region Serialized Variables
    [Header("References")]
    [SerializeField] private WeaponLevelManager levelManager;
    [SerializeField] private WeaponLevelConfig levelConfig;
    [SerializeField] private BulletInfoSO bulletInfoSO;
    #endregion

    #region Private Variables
    private WeaponLevelData currentWeaponData;
    #endregion

    #region Public Properties
    public WeaponLevelData CurrentWeaponData => currentWeaponData;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        if (levelManager == null)
        {
            levelManager = FindAnyObjectByType<WeaponLevelManager>();
        }

        if (levelConfig == null && levelManager != null)
        {
            levelConfig = levelManager.LevelConfig;
        }

        if (bulletInfoSO == null)
        {
            bulletInfoSO = FindAnyObjectByType<NetworkGameManager>().BulletInfoSO;
        }
    }
    #endregion

    #region Upgrade Application
    public void LoadWeaponUpgrades(Weapon weapon)
    {
        if (levelManager == null) return;

        currentWeaponData = levelManager.GetWeaponData(weapon);
    }

    public float GetModifiedDamage(Weapon weapon, float baseDamage)
    {
        if (levelManager == null) return baseDamage;
        if (currentWeaponData == null || currentWeaponData.weaponType != weapon)
        {
            LoadWeaponUpgrades(weapon);
        }

        float damageIncrease = currentWeaponData.totalDamageUpgrades * levelConfig.WeaponUpgradeValues[(int)weapon].DamageIncrease;
        
        WeaponSpecificConfig config = levelConfig.GetConfigForWeapon(weapon);
        float configMultiplier = config != null ? config.DamageMultiplier : 1f;

        float finalDamage = (baseDamage + damageIncrease) * configMultiplier;
        
        return finalDamage;
    }

    public float GetModifiedTimeToShoot(Weapon weapon, float baseTimeToShoot)
    {
        if (levelManager == null) return baseTimeToShoot;
        if (currentWeaponData == null || currentWeaponData.weaponType != weapon)
        {
            LoadWeaponUpgrades(weapon);
        }

        float timeDecrease = currentWeaponData.totalTimeToShootUpgrades * levelConfig.WeaponUpgradeValues[(int)weapon].TimeToShootDecrease;
        
        WeaponSpecificConfig config = levelConfig.GetConfigForWeapon(weapon);
        float configMultiplier = config != null ? config.TimeToShootMultiplier : 1f;

        float finalTime = (baseTimeToShoot - timeDecrease) * configMultiplier;
        finalTime = Mathf.Max(finalTime, 0.05f);

        return finalTime;
    }

    public float GetModifiedMaxDistance(Weapon weapon, float baseMaxDistance)
    {
        if (levelManager == null) return baseMaxDistance;
        if (currentWeaponData == null || currentWeaponData.weaponType != weapon)
        {
            LoadWeaponUpgrades(weapon);
        }

        float distanceIncrease = currentWeaponData.totalMaxDistanceUpgrades * levelConfig.WeaponUpgradeValues[(int)weapon].MaxDistanceIncrease;
        
        WeaponSpecificConfig config = levelConfig.GetConfigForWeapon(weapon);
        float configMultiplier = config != null ? config.DistanceMultiplier : 1f;

        float finalDistance = (baseMaxDistance + distanceIncrease) * configMultiplier;
        
        return finalDistance;
    }

    public int GetUpgradeCount(Weapon weapon, WeaponUpgradeType upgradeType)
    {
        if (levelManager == null) return 0;
        if (currentWeaponData == null || currentWeaponData.weaponType != weapon)
        {
            LoadWeaponUpgrades(weapon);
        }

        switch (upgradeType)
        {
            case WeaponUpgradeType.Damage:
                return currentWeaponData.totalDamageUpgrades;
            case WeaponUpgradeType.TimeToShoot:
                return currentWeaponData.totalTimeToShootUpgrades;
            case WeaponUpgradeType.MaxDistance:
                return currentWeaponData.totalMaxDistanceUpgrades;
            default:
                return 0;
        }
    }

    public bool CanUpgrade(Weapon weapon, WeaponUpgradeType upgradeType)
    {
        if (levelManager == null) return false;
        if (levelConfig == null) return false;
        if (currentWeaponData == null || currentWeaponData.weaponType != weapon)
        {
            LoadWeaponUpgrades(weapon);
        }

        int upgradeCount = GetUpgradeCount(weapon, upgradeType);
        return upgradeCount < levelConfig.MaxLevel;
    }
    #endregion

    #region Public Methods
    public void ApplyUpgrade(Weapon weapon, WeaponUpgradeType upgradeType)
    {
        if (levelManager == null) return;

        levelManager.ApplyUpgrade(weapon, upgradeType);
        LoadWeaponUpgrades(weapon);
    }

    public void RefreshWeaponData(Weapon weapon)
    {
        LoadWeaponUpgrades(weapon);
    }
    #endregion
}
