using UnityEngine;
using Unity.Netcode;

public class Shoot : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private bool isOffline = false;
    [SerializeField] private BulletInfoSO bulletInfoSO;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private int currentBullets;
    [SerializeField] private Weapon currentWeapon = Weapon.Pistol;
    private BulletInfo currentBulletInfo;
    #endregion
    #region Override Variables
    [Header("Override")]
    [SerializeField] private bool overrideDamage = false;
    [SerializeField] private int damageOverride;
    #endregion
    #region Auxiliar Variables
    private GameObject bullet;
    private BulletMovement bulletMovement;
    private BulletInteractions bulletInteractions;
    #endregion
    #region Public Properties
    public BulletInfo CurrentBulletInfo { get { return currentBulletInfo; } }
    public int CurrentBullets { get { return currentBullets; } }
    public bool OverrideDamage { get { return overrideDamage; } set => overrideDamage = value; }
    public int DamageOverride { get { return damageOverride; } set => damageOverride = value; }
    #endregion
    #region Functions
    private void Start()
    {
        if (!isOffline && !IsOwner) return;
        bulletInfoSO = FindObjectOfType<GameManager>().BulletInfoSO;
    }

    public void ShootCurrentWeapon()
    {
        switch (currentWeapon)
        {
            case Weapon.Pistol:
                ShootPistolBullet();
                break;
            case Weapon.Shotgun:
                ShootShotgunBullet();
                DecrementBullets();
                break;
            case Weapon.MachineGun:
                ShootMachineBullet();
                DecrementBullets();
                break;
            case Weapon.Sniper:
                ShootSniperBullet();
                DecrementBullets();
                break;
        }
    }

    void ShootPistolBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Pistol);
        if (!isOffline)
            GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage);
        else
            GenerateBulletLocal(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage);
    }
    void ShootShotgunBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Shotgun);
        for (int i = 0; i < bulletInfoSO.TotalShotgunBullets; i++)
        {
            if (!isOffline)
                GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage);
            else
                GenerateBulletLocal(bulletPosition.position, currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage);
        }
    }

    void ShootMachineBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.MachineGun);
        if (!isOffline)
            GenerateBulletServerRpc(new(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.3f, 0.3f)), currentBulletInfo.Color, transform.right,
            currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage);
        else
            GenerateBulletLocal(new Vector2(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.3f, 0.3f)), currentBulletInfo.Color, transform.right,
            currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage);
    }

    void ShootSniperBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Sniper);
        if (!isOffline)
            GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage);
        else
            GenerateBulletLocal(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage);
    }
    [ServerRpc]
    void GenerateBulletServerRpc(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed, int damage)
    {
        bullet = NetworkObjectPool.Singleton.GetNetworkObject("Bullet", position, Quaternion.identity).gameObject;
        bulletMovement = bullet.GetComponent<BulletMovement>();
        bulletMovement.SetDirection(direction);
        bulletMovement.SetSpeed(speed);
        bulletMovement.SetMaxDistance(maxDistance);
        bulletMovement.ReiniciateMovement();
        bulletInteractions = bullet.GetComponent<BulletInteractions>();
        bulletInteractions.SetDamage(damage);
        bulletInteractions.SetDead(false);
        bulletInteractions.ChangeColorClientRpc(color);
    }
    void GenerateBulletLocal(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed, int damage)
    {
        bullet = ObjectPool.Singleton.GetObject("Offline Bullet", position, Quaternion.identity);
        bulletMovement = bullet.GetComponent<BulletMovement>();
        bulletMovement.SetDirection(direction);
        bulletMovement.SetSpeed(speed);
        bulletMovement.SetMaxDistance(maxDistance);
        bulletMovement.ReiniciateMovement();
        bulletInteractions = bullet.GetComponent<BulletInteractions>();
        if (overrideDamage)
            bulletInteractions.SetDamage(damageOverride);
        else
            bulletInteractions.SetDamage(damage);
        bulletInteractions.SetDead(false);
        bulletInteractions.ChangeColor(color);
    }
    [ClientRpc]
    public void SetCurrentWeaponClientRpc(int weaponIndex, int totalBullets, ClientRpcParams clientRpcParams = default)
    {
        SetCurrentWeapon(weaponIndex, totalBullets);
    }
    public void SetCurrentWeapon(int weaponIndex, int totalBullets)
    {
        ChangeCurrentWeaponByIndex(weaponIndex);
        currentBullets = totalBullets;

    }
    void DecrementBullets()
    {
        currentBullets--;
        if (currentBullets <= 0)
        {
            ChangeCurrentWeaponByIndex(0);
            currentBullets = 100;
        }
    }

    void ChangeCurrentWeaponByIndex(int index)
    {
        currentWeapon = (Weapon)index;
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(currentWeapon);
    }
    #endregion
}