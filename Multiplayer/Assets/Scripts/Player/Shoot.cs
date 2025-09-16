using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class Shoot : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPosition;
    //time to shoot for every Weapon
    [SerializeField] private float[] timeToShoot;
    [SerializeField] private float[] weaponSpeeds;
    [SerializeField] private float[] weaponMaxDistances;
    [SerializeField] private int[] weaponDamage;

    [SerializeField] private const int TotalShotgunBullets = 6;

    [SerializeField] private PlayerUI playerUI;
    //bullets for weapons except pistol
    [SerializeField] private int currentBullets;

    PlayerInput playerInput;

    float shootTimer = 0;

    GameObject bullet;
    BulletMovement bulletMovement;
    BulletInteractions bulletInteractions;
    public enum Weapon
    {
        Pistol = 0,
        Shotgun = 1,
        MachineGun = 2,
        Sniper = 3,
    }
    [SerializeField] private Weapon currentWeapon = Weapon.Pistol;
    private void Start()
    {
        if (!IsOwner) return;
        playerInput = GetComponent<PlayerInput>();
    }
    void Update()
    {
        if (!IsOwner) return;
        if(shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
            return;
        }
        if (playerInput.actions["Shoot"].IsPressed())
        {
            ShootCurrentWeapon();
            shootTimer = timeToShoot[(int)currentWeapon];
        }
    }

    void ShootCurrentWeapon()
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
        GenerateBulletServerRpc(bulletPosition.position, Color.black, transform.right, weaponMaxDistances[0], weaponSpeeds[0], weaponDamage[0]);
    }
    void ShootShotgunBullet()
    {
        for(int i = 0; i < TotalShotgunBullets; i++)
        {
            GenerateBulletServerRpc(bulletPosition.position, Color.red, 
                new Vector2(transform.right.x, Random.Range(-0.5f, 0.5f)).normalized, weaponMaxDistances[1], weaponSpeeds[1], weaponDamage[1]);
        }
    }

    void ShootMachineBullet()
    {
        GenerateBulletServerRpc(new(bulletPosition.position.x, bulletPosition.position.y + Random.Range(-0.3f, 0.3f)), Color.green, transform.right,
            weaponMaxDistances[2], weaponSpeeds[2], weaponDamage[2]);
    }

    void ShootSniperBullet()
    {
        GenerateBulletServerRpc(bulletPosition.position, Color.yellow, transform.right, weaponMaxDistances[3], weaponSpeeds[3], weaponDamage[3]);
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
    [ClientRpc]
    public void SetCurrentWeaponClientRpc(int weaponIndex, int totalBullets, ClientRpcParams clientRpcParams = default)
    {
        SetCurrentWeapon(weaponIndex, totalBullets);

    }
    public void SetCurrentWeapon(int weaponIndex, int totalBullets)
    {
        currentWeapon = (Weapon)weaponIndex;
        currentBullets = totalBullets;
        playerUI.SetBulletText(currentBullets.ToString());
        playerUI.SetWeponSprite(weaponIndex);

    }
    void DecrementBullets()
    {
        
        currentBullets--;
        if (currentBullets <= 0)
        {
            currentWeapon = Weapon.Pistol;
            currentBullets = 100;
            playerUI.SetWeponSprite(0);
        }
        playerUI.SetBulletText(currentBullets.ToString());
    }
}
