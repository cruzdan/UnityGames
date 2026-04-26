using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponLevelConfig", menuName = "ScriptableObjects/WeaponLevelConfig", order = 2)]
public class WeaponLevelConfig : ScriptableObject
{
    #region Serialized Variables
    [Header("General Settings")]
    [SerializeField] private int maxLevel = 10;
    [SerializeField] private int baseXPRequired = 100;
    [SerializeField] private int xpIncreasePerLevel = 50;
    
    [Header("XP Rewards")]
    [SerializeField] private int xpPerKill = 25;
    
    [Header("Weapon Specific Config")]
    [SerializeField] private List<WeaponUpgradeValuesConfig> weaponUpgradeValues = new List<WeaponUpgradeValuesConfig>();
    [SerializeField] private List<WeaponSpecificConfig> weaponConfigs = new List<WeaponSpecificConfig>();
    #endregion

    #region Public Properties
    public int MaxLevel => maxLevel;
    public int BaseXPRequired => baseXPRequired;
    public int XPIncreasePerLevel => xpIncreasePerLevel;
    public int XPPerKill => xpPerKill;
    public List<WeaponUpgradeValuesConfig> WeaponUpgradeValues => weaponUpgradeValues;
    #endregion

    #region Functions
    public int GetXPRequiredForLevel(int currentLevel)
    {
        return baseXPRequired + (currentLevel * xpIncreasePerLevel);
    }

    public WeaponSpecificConfig GetConfigForWeapon(Weapon weapon)
    {
        return weaponConfigs.Find(c => c.WeaponType == weapon);
    }
    #endregion
}

[System.Serializable]
public class WeaponSpecificConfig
{
    [SerializeField] private Weapon weaponType;
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float timeToShootMultiplier = 1f;
    [SerializeField] private float distanceMultiplier = 1f;

    public Weapon WeaponType => weaponType;
    public float DamageMultiplier => damageMultiplier;
    public float TimeToShootMultiplier => timeToShootMultiplier;
    public float DistanceMultiplier => distanceMultiplier;
}

[System.Serializable]
public class WeaponUpgradeValuesConfig
{
    public Weapon Weapon;
    public float DamageIncrease;
    public float TimeToShootDecrease;
    public float MaxDistanceIncrease;
}