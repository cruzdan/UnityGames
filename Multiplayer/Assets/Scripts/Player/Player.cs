using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private bool isOffline = false;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private GameObject canvas;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Shoot playerShoot;
    [SerializeField] private Stun playerStun;
    [SerializeField] private Burning burning;
    private SpriteRenderer spriteRenderer;
    #endregion
    #region Life
    [Header("Life")]
    [SerializeField] private Health health;
    #endregion
    #region Invincible
    [Header("Invincible")]
    [SerializeField] private float timeInvincible;
    private bool invincible;
    private bool visible = true;
    private float timerInvincible;
    #endregion
    #region Dead
    [Header("Dead")]
    [SerializeField] private float deadWaitTime = 3f;
    private bool dead;
    private float timerDead;
    private bool restarting = false;
    #endregion
    #region Network Variables
    private ClientRpcParams clientRpcParams1;
    private readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Public Properties
    public PlayerUI PlayerUI { get { return playerUI; } }
    public Stun PlayerStun { get { return playerStun; } }
    public Burning Burning { get { return burning; } }
    public Health Health { get { return health; } }
    #endregion
    #region Functions
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShoot.CanAttackEnemies = true;
        playerShoot.CanAttackPlayers = true;
        playerShoot.OwnerPlayer = this;
        playerShoot.OwnerEnemy = null;
        if (!isOffline && !IsOwner) return;
        canvas.SetActive(true);
        PlayerCameraFollow.Instance.FollowPlayer(transform);
        health.InitializeHealth();
        timerInvincible = timeInvincible;
        timerDead = deadWaitTime;
        if (!isOffline)
            SetSpawnPositionServerRpc();
        //else
            //transform.position = Spawns.Instance.GetPlayerSpawnPoint().position;
        restarting = false;
        health.OnDie += OnDie;
        health.OnLifeChange += OnLifeChange;
        burning.Health = health;
    }

    void Update()
    {
        if ((!isOffline && !IsOwner) || restarting) return;
        if (dead)
        {
            int a = (int)timerDead;
            timerDead -= Time.deltaTime;
            if (a != (int)timerDead)
            {
                a = (int)timerDead;
                playerUI.SetDeadTimeText(a.ToString());
            }
            if (timerDead <= 0)
            {
                dead = false;
                timerDead = deadWaitTime;

                playerMovement.enabled = true;
                playerShoot.enabled = true;
                playerUI.ActiveDeadMenu(false);
            }
        }
        else if (invincible)
        {
            timerInvincible -= Time.deltaTime;
            int k = (int)(timerInvincible * 10) % 5;
            if (k < 2.5f)
            {
                if (visible)
                {
                    visible = false;
                    if (!isOffline)
                        SetColorSpriteRendererServerRpc(new Color(255, 255, 255, 0));
                    else
                        spriteRenderer.color = new Color(255, 255, 255, 0);
                }
            }
            else
            {
                if (!visible)
                {
                    visible = true;
                    if (!isOffline)
                        SetColorSpriteRendererServerRpc(new Color(255, 255, 255, 255));
                    else
                        spriteRenderer.color = new Color(255, 255, 255, 255);
                }
            }
            if (timerInvincible < 0)
            {
                ResetPlayer();
            }
            
        }
        if (transform.position.y < -25)
        {
            restarting = true;
            ResetPlayer();
            health.Die();
        }
    }

    [ClientRpc]
    public void SetColorSpriteRendererClientRpc(Color color)
    {
        spriteRenderer.color = color;
    }
    [ServerRpc]
    public void SetColorSpriteRendererServerRpc(Color color)
    {
        SetColorSpriteRendererClientRpc(color);
    }

    [ClientRpc]
    public void SetPositionClientRpc(Vector2 pos, ClientRpcParams clientRpcParams = default)
    {
        SetPosition(pos);
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    [ClientRpc]
    public void SetSpawnPositionClientRpc(Vector2 pos, ClientRpcParams clientRpcParams = default)
    {
        transform.position = pos;
        restarting = false;
    }

    [ServerRpc]
    public void SetSpawnPositionServerRpc(ServerRpcParams serverRpcParams = default)
    {
        clientId[0] = serverRpcParams.Receive.SenderClientId;
        clientRpcParams1.Send.TargetClientIds = clientId;
        SetSpawnPositionClientRpc(Spawns.Instance.GetPlayerSpawnPoint().position, clientRpcParams1);
    }
    public void DecrementLife(float damage)
    {
        if (!invincible)
        {
            health.TakeDamage(damage);
        }
    }

    void OnDie()
    {
        health.InitializeHealth();
        invincible = true;
        playerMovement.SetMaxStamina();
        playerMovement.RestartVelocity();
        playerShoot.SetCurrentWeapon(Weapon.Pistol, 100);
        playerUI.SetBulletText(playerShoot.CurrentBullets.ToString());
        playerMovement.enabled = false;
        playerShoot.enabled = false;
        dead = true;
        if (!isOffline)
            SetColorSpriteRendererServerRpc(new Color(0.1f, 0.1f, 0.1f, 0.4f));
        else
            spriteRenderer.color = new Color(0.1f, 0.1f, 0.1f, 0.4f);

        playerUI.ActiveDeadMenu(true);
        playerUI.SetDeadTimeText(deadWaitTime.ToString());
        if (!isOffline)
            SetSpawnPositionServerRpc();
        else
            transform.position = Spawns.Instance.GetPlayerSpawnPoint().position;
        restarting = false;
        burning.StopBurning();
        health.CanBurn = false;
    }

    void OnLifeChange(float life)
    {
        playerUI.SetLifeText(((int)health.CurrentLife).ToString());
        playerUI.SetLifeWidth(health.CurrentLife * 0.01f);
    }

    [ClientRpc]
    public void DecrementLifeClientRpc(float damage, ClientRpcParams clientRpcParams = default)
    {
        DecrementLife(damage);
    }
    void ResetPlayer()
    {
        invincible = false;
        timerInvincible = timeInvincible;
        health.CanBurn = true;
        visible = true;
        if (!isOffline)
            SetColorSpriteRendererServerRpc(new Color(255, 255, 255, 255));
        else
            spriteRenderer.color = new Color(255, 255, 255, 255);
    }
    [ClientRpc]
    public void AddLifeClientRpc(ClientRpcParams clientRpcParams = default)
    {
        AddLife();
    }

    public void AddLife()
    {
        health.InitializeHealth();
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartBurningServerRpc()
    {
        Burning.StartBurning();
    }
    #endregion
}