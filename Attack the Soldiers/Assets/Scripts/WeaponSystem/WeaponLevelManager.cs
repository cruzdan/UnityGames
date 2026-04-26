using UnityEngine;
using System;
using System.Collections.Generic;

public class WeaponLevelManager : MonoBehaviour
{
    #region Serialized Variables
    [Header("Configuration")]
    [SerializeField] private WeaponLevelConfig levelConfig;
    [SerializeField] private BulletInfoSO bulletInfoSO;
    #endregion

    #region Private Variables
    private Dictionary<Weapon, WeaponLevelData> weaponLevels = new Dictionary<Weapon, WeaponLevelData>();
    private WeaponLevelSaveData saveData;
    #endregion

    #region Actions
    public static Action<Weapon, int> OnWeaponLevelUp;
    public static Action<Weapon, int, int> OnXPChanged;
    public static Action<Weapon> OnLevelUpAvailable;
    #endregion

    #region Public Properties
    public WeaponLevelConfig LevelConfig => levelConfig;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        if (levelConfig == null)
        {
            Debug.LogError("WeaponLevelConfig not assigned!");
            return;
        }

        if (bulletInfoSO == null)
        {
            bulletInfoSO = FindAnyObjectByType<NetworkGameManager>().BulletInfoSO;
        }

        LoadWeaponLevels();
    }

    private void OnApplicationQuit()
    {
        SaveWeaponLevels();
    }

    private void OnDestroy()
    {
        SaveWeaponLevels();
    }
    #endregion

    #region Load/Save System
    private void LoadWeaponLevels()
    {
        saveData = WeaponLevelSaveSystem.LoadWeaponLevels();
        weaponLevels.Clear();
        foreach (Weapon weapon in Enum.GetValues(typeof(Weapon)))
        {
            WeaponLevelData data = saveData.GetWeaponData(weapon);
            weaponLevels[weapon] = data;
        }
    }

    private void SaveWeaponLevels()
    {
        if (saveData == null) return;

        foreach (var kvp in weaponLevels)
        {
            saveData.UpdateWeaponData(kvp.Key, kvp.Value);
        }
        WeaponLevelSaveSystem.SaveWeaponLevels(saveData);
    }
    #endregion

    #region XP Management
    public void AddXP(Weapon weapon, int amount)
    {
        if (!weaponLevels.ContainsKey(weapon))
        {
            weaponLevels[weapon] = new WeaponLevelData(weapon);
        }
        WeaponLevelData data = weaponLevels[weapon];

        data.currentXP += amount;
        OnXPChanged?.Invoke(weapon, data.currentXP, data.currentLevel);
        CheckLevelUp(weapon, data);
    }

    public void AddKillXP(Weapon weapon)
    {
        AddXP(weapon, levelConfig.XPPerKill);
    }

    private void CheckLevelUp(Weapon weapon, WeaponLevelData data)
    {
        int requiredXP = levelConfig.GetXPRequiredForLevel(data.currentLevel);
        while (data.currentXP >= requiredXP)
        {
            data.currentXP -= requiredXP;
            data.currentLevel++;

            OnWeaponLevelUp?.Invoke(weapon, data.currentLevel);
            OnLevelUpAvailable?.Invoke(weapon);
        }
    }
    #endregion

    #region Upgrade System
    public bool CanUpgrade(Weapon weapon)
    {
        if (!weaponLevels.ContainsKey(weapon)) return false;

        WeaponLevelData data = weaponLevels[weapon];
        return data.currentLevel > 0 && 
               (data.totalDamageUpgrades < levelConfig.MaxLevel || 
                data.totalTimeToShootUpgrades < levelConfig.MaxLevel || 
                data.totalMaxDistanceUpgrades < levelConfig.MaxLevel);
    }

    public void ApplyUpgrade(Weapon weapon, WeaponUpgradeType upgradeType)
    {
        if (!weaponLevels.ContainsKey(weapon))
        {
            weaponLevels[weapon] = new WeaponLevelData(weapon);
        }
        WeaponLevelData data = weaponLevels[weapon];

        switch (upgradeType)
        {
            case WeaponUpgradeType.Damage:
                if (data.totalDamageUpgrades >= levelConfig.MaxLevel)
                {
                    return;
                }
                data.totalDamageUpgrades++;
                break;

            case WeaponUpgradeType.TimeToShoot:
                if (data.totalTimeToShootUpgrades >= levelConfig.MaxLevel)
                {
                    return;
                }
                data.totalTimeToShootUpgrades++;
                break;

            case WeaponUpgradeType.MaxDistance:
                if (data.totalMaxDistanceUpgrades >= levelConfig.MaxLevel)
                {
                    return;
                }
                data.totalMaxDistanceUpgrades++;
                break;
        }

        saveData.UpdateWeaponData(weapon, data);
        SaveWeaponLevels();
    }

    public WeaponLevelData GetWeaponData(Weapon weapon)
    {
        if (weaponLevels.ContainsKey(weapon))
        {
            return weaponLevels[weapon];
        }
        return new WeaponLevelData(weapon);
    }

    public int GetXPRequiredForNextLevel(Weapon weapon)
    {
        if (!weaponLevels.ContainsKey(weapon)) return 0;

        WeaponLevelData data = weaponLevels[weapon];
        return levelConfig.GetXPRequiredForLevel(data.currentLevel);
    }

    public int GetCurrentLevel(Weapon weapon)
    {
        if (!weaponLevels.ContainsKey(weapon)) return 0;
        return weaponLevels[weapon].currentLevel;
    }

    public int GetCurrentXP(Weapon weapon)
    {
        if (!weaponLevels.ContainsKey(weapon)) return 0;
        return weaponLevels[weapon].currentXP;
    }
    #endregion

    #region Public Methods
    public void ForceSave()
    {
        SaveWeaponLevels();
    }

    public void ResetAllProgress()
    {
        WeaponLevelSaveSystem.ClearSaveData();
        LoadWeaponLevels();
    }
    #endregion
}

public enum WeaponUpgradeType
{
    Damage = 0,
    TimeToShoot = 1,
    MaxDistance = 2
}
