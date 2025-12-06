using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Class to manage cheat information for individual players in the UI.
public class PlayerCheatInfo : MonoBehaviour
{
    #region Public Variables
    public GameObject PlayerInfoObject;
    public TextMeshProUGUI PlayerIDText;
    public TMP_Dropdown PlayerWeaponDropdown;
    public Slider PlayerSpeedSlider;
    public Toggle StaminaToggle;
    public Toggle AmmoToggle;
    public Slider JumpSlider;
    #endregion
    #region Actions
    public Action<int, ulong> OnPlayerWeaponChangedAction;
    public Action<float, ulong> OnPlayerSpeedChangedAction;
    public Action<bool, ulong> OnStaminaToggleChangedAction;
    public Action<bool, ulong> OnAmmoToggleChangedAction;
    public Action<float, ulong> OnPlayerJumpChangedAction;
    #endregion
    #region Functions
    private void Awake()
    {
        AddPlayerPanelEvents();
    }

    void AddPlayerPanelEvents()
    {
        PlayerWeaponDropdown.onValueChanged.AddListener(OnPlayerWeaponChanged);
        PlayerSpeedSlider.onValueChanged.AddListener(OnPlayerSpeedChanged);
        StaminaToggle.onValueChanged.AddListener(OnStaminaToggleChanged);
        AmmoToggle.onValueChanged.AddListener(OnAmmoToggleChanged);
        JumpSlider.onValueChanged.AddListener(OnPlayerJumpChanged);
    }

    void OnPlayerWeaponChanged(int index)
    {
        OnPlayerWeaponChangedAction?.Invoke(index, ulong.Parse(PlayerIDText.text));
    }

    void OnPlayerSpeedChanged(float value)
    {
        OnPlayerSpeedChangedAction?.Invoke(value, ulong.Parse(PlayerIDText.text));
    }

    void OnStaminaToggleChanged(bool value)
    {
        OnStaminaToggleChangedAction?.Invoke(value, ulong.Parse(PlayerIDText.text));
    }

    void OnAmmoToggleChanged(bool value)
    {
        OnAmmoToggleChangedAction?.Invoke(value, ulong.Parse(PlayerIDText.text));
    }

    void OnPlayerJumpChanged(float value)
    {
        OnPlayerJumpChangedAction?.Invoke(value, ulong.Parse(PlayerIDText.text));
    }
    #endregion
}
