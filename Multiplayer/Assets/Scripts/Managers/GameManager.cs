using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private BulletInfoSO bulletInfoSO;
    [SerializeField] private BoxInfoSO boxInfoSO;
    [SerializeField] private BoxManager boxManager;
    [SerializeField] private bool isOffline = true;
    [Header("Options")]
    [SerializeField] private bool activeEnemyManager = true;
    [SerializeField] private bool activeBoxManager = true;
    #endregion
    #region Public Properties
    public BulletInfoSO BulletInfoSO => bulletInfoSO;
    public BoxInfoSO BoxInfoSO => boxInfoSO;
    public bool ActiveBoxManager => activeBoxManager;
    public bool ActiveEnemyManager => activeEnemyManager;
    #endregion
    #region Functions
    private void Awake()
    {
        bulletInfoSO.InitializeBulletInfoDictionary();
        boxInfoSO.InitializeBoxDictionary();
    }

    private void Start()
    {
        if (isOffline)
        {
            //playerTransform.position = Spawns.Instance.GetPlayerSpawnPoint().position;
            if (activeEnemyManager)
                enemyManager.StartSpawning();
            if (activeBoxManager)
            {

                boxManager.Initialize();
                boxManager.gameObject.SetActive(true);
            }
        }
        else
        {
            //boxManager.Initialize();
        }
    }
    #endregion
}
