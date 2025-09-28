using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletInfo", menuName = "ScriptableObjects/BulletInfoSO", order = 1)]
public class BulletInfoSO : ScriptableObject
{
    [SerializeField] private int totalShotgunBullets = 6;
    [SerializeField] List<BulletInfo> bulletInfos = new List<BulletInfo>();
    public List<BulletInfo> BulletInfos { get { return bulletInfos; } }
    public int TotalShotgunBullets { get { return totalShotgunBullets; } }
    Dictionary<Weapon, BulletInfo> bulletInfoDictionary = new Dictionary<Weapon, BulletInfo>();
    public void InitializeBulletInfoDictionary()
    {
        bulletInfoDictionary.InitializeFromList(bulletInfos, b => b.WeaponType);
    }
    public BulletInfo GetBulletInfoByWeapon(Weapon weapon)
    {
        return bulletInfoDictionary.TryGetValue(weapon, out var info) ? info : null;
    }
}
