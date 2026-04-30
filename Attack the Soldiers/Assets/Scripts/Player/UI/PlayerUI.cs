using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private Text lifeText;
    [SerializeField] private RectTransform lifeRect;
    [SerializeField] private Text staminaText;
    [SerializeField] private RectTransform staminaRect;
    [SerializeField] private GameObject deadMenu;
    [SerializeField] private Text bulletText;
    [SerializeField] private Text deadTimeText;
    [SerializeField] private Sprite[] weaponSprites;
    [SerializeField] private Image weaponImage;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private MainMenuUI mainMenuUI;
    #endregion
    #region Private Variables
    private NetworkGameManager networkGameManager;
    private WeaponUpgraderUI weaponUpgraderUI;
    #endregion
    private void Start()
    {
        networkGameManager = FindAnyObjectByType<NetworkGameManager>();
        weaponUpgraderUI = FindAnyObjectByType<WeaponUpgraderUI>();
    }
    private void OnEnable()
    {
        mainMenuUI = FindAnyObjectByType<MainMenuUI>();
        mainMenuUI.UpgradesMenuBackButton.onClick.AddListener(OnUpgradesButtonPressed);
    }

    private void OnDisable()
    {
        mainMenuUI.UpgradesMenuBackButton.onClick.RemoveListener(OnUpgradesButtonPressed);
    }
    #region Actions
    public Action OnPausePressed;
    #endregion
    #region Functions
    public void SetLifeText(string text)
    {
        lifeText.text = text;
    }

    public void SetLifeWidth(float scaleX)
    {
        lifeRect.localScale = new Vector3(scaleX, 1, 1);
    }

    public void SetStaminaText(string text)
    {
        staminaText.text = text;
    }

    public void SetBulletText(string text)
    {
        bulletText.text = text;
    }

    public void SetStaminaWidth(float scaleX)
    {
        staminaRect.localScale = new Vector3(scaleX, 1, 1);
    }

    public void SetDeadTimeText(string text)
    {
        deadTimeText.text = text;
    }

    public void ActiveDeadMenu(bool value)
    {
        deadMenu.SetActive(value);
    }

    public void SetWeponSprite(int index)
    {
        weaponImage.sprite = weaponSprites[index];
    }

    public void ChangePauseMenu()
    {
        OnPausePressed?.Invoke();
    }

    public void OpenPauseMenu()
    {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void OnUpgradesButtonPressed()
    {
        networkGameManager.UpgradesObject.SetActive(!networkGameManager.UpgradesObject.activeSelf);
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (networkGameManager.UpgradesObject.activeSelf)
            weaponUpgraderUI.InitializeWeaponUI(weaponUpgraderUI.CurrentWeapon);

    }

    public void ReturnToMainScene()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(0);
    }
    #endregion
}
