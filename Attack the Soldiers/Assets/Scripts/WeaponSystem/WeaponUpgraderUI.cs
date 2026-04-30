using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgraderUI : MonoBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private WeaponLevelManager weaponLevelManager;
    [SerializeField] private BulletInfoSO bulletInfoSO;
    [SerializeField] private WeaponLevelConfig levelConfig;
    [SerializeField] private WeaponUpgradeHandler upgradeHandler;
    [SerializeField] private WeaponSpritesSO weaponSpritesSO;
    [SerializeField] private WeaponLevelManager levelManager;
    [SerializeField] private MainMenuUI mainMenuUI;
    #endregion
    #region Private Variables
    private Weapon currentWeapon = Weapon.Pistol;
    private WeaponLevelData weaponLevelData;
    #endregion
    #region UI
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI weaponNameTextUI;
    [SerializeField] private TextMeshProUGUI weaponLevelTextUI;
    [SerializeField] private TextMeshProUGUI weaponXPTextUI;
    [SerializeField] private TextMeshProUGUI currentWeaponDamageValueUI;
    [SerializeField] private TextMeshProUGUI currentWeaponFireRateValueUI;
    [SerializeField] private TextMeshProUGUI currentWeaponDistanceValueUI;
    [SerializeField] private TextMeshProUGUI upgradeWeaponDamageValueUI;
    [SerializeField] private TextMeshProUGUI upgradeWeaponFirerateValueUI;
    [SerializeField] private TextMeshProUGUI upgradeWeaponDistanceValueUI;
    [SerializeField] private TextMeshProUGUI currentLevelWeaponDamageValueUI;
    [SerializeField] private TextMeshProUGUI currentLevelWeaponFirerateValueUI;
    [SerializeField] private TextMeshProUGUI currentLevelWeaponDistanceValueUI;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button firerateButton;
    [SerializeField] private Button distanceButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image damageImage;
    [SerializeField] private Image timeToShootImage;
    [SerializeField] private Image distanceImage;
    [SerializeField] private Color disableImageColor;
    #endregion
    #region Actions
    public static Action<Weapon> OnUpgradeSelected;
    #endregion
    #region Public Properties
    public Weapon CurrentWeapon => currentWeapon;
    #endregion
    private void Start()
    {
        InitializeWeaponUI(currentWeapon);
        AddButtonEvents();
        mainMenuUI.OnShowUpgrades += () => InitializeWeaponUI(currentWeapon);
    }
    public void InitializeWeaponUI(Weapon weapon)
    {
        weaponLevelData = weaponLevelManager.GetWeaponData(weapon);
        AddWeaponInfoToUI(weapon);
        ActiveCorrectButtons();
        currentWeapon = weapon;
    }

    void AddWeaponInfoToUI(Weapon weapon)
    {
        weaponNameTextUI.text = weapon.ToString();
        weaponLevelTextUI.text = $"{weaponLevelData.currentLevel}";
        weaponXPTextUI.text = $"{weaponLevelData.currentXP} / {levelConfig.GetXPRequiredForLevel(weaponLevelData.currentLevel)}";

        BulletInfo bulletInfo = bulletInfoSO.GetBulletInfoByWeapon(weapon);
        upgradeHandler.GetModifiedDamage(weapon, bulletInfo.Damage);

        currentWeaponDamageValueUI.text = $"Current V: {upgradeHandler.GetModifiedDamage(weapon, bulletInfo.Damage)}";
        currentWeaponFireRateValueUI.text = $"Current V: {upgradeHandler.GetModifiedTimeToShoot(weapon, bulletInfo.TimeToShoot)}";
        currentWeaponDistanceValueUI.text = $"Current V: {upgradeHandler.GetModifiedMaxDistance(weapon, bulletInfo.MaxDistance)}";

        upgradeWeaponDamageValueUI.text = $"+{weaponLevelManager.LevelConfig.WeaponUpgradeValues[(int)weapon].DamageIncrease}";
        upgradeWeaponFirerateValueUI.text = $"-{weaponLevelManager.LevelConfig.WeaponUpgradeValues[(int)weapon].TimeToShootDecrease}";
        upgradeWeaponDistanceValueUI.text = $"+{weaponLevelManager.LevelConfig.WeaponUpgradeValues[(int)weapon].MaxDistanceIncrease}";

        currentLevelWeaponDamageValueUI.text = $"{weaponLevelData.totalDamageUpgrades} / {levelConfig.MaxLevel}";
        currentLevelWeaponFirerateValueUI.text = $"{weaponLevelData.totalTimeToShootUpgrades} / {levelConfig.MaxLevel}";
        currentLevelWeaponDistanceValueUI.text = $"{weaponLevelData.totalMaxDistanceUpgrades} / {levelConfig.MaxLevel}";

        weaponImage.sprite = weaponSpritesSO.GetWeaponSprite(weapon);
    }

    void AddButtonEvents()
    {
        damageButton.onClick.AddListener(() => ApplyUpgradeIfPossible(WeaponUpgradeType.Damage));
        firerateButton.onClick.AddListener(() => ApplyUpgradeIfPossible(WeaponUpgradeType.TimeToShoot));
        distanceButton.onClick.AddListener(() => ApplyUpgradeIfPossible(WeaponUpgradeType.MaxDistance));
        nextButton.onClick.AddListener(() => ChangeWeapon(1));
        previousButton.onClick.AddListener(() => ChangeWeapon(-1));
    }

    void ApplyUpgradeIfPossible(WeaponUpgradeType type)
    {
        if (weaponLevelData.currentLevel <= (weaponLevelData.totalDamageUpgrades + weaponLevelData.totalTimeToShootUpgrades + weaponLevelData.totalMaxDistanceUpgrades))
            return;

        levelManager.ApplyUpgrade(currentWeapon, type);
        OnUpgradeSelected?.Invoke(currentWeapon);
        InitializeWeaponUI(currentWeapon);
    }

    void ChangeWeapon(int direction)
    {
        int weaponCount = Enum.GetValues(typeof(Weapon)).Length;
        int currentIndex = Array.IndexOf(Enum.GetValues(typeof(Weapon)), currentWeapon);
        int newIndex = (currentIndex + direction + weaponCount) % weaponCount;
        currentWeapon = (Weapon)Enum.GetValues(typeof(Weapon)).GetValue(newIndex);
        InitializeWeaponUI(currentWeapon);
    }

    void ActiveCorrectButtons()
    {
        int totalUpgrades = weaponLevelData.totalDamageUpgrades + weaponLevelData.totalTimeToShootUpgrades + weaponLevelData.totalMaxDistanceUpgrades;
        bool hasUpgradesAvailable = weaponLevelData.currentLevel > totalUpgrades;
        SetInteractableUpgradeButtons(
            hasUpgradesAvailable && weaponLevelData.totalDamageUpgrades < levelConfig.MaxLevel,
            hasUpgradesAvailable && weaponLevelData.totalTimeToShootUpgrades < levelConfig.MaxLevel,
            hasUpgradesAvailable && weaponLevelData.totalMaxDistanceUpgrades < levelConfig.MaxLevel
            );

    }

    void SetInteractableUpgradeButtons(bool damageInteractable, bool firerateInteractable, bool distanceInteractable)
    {
        damageButton.interactable = damageInteractable;
        damageImage.color = damageInteractable ? Color.white : disableImageColor;
        firerateButton.interactable = firerateInteractable;
        timeToShootImage.color = firerateInteractable ? Color.white : disableImageColor;
        distanceButton.interactable = distanceInteractable;
        distanceImage.color = distanceInteractable ? Color.white : disableImageColor;
    }
}
