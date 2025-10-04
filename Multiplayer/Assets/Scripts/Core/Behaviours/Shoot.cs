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
    public bool Infinite { get { return infinite; } set => infinite = value; }
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

    void ShootPistolBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Pistol);
        if (!isOffline)
            GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
                "Bullet");
        else
            GenerateBulletLocal(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
                "Offline Bullet");
    }
    void ShootShotgunBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Shotgun);
        for (int i = 0; i < bulletInfoSO.TotalShotgunBullets; i++)
        {
            if (!isOffline)
                GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
                "Bullet");
            else
                GenerateBulletLocal(bulletPosition.position, currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
                "Offline Bullet");
        }
    }

    void ShootMachineBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.MachineGun);
        if (!isOffline)
            GenerateBulletServerRpc(new(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.3f, 0.3f)), currentBulletInfo.Color, transform.right,
            currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
            "Bullet");
        else
            GenerateBulletLocal(new Vector2(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.3f, 0.3f)), currentBulletInfo.Color, transform.right,
            currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage, 
            "Offline Bullet");
    }

    void ShootSniperBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Sniper);
        if (!isOffline)
            GenerateBulletServerRpc(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
                "Bullet");
        else
            GenerateBulletLocal(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
                "Offline Bullet");
    }

    void ShootFlameThrowerBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.FlameThrower);
        if (!isOffline)
            GenerateBulletServerRpc(new(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.2f, 0.2f)), currentBulletInfo.Color,
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
            currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
            "Flame Online");
        else
            GenerateBulletLocal(new Vector2(bulletPosition.position.x, bulletPosition.position.y +
                Random.Range(-0.2f, 0.2f)), currentBulletInfo.Color, 
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized,
            currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage, 
            "Flame");
    }

    void ShootBarrelBullet()
    {
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(Weapon.Barrel);
        if (!isOffline)
        {
            GenerateBarrelBulletServerRpc(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
                "Barrel Online");
        }
        else
        {
            GenerateBarrelBulletLocal(bulletPosition.position, currentBulletInfo.Color, transform.right,
                currentBulletInfo.MaxDistance, currentBulletInfo.Speed, currentBulletInfo.Damage,
                "Barrel");

        }
    }


    [ServerRpc]
    void GenerateBulletServerRpc(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed, 
        int damage, string bulletPoolName)
    {
        bullet = NetworkObjectPool.Singleton.GetNetworkObject(bulletPoolName, position, Quaternion.identity).gameObject;
        bulletMovement = bullet.GetComponent<BulletMovement>();
        bulletMovement.SetDirection(direction);
        bulletMovement.SetSpeed(speed);
        bulletMovement.SetMaxDistance(maxDistance);
        bulletMovement.ReiniciateMovement();
        bulletMovement.OfflinePoolTag = bulletPoolName;
        bulletMovement.OnlinePoolTag = bulletPoolName;
        bulletInteractions = bullet.GetComponent<BulletInteractions>();
        if (overrideDamage)
            bulletInteractions.SetDamage(damageOverride);
        else
            bulletInteractions.SetDamage(damage);
        bulletInteractions.BulletCollided = false;
        bulletInteractions.ChangeColorClientRpc(color);
        bulletInteractions.CanAttackEnemies = canAttackEnemies;
        bulletInteractions.CanAttackPlayers = canAttackPlayers;
        bulletInteractions.OwnerPlayer = ownerPlayer;
        bulletInteractions.OwnerEnemy = ownerEnemy;
    }

    void GenerateBulletLocal(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed, 
        int damage, string bulletPoolName)
    {
        bullet = ObjectPool.Singleton.GetObject(bulletPoolName, position, Quaternion.identity);
        bulletMovement = bullet.GetComponent<BulletMovement>();
        bulletMovement.SetDirection(direction);
        bulletMovement.SetSpeed(speed);
        bulletMovement.SetMaxDistance(maxDistance);
        bulletMovement.ReiniciateMovement();
        bulletMovement.OfflinePoolTag = bulletPoolName;
        bulletMovement.OnlinePoolTag = bulletPoolName;
        bulletInteractions = bullet.GetComponent<BulletInteractions>();
        if (overrideDamage)
            bulletInteractions.SetDamage(damageOverride);
        else
            bulletInteractions.SetDamage(damage);
        bulletInteractions.BulletCollided = false;
        bulletInteractions.ChangeColor(color);
        bulletInteractions.CanAttackEnemies = canAttackEnemies;
        bulletInteractions.CanAttackPlayers = canAttackPlayers;
        bulletInteractions.OwnerPlayer = ownerPlayer;
        bulletInteractions.OwnerEnemy = ownerEnemy;
    }

    [ServerRpc]
    void GenerateBarrelBulletServerRpc(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed,
        int damage, string bulletPoolName)
    {
        bullet = NetworkObjectPool.Singleton.GetNetworkObject(bulletPoolName, position, Quaternion.identity).gameObject;
        bulletInteractions = bullet.GetComponent<BulletInteractions>();
        if (overrideDamage)
            bulletInteractions.SetDamage(damageOverride);
        else
            bulletInteractions.SetDamage(damage);
        bulletInteractions.BulletCollided = false;
        bulletInteractions.ChangeColorClientRpc(color);
        bulletInteractions.CanAttackEnemies = canAttackEnemies;
        bulletInteractions.CanAttackPlayers = canAttackPlayers;
        bulletInteractions.OwnerPlayer = ownerPlayer;
        bulletInteractions.OwnerEnemy = ownerEnemy;
        Barrel barrel = bullet.GetComponent<Barrel>();
        if (direction.x > 0)
            barrel.Initialize(speed);
        else
            barrel.Initialize(-speed);
    }

    void GenerateBarrelBulletLocal(Vector2 position, Color color, Vector2 direction, float maxDistance, float speed,
        int damage, string bulletPoolName)
    {
        bullet = ObjectPool.Singleton.GetObject(bulletPoolName, position, Quaternion.identity);
        bulletInteractions = bullet.GetComponent<BulletInteractions>();
        if (overrideDamage)
            bulletInteractions.SetDamage(damageOverride);
        else
            bulletInteractions.SetDamage(damage);
        bulletInteractions.BulletCollided = false;
        bulletInteractions.ChangeColor(color);
        bulletInteractions.CanAttackEnemies = canAttackEnemies;
        bulletInteractions.CanAttackPlayers = canAttackPlayers;
        bulletInteractions.OwnerPlayer = ownerPlayer;
        bulletInteractions.OwnerEnemy = ownerEnemy;
        Barrel barrel = bullet.GetComponent<Barrel>();
        if (direction.x > 0)
            barrel.Initialize(speed);
        else
            barrel.Initialize(-speed);
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
    }

    void ChangeCurrentWeaponByIndex(Weapon weapon)
    {
        currentWeapon = weapon;
        currentBulletInfo = bulletInfoSO.GetBulletInfoByWeapon(currentWeapon);
    }
    #endregion
}