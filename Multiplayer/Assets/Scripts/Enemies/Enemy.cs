using UnityEngine;
using Unity.Netcode;

#if UNITY_EDITOR
using UnityEditor;
#endif
//Class that each enemy must have to function correctly
[RequireComponent(typeof(EnemyState))]
public class Enemy : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private bool isOffline = false;
    [SerializeField, Tooltip("Name of the prefab to use in enemy pool")] 
    private string enemyName;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyDetection enemyDetection;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private Burning burning;
    [SerializeField] private Health health;
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
    public bool IsOffline { get { return isOffline; } }
    public Health Health => health;
    public Burning Burning => burning;
    #endregion
    #region Functions
    private void Start()
    {
        health.OnDie += Die;
        burning.Health = health;
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
        ObjectPool.Singleton.ReturnObject(gameObject, enemyName);
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
        Burning.StartBurning();
    }

    [ServerRpc]
    public void DecrementLifeClientRpc(float damage, ClientRpcParams clientRpcParams = default)
    {
        DecrementLife(damage);
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