using System;
using System.Collections.Generic;

[Serializable]
public class WeaponLevelData
{
    public Weapon weaponType;
    public int currentLevel;
    public int currentXP;
    public int totalDamageUpgrades;
    public int totalTimeToShootUpgrades;
    public int totalMaxDistanceUpgrades;

    public WeaponLevelData(Weapon weapon)
    {
        weaponType = weapon;
        currentLevel = 0;
        currentXP = 0;
        totalDamageUpgrades = 0;
        totalTimeToShootUpgrades = 0;
        totalMaxDistanceUpgrades = 0;
    }
}

[Serializable]
public class WeaponLevelSaveData
{
    public List<WeaponLevelData> weaponLevels = new List<WeaponLevelData>();

    public WeaponLevelData GetWeaponData(Weapon weapon)
    {
        WeaponLevelData data = weaponLevels.Find(w => w.weaponType == weapon);
        if (data == null)
        {
            data = new WeaponLevelData(weapon);
            weaponLevels.Add(data);
        }
        return data;
    }

    public void UpdateWeaponData(Weapon weapon, WeaponLevelData data)
    {
        int index = weaponLevels.FindIndex(w => w.weaponType == weapon);
        if (index >= 0)
        {
            weaponLevels[index] = data;
        }
        else
        {
            weaponLevels.Add(data);
        }
    }
}
