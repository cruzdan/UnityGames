using System.Collections.Generic;
using UnityEngine;
//Scriptable Object that contains all the information about the bullets
[CreateAssetMenu(fileName = "BulletInfo", menuName = "ScriptableObjects/BulletInfoSO", order = 1)]
public class BulletInfoSO : ScriptableObject
{
    #region Serialized Variables
    [SerializeField] private int totalShotgunBullets = 6;
    [SerializeField] List<BulletInfo> bulletInfos = new List<BulletInfo>();
    #endregion
    #region Public Properties
    public List<BulletInfo> BulletInfos { get { return bulletInfos; } }
    public int TotalShotgunBullets { get { return totalShotgunBullets; } }
    #endregion
    #region Private Variables
    private Dictionary<Weapon, BulletInfo> bulletInfoDictionary = new Dictionary<Weapon, BulletInfo>();
    #endregion
    #region Functions
    public void InitializeBulletInfoDictionary()
    {
        bulletInfoDictionary.InitializeFromList(bulletInfos, b => b.WeaponType);
    }
    public BulletInfo GetBulletInfoByWeapon(Weapon weapon)
    {
        return bulletInfoDictionary.TryGetValue(weapon, out var info) ? info : null;
    }
    #endregion
}
