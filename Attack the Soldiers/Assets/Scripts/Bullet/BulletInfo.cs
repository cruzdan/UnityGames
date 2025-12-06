using UnityEngine;
[System.Serializable]
public class BulletInfo
{
    #region Serialized Variables
    [SerializeField] private Weapon weaponType;
    [SerializeField] private GameObject onlineBulletPrefab;
    [SerializeField] private GameObject offlineBulletPrefab;
    [SerializeField] private float timeToShoot;
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    [SerializeField] private int damage;
    [SerializeField] private Color color;
    [SerializeField] private int totalBullets;
    #endregion 
    #region Public Properties
    public Weapon WeaponType { get { return weaponType; } }
    public GameObject OnlineBulletPrefab { get { return onlineBulletPrefab; } }
    public GameObject OfflineBulletPrefab { get { return offlineBulletPrefab; } }
    public float TimeToShoot { get { return timeToShoot; } }
    public float Speed { get { return speed; } }
    public float MaxDistance { get { return maxDistance; } }
    public int Damage { get { return damage; } }
    public Color Color { get { return color; } }
    public int TotalBullets { get { return totalBullets; } }
    #endregion
}
public enum Weapon
{
    Pistol = 0,
    Shotgun = 1,
    MachineGun = 2,
    Sniper = 3,
    Barrel = 4,
    FlameThrower = 5
}