using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Shoot shoot;
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private Stun playerStun;
    [SerializeField] private BurnStatus burnStatus;
    [SerializeField] private Stamina playerStamina;
    [SerializeField] private DeviceChecker deviceChecker;
    private SpriteRenderer spriteRenderer;
    public bool canActiveDeadMenu = true;
    #endregion
    #region UI
    [Header("UI")]
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject mobileInputCanvas;
    [SerializeField] private GameObject pauseButton;
    #endregion
    #region Life
    [Header("Life")]
    [SerializeField] private float initialLife = 100f;
    public NetworkVariable<float> CurrentLife = new NetworkVariable<float>();
    #endregion
    #region Invincible
    [Header("Invincible")]
    [SerializeField] private float timeInvincible;
    public NetworkVariable<bool> Invincible = new NetworkVariable<bool>(false);
    private bool visible = true;
    private float timerInvincible;
    #endregion
    #region Dead
    [Header("Dead")]
    [SerializeField] private float deadWaitTime = 3f;
    public NetworkVariable<bool> dead = new NetworkVariable<bool>(false);
    private float timerDead;
    private bool restarting = false;
    #endregion
    #region Weapons
    [Header("Weapons")]
    [SerializeField] private Weapon startingWeapon;
    #endregion
    #region Network Variables
    private ClientRpcParams clientRpcParams1;
    private readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Public Properties
    public PlayerUI PlayerUI { get { return playerUI; } }
    public Stun PlayerStun { get { return playerStun; } }
    public Shoot Shoot { get { return shoot; } }
    public PlayerMovement PlayerMovement { get { return playerMovement; } }
    public bool Dead { get { return dead.Value; } set { dead.Value = value; } }
    #endregion
    #region Functions
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shoot.CanAttackEnemies = true;
        shoot.CanAttackPlayers = GameNetwork.MultiplayerModeType == MultiplayerModeType.PVP;
        shoot.OwnerPlayer = this;
        shoot.OwnerEnemy = null;
        if (GameNetwork.Instance.IsOnline)
        {
            if (IsServer)
            {
                PlayerManager.Instance.AddPlayer(this);
                CurrentLife.Value = initialLife;
            }
        }
        else
        {
            CurrentLife.Value = initialLife;
        }
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) { enabled = false; return; }
        canvas.SetActive(true);
        FindAnyObjectByType<MainMenuUI>().UICanvas.SetActive(false);
        SetPlayerOnSpawnPoint();
        if (!GameNetwork.Instance.IsOnline || GameNetwork.MultiplayerModeType != MultiplayerModeType.PVP)
        {
            FindAnyObjectByType<UIScoreDisplay>().transform.parent.gameObject.SetActive(false);
        }
        if (deviceChecker.IsOnMobile())
        {
            pauseButton.SetActive(false);
            mobileInputCanvas.SetActive(true);
        }

        CurrentLife.OnValueChanged += OnLifeChange;
        timerInvincible = timeInvincible;
        timerDead = deadWaitTime;
        restarting = false;
    }


    void Update()
    {
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject) || restarting) return;
        if (dead.Value)
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
                if (GameNetwork.Instance.IsOnline)
                    SetDeadValueServerRpc(false);
                else
                    SetDeadValue(false);
                timerDead = deadWaitTime;
                playerShoot.CanShoot = true;
                playerMovement.enabled = true;
                shoot.enabled = true;
                playerUI.ActiveDeadMenu(false);
            }
        }
        else if (Invincible.Value)
        {
            timerInvincible -= Time.deltaTime;
            int k = (int)(timerInvincible * 10) % 5;
            if (k < 2.5f)
            {
                if (visible)
                {
                    visible = false;
                    if (GameNetwork.Instance.IsOnline)
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
                    if (GameNetwork.Instance.IsOnline)
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
            OnDie();

            if (GameNetwork.Instance.IsOnline)
                SetColorSpriteRendererServerRpc(new Color(0.1f, 0.1f, 0.1f, 0.4f));
            else
                spriteRenderer.color = new Color(0.1f, 0.1f, 0.1f, 0.4f);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SetColorSpriteRendererClientRpc(Color color)
    {
        spriteRenderer.color = color;
    }

    [ServerRpc(RequireOwnership = false)]
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
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetSpawnPositionServerRpc(ServerRpcParams serverRpcParams = default)
    {
        clientId[0] = serverRpcParams.Receive.SenderClientId;
        clientRpcParams1.Send.TargetClientIds = clientId;
        SetSpawnPositionClientRpc(Spawns.Instance.GetSpawnByType(SpawnType.Player).position, clientRpcParams1);
    }

    public void DecrementLife(float damage)
    {
        if (!Invincible.Value)
        {
            TakeDamage(damage);
        }
    }

    void TakeDamage(float damage)
    {
        CurrentLife.Value -= damage;
    }

    void OnDie()
    {
        if (GameNetwork.Instance.IsOnline)
            InitializeLifeServerRpc();
        else
            InitializeLife();
        if (GameNetwork.Instance.IsOnline)
            SetInvincibleServerRpc(true);
        else
            SetInvincible(true);
        playerStun.StopStunPlayer();
        playerStamina.SetMaxStamina();
        playerMovement.RestartVelocity();
        shoot.SetCurrentWeapon(startingWeapon, 100);
        playerMovement.enabled = false;
        shoot.enabled = false;
        if (GameNetwork.Instance.IsOnline)
            SetDeadValueServerRpc(true);
        else
            SetDeadValue(true);
        burnStatus.SetCanBurn(false);
        if (canActiveDeadMenu)
            playerUI.ActiveDeadMenu(true);
        playerUI.SetDeadTimeText(deadWaitTime.ToString());

        SetPlayerOnSpawnPoint();
        restarting = false;
        if (GameNetwork.Instance.IsOnline)
        {
            SetColorSpriteRendererServerRpc(new Color(0.1f, 0.1f, 0.1f, 0.4f));
        }
        else
            spriteRenderer.color = new Color(0.1f, 0.1f, 0.1f, 0.4f);
        playerShoot.CanShoot = false;
    }

    void SetPlayerOnSpawnPoint()
    {
        if (GameNetwork.Instance.IsOnline)
            SetSpawnPositionServerRpc();
        else
            transform.position = Spawns.Instance.GetSpawnByType(SpawnType.Player).position;
        restarting = false;
    }

    void OnLifeChange(float previousValue, float currentValue)
    {
        playerUI.SetLifeText(((int)currentValue).ToString());
        playerUI.SetLifeWidth(currentValue * 0.01f);
        if (currentValue <= 0)
        {
            OnDie();
        }
    }

    [ClientRpc]
    public void DecrementLifeClientRpc(float damage, ClientRpcParams clientRpcParams = default)
    {
        DecrementLife(damage);
    }

    void ResetPlayer()
    {
        if (GameNetwork.Instance.IsOnline)
            SetInvincibleServerRpc(false);
        else
            SetInvincible(false);
        timerInvincible = timeInvincible;
        visible = true;
        burnStatus.SetCanBurn(true);
        
        if (GameNetwork.Instance.IsOnline)
        {
            SetColorSpriteRendererServerRpc(new Color(255, 255, 255, 255));
        }
        else
            spriteRenderer.color = new Color(255, 255, 255, 255);
        burnStatus.AttackingPlayer = null;
    }

    [ClientRpc]
    public void AddLifeClientRpc(ClientRpcParams clientRpcParams = default)
    {
        InitializeLifeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void InitializeLifeServerRpc()
    {
        InitializeLife();
    }

    public void InitializeLife()
    {
        CurrentLife.Value = initialLife;
    }

    public override void OnNetworkDespawn()
    {
        if (GameNetwork.Instance.IsOnline && IsServer)
        {
            PlayerManager.Instance.RemovePlayer(this);
        }
    }

    public bool IsDeadAfterDamage(float damage)
    {
        return !Invincible.Value && CurrentLife.Value <= damage;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetInvincibleServerRpc(bool value)
    {
        SetInvincible(value);
    }

    public void SetInvincible(bool value)
    {
        Invincible.Value = value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetDeadValueServerRpc(bool value)
    {
        SetDeadValue(value);
    }

    public void SetDeadValue(bool value)
    {
        Dead = value;
    }
    #endregion
}