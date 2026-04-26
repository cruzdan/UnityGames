using UnityEngine;
using Unity.Netcode;
public class Shoot : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private BulletInfoSO bulletInfoSO;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private int currentBullets;
    [SerializeField] private Weapon currentWeapon = Weapon.Pistol;
    private BulletInfo currentBulletInfo;
    #endregion
    #region Attack Variables
    [Header("Attack")]
    [SerializeField] protected bool canAttackPlayers;
    [SerializeField] protected Player ownerPlayer;
    [SerializeField] protected bool canAttackEnemies;
    [SerializeField] protected Enemy ownerEnemy;
    public bool CanAttackPlayers { get { return canAttackPlayers; } set => canAttackPlayers = value; }
    public bool CanAttackEnemies { get { return canAttackEnemies; } set => canAttackEnemies = value; }
    public Player OwnerPlayer { get { return ownerPlayer; } set { ownerPlayer = value; } }
    public Enemy OwnerEnemy { get { return ownerEnemy; } set { ownerEnemy = value; } }
    #endregion
    #region Override Variables
    [Header("Override")]
    [SerializeField] private bool overrideDamage = false;
    [SerializeField] private int damageOverride;
    [SerializeField] private bool infinite;
    #endregion
    #region Actions
    public System.Action<int> OnBulletNumberChanged;
    #endregion
    #region Auxiliar Variables
    private NetworkObject bullet;
    private BulletMovement bulletMovement;
    private BulletInteractions bulletInteractions;
    private WeaponUpgradeHandler upgradeHandler;
    #endregion
    #region Public Properties
    public BulletInfo CurrentBulletInfo { get { if (currentBulletInfo == null) return currentBulletInfo = FindAnyObjectByType<NetworkGameManager>().BulletInfoSO.GetBulletInfoByWeapon(currentWeapon); return currentBulletInfo; } }
    public int CurrentBullets { get { return currentBullets; } }
    public bool OverrideDamage { get { return overrideDamage; } set => overrideDamage = value; }
    public int DamageOverride { get { return damageOverride; } set => damageOverride = value; }
    public bool Infinite { get { return infinite; } set => infinite = value; }
    public Weapon CurrentWeapon { get { return currentWeapon; } }
    public float UpgradedDamage { get; private set; }
    public float UpgradedTimeToShoot { get; private set; }
    public float UpgradedMaxDistance { get; private set; }
    #endregion
    #region Functions
    private void Start()
    {
        bulletInfoSO = FindAnyObjectByType<NetworkGameManager>().BulletInfoSO;
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) return;
        ChangeCurrentWeaponByIndex(currentWeapon);
        InitializeUpgradeHandler();
    }

    private void InitializeUpgradeHandler()
    {
        upgradeHandler = FindAnyObjectByType<WeaponUpgradeHandler>();
        if (upgradeHandler == null)
        {
            Player player = GetComponentInParent<Player>();
            if (player != null)
            {
                upgradeHandler = player.GetComponent<WeaponUpgradeHandler>();
            }
        }

        if (upgradeHandler != null)
        {
            upgradeHandler.LoadWeaponUpgrades(currentWeapon);
            ApplyWeaponUpgrades();
        }
    }

    public void ShootCurrentWeapon()
    {
        if (bulletInfoSO == null)
            bulletInfoSO = FindAnyObjectByType<NetworkGameManager>().BulletInfoSO;
        ApplyWeaponUpgrades();

        switch (currentWeapon)
        {
            case Weapon.Pistol:
                ShootPistolBullet();
                break;
            case Weapon.Shotgun:
                ShootShotgunBullet();
                DecrementBulletsIfPossible();
                break;
            case Weapon.MachineGun:
                ShootMachineBullet();
                DecrementBulletsIfPossible();
                break;
            case Weapon.Sniper:
                ShootSniperBullet();
                DecrementBulletsIfPossible();
                break;
            case Weapon.FlameThrower:
                ShootFlameThrowerBullet();
                DecrementBulletsIfPossible();
                break;
            case Weapon.Barrel:
                ShootBarrelBullet();
                DecrementBulletsIfPossible();
                break;
        }
    }

    private void ApplyWeaponUpgrades()
    {

        BulletInfo baseInfo = CurrentBulletInfo;
        if (baseInfo == null) return;

        UpgradedDamage = baseInfo.Damage;
        UpgradedTimeToShoot = baseInfo.TimeToShoot;
        UpgradedMaxDistance = baseInfo.MaxDistance;

        if (overrideDamage) return;

        if (upgradeHandler != null)
        {
            UpgradedDamage = upgradeHandler.GetModifiedDamage(currentWeapon, baseInfo.Damage);
            UpgradedTimeToShoot = upgradeHandler.GetModifiedTimeToShoot(currentWeapon, baseInfo.TimeToShoot);
            UpgradedMaxDistance = upgradeHandler.GetModifiedMaxDistance(currentWeapon, baseInfo.MaxDistance);
        }
    }

    void ShootPistolBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Pistol);

        if (GameNetwork.Instance.IsOnline)
            GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color, transform.right,
                UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
                Constants.NETWORK_OBJECT_POOL_BULLET);
        else
            GenerateBullet(bulletPosition.position, currentBulletInfo.Color, transform.right,
                UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
                Constants.NETWORK_OBJECT_POOL_BULLET);
    }

    void ShootShotgunBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Shotgun);
        for (int i = 0; i < bulletInfoSO.TotalShotgunBullets; i++)
        {
            if (GameNetwork.Instance.IsOnline)
                GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
                UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
                Constants.NETWORK_OBJECT_POOL_BULLET);
            else
                GenerateBullet(bulletPosition.position, currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
                UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
                Constants.NETWORK_OBJECT_POOL_BULLET);
        }
    }

    void ShootMachineBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.MachineGun);
        if (GameNetwork.Instance.IsOnline)
            GenerateBulletServerRpc(new(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.3f, 0.3f)), currentBulletInfo.Color, transform.right,
            UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
            Constants.NETWORK_OBJECT_POOL_BULLET);
        else
            GenerateBullet(new Vector2(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.3f, 0.3f)), currentBulletInfo.Color, transform.right,
            UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
            Constants.NETWORK_OBJECT_POOL_BULLET);
    }

    void ShootSniperBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Sniper);
        if (GameNetwork.Instance.IsOnline)
            GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color, transform.right,
                UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
                Constants.NETWORK_OBJECT_POOL_BULLET);
        else
            GenerateBullet(bulletPosition.position, currentBulletInfo.Color, transform.right,
                UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
                Constants.NETWORK_OBJECT_POOL_BULLET);
    }

    void ShootFlameThrowerBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.FlameThrower);
        if (GameNetwork.Instance.IsOnline)
            GenerateBulletServerRpc(new(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.2f, 0.2f)), currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
            UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
            Constants.NETWORK_OBJECT_POOL_FLAME);
        else
            GenerateBullet(new Vector2(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.2f, 0.2f)), currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
            UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
            Constants.NETWORK_OBJECT_POOL_FLAME);
    }

    void ShootBarrelBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Barrel);
        if (GameNetwork.Instance.IsOnline)
        {
            GenerateBarrelServerRpc(bulletPosition.position, currentBulletInfo.Color, transform.right,
                UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
                Constants.NETWORK_OBJECT_POOL_BARREL);
        }
        else
        {
            GenerateBarrel(bulletPosition.position, currentBulletInfo.Color, transform.right,
                UpgradedMaxDistance, currentBulletInfo.Speed, (int)UpgradedDamage,
                Constants.NETWORK_OBJECT_POOL_BARREL);
        }
    }

    [Rpc(SendTo.Server)]
    void GenerateBulletServerRpc(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed,
        int damage, string bulletPoolName)
    {
        GenerateBullet(position, color, direction, maxDistance, speed, damage, bulletPoolName);
    }

    void GenerateBullet(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed, 
        int damage, string bulletPoolName)
    {

        bullet = GameNetwork.Instance.Spawn(bulletPoolName, position, Quaternion.identity);
        bulletMovement = bullet.GetComponent<BulletMovement>();
        bulletMovement.SetDirection(direction);
        bulletMovement.SetSpeed(speed);
        bulletMovement.SetMaxDistance(maxDistance);
        bulletMovement.ReiniciateMovement();
        bulletMovement.PoolTag = bulletPoolName;
        bulletInteractions = bullet.GetComponent<BulletInteractions>();
        if (overrideDamage)
            bulletInteractions.SetDamage(damageOverride);
        else
            bulletInteractions.SetDamage(damage);
        bulletInteractions.BulletCollided = false;

        if (GameNetwork.Instance.IsOnline)
        {
            bulletInteractions.ChangeColorClientRpc(color);
            bulletInteractions.networkColor.Value = color;
        }
        else
        {
            bulletInteractions.ChangeColor(color);
        }

        bulletInteractions.CanAttackEnemies = canAttackEnemies;
        bulletInteractions.CanAttackPlayers = canAttackPlayers;
        bulletInteractions.OwnerPlayer = ownerPlayer;
        bulletInteractions.OwnerEnemy = ownerEnemy;
        bulletInteractions.PoolTag = bulletPoolName;
    }

    [ServerRpc]
    void GenerateBarrelServerRpc(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed,
        int damage, string bulletPoolName)
    {
        GenerateBarrel(position, color, direction, maxDistance, speed, damage, bulletPoolName);
    }

    void GenerateBarrel(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed,
        int damage, string bulletPoolName)
    {
        bullet = GameNetwork.Instance.Spawn(bulletPoolName, position, Quaternion.identity);
        bulletInteractions = bullet.GetComponent<BulletInteractions>();
        if (overrideDamage)
            bulletInteractions.SetDamage(damageOverride);
        else
            bulletInteractions.SetDamage(damage);
        bulletInteractions.BulletCollided = false;
        bulletInteractions.CanAttackEnemies = canAttackEnemies;
        bulletInteractions.CanAttackPlayers = canAttackPlayers;
        bulletInteractions.OwnerPlayer = ownerPlayer;
        bulletInteractions.OwnerEnemy = ownerEnemy;
        bulletInteractions.PoolTag = bulletPoolName;
        Barrel barrel = bullet.GetComponent<Barrel>();
        speed = direction.x > 0 ? speed : -speed;
        barrel.Initialize(speed);
        ChangeBulletColor(bulletInteractions, color);
    }

    void ChangeBulletColor(BulletInteractions bulletInteractions, Color color)
    {
        if (GameNetwork.Instance.IsOnline)
        {
            bulletInteractions.ChangeColorClientRpc(color);
            bulletInteractions.networkColor.Value = color;
        }
        else
            bulletInteractions.ChangeColor(color);
    }

    [ClientRpc]
    public void SetCurrentWeaponClientRpc(Weapon weapon, int totalBullets, ClientRpcParams clientRpcParams = default)
    {
        SetCurrentWeapon(weapon, totalBullets);
    }
    public void SetCurrentWeapon(Weapon weapon, int totalBullets)
    {
        ChangeCurrentWeaponByIndex(weapon);
        currentBullets = totalBullets;
        OnBulletNumberChanged?.Invoke(currentBullets);
    }

    [ClientRpc]
    public void SetCurrentWeaponWithSOClientRpc(Weapon weapon, ClientRpcParams clientRpcParams = default)
    {
        SetCurrentWeaponWithSO(weapon);
    }

    void SetCurrentWeaponWithSO(Weapon weapon)
    {
        ChangeCurrentWeaponByIndex(weapon);
        currentBullets = CurrentBulletInfo.TotalBullets;
        OnBulletNumberChanged?.Invoke(currentBullets);
    }

    void DecrementBulletsIfPossible()
    {
        if (!infinite)
            DecrementBullets();
    }
    void DecrementBullets()
    {
        currentBullets--;
        if (currentBullets <= 0)
        {
            ChangeCurrentWeaponByIndex(Weapon.Pistol);
            currentBullets = 100;
        }
        OnBulletNumberChanged?.Invoke(currentBullets);
    }

    void ChangeCurrentWeaponByIndex(Weapon weapon)
    {
        currentWeapon = weapon;
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(currentWeapon);
    }

    [ClientRpc]
    public void SetInfiniteAmmoClientRpc(bool value, ClientRpcParams clientRpcParams = default)
    {
        SetInfiniteAmmo(value);
    }

    void SetInfiniteAmmo(bool value)
    {
        infinite = value;
    }
    #endregion
}