using UnityEngine;
using Unity.Netcode;

#if UNITY_EDITOR
using UnityEditor;
#endif
//Class that each enemy must have to function correctly
[RequireComponent(typeof(EnemyState))]
public class Enemy : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField, Tooltip("Name of the prefab to use in enemy pool")] 
    private string enemyName;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyDetection enemyDetection;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private BurnStatus burnStatus;
    [SerializeField] private Health health;
    [SerializeField] private PlayerAnimationController anim;
    [SerializeField] private Jump jump;
    #endregion
    #region Network Variables
    private ClientRpcParams clientRpcParams1;
    private readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Private Variables
    private Player playerTarget;
    #endregion
    #region Public Properties
    public EnemyMovement EnemyMovement => enemyMovement;
    public EnemyAttack EnemyAttack => enemyAttack;
    public EnemyDetection EnemyDetection => enemyDetection;
    public EnemyState EnemyState => enemyState;
    public Player PlayerTarget { get { return playerTarget; } set { playerTarget = value; } }
    public PlayerManager PlayerManager { get { return playerManager; } set { playerManager = value; } }
    public Animator EnemyAnimator => enemyAnimator;
    public Health Health => health;
    public NetworkObject EnemyNetworkObject => NetworkObject;
    #endregion
    #region Functions
    private void Start()
    {
        if (GameNetwork.Instance.IsOnline && !IsServer) { enabled = false; return; }
        health.OnDie += Die;
    }

    private void Update()
    {
        switch (enemyState.CurrentEnemyState)
        {
            case EnemyStateEnum.Idle:
                // Idle behavior
                IdleState();
                break;
            case EnemyStateEnum.Chasing:
                // Chasing behavior
                ChasingState();
                break;
            case EnemyStateEnum.Attacking:
                // Attacking behavior
                AttackingState();
                break;
        }

        if (transform.position.y < -25)
        {
            if (GameNetwork.Instance.IsOnline)
            {
                SetSpawnPositionServerRpc();
            }
            else
                transform.position = Spawns.Instance.GetSpawnByType(SpawnType.Enemy).position;
        }

        HandleAnimation();
    }

    [ServerRpc]
    public void SetSpawnPositionServerRpc(ServerRpcParams serverRpcParams = default)
    {
        clientId[0] = serverRpcParams.Receive.SenderClientId;
        clientRpcParams1.Send.TargetClientIds = clientId;
        SetSpawnPositionClientRpc(Spawns.Instance.GetSpawnByType(SpawnType.Enemy).position, clientRpcParams1);
    }

    [ClientRpc]
    public void SetSpawnPositionClientRpc(Vector2 pos, ClientRpcParams clientRpcParams = default)
    {
        transform.position = pos;
    }

    public void IdleState()
    {
        enemyDetection.Idle();
    }

    public void ChasingState()
    {
        enemyMovement.Chase();
    }

    public void AttackingState()
    {
        enemyAttack.Attack();
    }

    void Die()
    {
        GameNetwork.Instance.Despawn(NetworkObject, enemyName);
        EnemyManager.RemoveEnemySpawned(this);
        EnemyManager.DecreaseEnemyCount();
    }

    public void FillReferences()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyDetection = GetComponent<EnemyDetection>();
        enemyState = GetComponent<EnemyState>();
        enemyMovement.Enemy = this;
        enemyAttack.Enemy = this;
        enemyDetection.Enemy = this;
        enemyState.Enemy = this;
    }

    public void StartDetection()
    {
        enemyState.CurrentEnemyState = EnemyStateEnum.Idle;
        enemyDetection.StartDetection();
    }

    public void StartChase()
    {
        enemyState.CurrentEnemyState = EnemyStateEnum.Chasing;
        enemyMovement.StartChase();
    }

    public void StartAttack()
    {
        enemyState.CurrentEnemyState = EnemyStateEnum.Attacking;
        enemyAttack.StartAttack();
    }

    public void DecrementLife(float damage)
    {
        health.TakeDamage(damage);
    }

    [ServerRpc]
    public void StartBurningServerRpc()
    {
        burnStatus.ApplyBurn();
    }

    [ClientRpc]
    public void DecrementLifeClientRpc(float damage, ClientRpcParams clientRpcParams = default)
    {
        DecrementLife(damage);
    }

    public void Initialize()
    {
        health.InitializeHealth();
        enemyMovement.Speed = enemyMovement.InitialSpeed;
        enemyAttack.SetDamage(enemyAttack.InitialDamage);
        burnStatus.StopBurn();
        EnemyMovement.IsFacingRight = true;
        StartDetection();
    }

    void HandleAnimation()
    {
        if (!jump.IshittingGround())
            anim.SetJump();
        else if (enemyState.CurrentEnemyState == EnemyStateEnum.Chasing)
            anim.SetWalk();
        else
            anim.SetIdle();
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    private Enemy enemy;
    private void OnEnable()
    {
        enemy = (Enemy)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Fill References"))
        {
            enemy.FillReferences();
        }
    }
}
#endif