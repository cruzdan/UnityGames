using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsteroidGameManager : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    [SerializeField] private Rigidbody2D shipRb;
    [SerializeField] private Text lifesText;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text shopMoneyText;
    [SerializeField] private AsteroidsGenerator astGen;
    [SerializeField] private SpriteChanger spriteChanger;
    [SerializeField] private ShopInformation shopInformation;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private CounterBack counter;
    //0 -> asteroidPool, 1 -> bulletPool, 2 -> explosionPool
    [SerializeField] private ObjectPool[] objectPools;
    //inital bullets when start game and restart game
    [SerializeField] private int initalBullets = 1;
    [SerializeField] private bool inGame;
    [SerializeField] private ScoreBounceTMP[] scoreBounceTMPs;
    [SerializeField] private Transform scoreParentTransform;
    [SerializeField] private Background background;
    #region DeviceType
    [SerializeField] private bool overrideDeviceType = false;
    [SerializeField] private bool isAndroid = false;
    [SerializeField] private GameObject mobileCanvas;
    [SerializeField] private DeviceChecker deviceChecker;
    public bool OverrideDeviceType { get => overrideDeviceType; set => overrideDeviceType = value; }
    public bool IsAndroid { get => isAndroid; set => isAndroid = value; }
    #endregion
    #region UI
    [SerializeField] private Toggle isOnMobileToggle;
    #endregion
    [SerializeField] private int lifes = 3;
    [SerializeField] private int money = 0;
    #region Restart
    [Header("Restart Settings")]
    [SerializeField] private float timeToRespawn = 2.0f;
    #endregion
    #region Flash Settings
    [SerializeField] private Vector3 asteroidFlashValues = new Vector3(2.5f, 5f, 0.3f);
    [SerializeField] private Vector3 shipFlashValues = new Vector3(7.5f, 15f, 0.6f);
    #endregion
    #region Public Properties
    public ObjectPool ExplosionPool => objectPools[2];
    #endregion
    public void SetLifes(int newLifes)
    {
        lifes = newLifes;
        lifesText.text = lifes.ToString();
    }
    public int GetLifes()
    {
        return lifes;
    }

    private void Awake()
    {
        //Set this variable on Game Scene to use Player Prefs value
        if (!overrideDeviceType) return;
        if (PlayerPrefs.GetInt("isMobile", 0) == 1)
        {
            overrideDeviceType = true;
            isAndroid = true;
        }
    }

    private void Start()
    {
        if (inGame) 
        { 
            counter.SetTime(-1); 
            TotalRestart(false);
            if (!overrideDeviceType)
                mobileCanvas.SetActive(deviceChecker.IsOnMobile());
            else
                mobileCanvas.SetActive(IsAndroid);
            shipRb = ship.GetComponent<Rigidbody2D>();
            MusicManager.Instance.PlayRandomMusicWithoutRepetition();
        }
        else
        {
            isOnMobileToggle.isOn = isAndroid;
            MusicManager.Instance.PlayRandomMusicWithoutRepetition(2); //play first two music clips only
        }
        
    }
    public void Restart() 
    {
        lifes--;
        if (lifes <= 0)
        {
            TotalRestart(true);
            counter.gameObject.SetActive(false);
            
        }
        else
        {
            PartialRestart();
            pauseManager.pause = true;
            counter.Reiniciate();
            counter.gameObject.SetActive(true);
            lifesText.text = lifes.ToString();
        }
        ship.SetActive(true);
        astGen.enabled = true;
    }
    void ReturnObjectsToPool(string tag, ObjectPool obPool)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        int total = objects.Length;
        for(int i = 0; i < total; i++)
        {
            obPool.ReturnObjectToPool(objects[i]);
        }
    }

    public void SetMoney(int newMoney)
    {
        money = newMoney;
        moneyText.text = money.ToString();
        shopMoneyText.text = money.ToString();
        BounceScore();
    }

    void BounceScore()
    {
        int total = scoreBounceTMPs.Length;
        for (int i = 0; i < total; i++)
        {
            scoreBounceTMPs[i].Bounce();
        }
    }

    public int GetMoney()
    {
        return money;
    }

    void PartialRestart()
    {
        ReturnObjectsToPool("Bullet", objectPools[1]);
        ReturnObjectsToPool("Asteroid", objectPools[0]);
        ReturnObjectsToPool("Explosion", objectPools[2]);
        ship.GetComponent<ShipMovement>().Restart();
        ship.GetComponent<Shoot>().Restart();
    }
    public void TotalRestart(bool toPause)
    {
        lifes = 3;
        lifesText.text = lifes.ToString();
        money = 0;
        moneyText.text = money.ToString();
        shopMoneyText.text = money.ToString();
        ship.GetComponent<ShipMovement>().InitSpeed();
        ship.GetComponent<ShipMovement>().InitRotation();
        ship.GetComponent<Shoot>().bulletsToShoot = initalBullets;
        astGen.SetBonus(0);
        spriteChanger.Init();
        shopInformation.Init();
        if(toPause)
            pauseManager.GameOverChange();
        PartialRestart();
        background.ChangeBackground();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ChangeGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SaveIsMobile()
    {
        PlayerPrefs.SetInt("isMobile", isOnMobileToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnShipCollided()
    {
        astGen.enabled = false;
        ship.SetActive(false);
        shipRb.linearVelocity = Vector2.zero;
        CameraShake.Instance.Shake(0.2f, 0.2f);
        Invoke(nameof(Restart), timeToRespawn);
        GenerateExplosion(ship.transform.position, shipFlashValues);
    }

    public void OnAsteroidDestroyed(Vector2 explosionPosition, int score)
    {
        GenerateExplosion(explosionPosition, asteroidFlashValues);
    }

    void GenerateExplosion(Vector2 explosionPosition, Vector3 explosionValues)
    {
        GameObject explosion = ExplosionPool.GetObjectFromPool();
        explosion.transform.position = explosionPosition;
        ShootFlash shootFlash = explosion.GetComponent<ShootFlash>();
        shootFlash.SetFlashValues(explosionValues);
        shootFlash.OnFlashEnd = () =>
        {
            ExplosionPool.ReturnObjectToPool(explosion);
        };
        shootFlash.Flash();
    }

    public void ChangeMusic()
    {
        MusicManager.Instance.PlayRandomMusicWithoutRepetition();
    }

    public void PlayClickSFX()
    {
        SFXManager.Instance.PlaySFX(AsteroidsSFX.Instance.ClickClip);
    }
}
