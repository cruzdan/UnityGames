using UnityEngine;
using System;

public static class WeaponLevelSaveSystem
{
    public static WeaponLevelSaveData LoadWeaponLevels()
    {
        WeaponLevelSaveData newData = new WeaponLevelSaveData();
        ValidateSaveData(newData);

        foreach (Weapon weapon in Enum.GetValues(typeof(Weapon)))
        {
            newData.weaponLevels[(int)weapon].currentLevel = PlayerPrefs.GetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_CURRENT_LEVEL, 0);
            newData.weaponLevels[(int)weapon].currentXP = PlayerPrefs.GetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_CURRENT_XP, 0);
            newData.weaponLevels[(int)weapon].totalDamageUpgrades = PlayerPrefs.GetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_DAMAGE_UPGRADES, 0);
            newData.weaponLevels[(int)weapon].totalTimeToShootUpgrades = PlayerPrefs.GetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_TIME_TO_SHOOT_UPGRADES, 0);
            newData.weaponLevels[(int)weapon].totalMaxDistanceUpgrades = PlayerPrefs.GetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_MAX_DISTANCE_UPGRADES, 0);
        }
        return newData;
    }

    public static void SaveWeaponLevels(WeaponLevelSaveData saveData)
    {
        foreach (Weapon weapon in Enum.GetValues(typeof(Weapon)))
        {
            PlayerPrefs.SetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_CURRENT_LEVEL, saveData.GetWeaponData(weapon).currentLevel);
            PlayerPrefs.SetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_CURRENT_XP, saveData.GetWeaponData(weapon).currentXP);
            PlayerPrefs.SetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_DAMAGE_UPGRADES, saveData.GetWeaponData(weapon).totalDamageUpgrades);
            PlayerPrefs.SetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_TIME_TO_SHOOT_UPGRADES, saveData.GetWeaponData(weapon).totalTimeToShootUpgrades);
            PlayerPrefs.SetInt(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_MAX_DISTANCE_UPGRADES, saveData.GetWeaponData(weapon).totalMaxDistanceUpgrades);
        }
        PlayerPrefs.Save();
    }

    public static void ClearSaveData()
    {
        foreach (Weapon weapon in Enum.GetValues(typeof(Weapon)))
        {
            PlayerPrefs.DeleteKey(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_CURRENT_LEVEL);
            PlayerPrefs.DeleteKey(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_CURRENT_XP);
            PlayerPrefs.DeleteKey(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_DAMAGE_UPGRADES);
            PlayerPrefs.DeleteKey(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_TIME_TO_SHOOT_UPGRADES);
            PlayerPrefs.DeleteKey(Constants.SAVE_INTRO + (int)weapon + Constants.SAVE_TOTAL_MAX_DISTANCE_UPGRADES);
        }
    }

    private static void ValidateSaveData(WeaponLevelSaveData saveData)
    {
        foreach (Weapon weapon in Enum.GetValues(typeof(Weapon)))
        {
            if (saveData.GetWeaponData(weapon) == null)
            {
                saveData.weaponLevels.Add(new WeaponLevelData(weapon));
            }
        }
    }
}
