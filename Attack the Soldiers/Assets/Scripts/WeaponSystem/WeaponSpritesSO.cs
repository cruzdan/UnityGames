using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSpritesSO", menuName = "ScriptableObjects/WeaponSpritesSO", order = 1)]
public class WeaponSpritesSO : ScriptableObject
{
    public List<WeaponSpriteInfo> weaponSprites;
    public Sprite GetWeaponSprite(Weapon weapon)
    {
        foreach (WeaponSpriteInfo info in weaponSprites)
        {
            if (info.Weapon == weapon)
            {
                return info.Sprite;
            }
        }
        Debug.LogWarning($"Sprite for weapon {weapon} not found!");
        return null;
    }
}

[System.Serializable]
public class WeaponSpriteInfo
{
    public Sprite Sprite;
    public Weapon Weapon;
}
