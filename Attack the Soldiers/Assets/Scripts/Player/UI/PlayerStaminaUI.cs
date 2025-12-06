using UnityEngine;

//Class in charge of updating the player's stamina UI
public class PlayerStaminaUI : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Stamina playerStamina;
    #endregion
    #region Functions
    private void Start()
    {
        playerStamina.OnStaminaChanged += UpdateStaminaUI;
        UpdateStaminaUI();
    }

    private void UpdateStaminaUI()
    {
        playerUI.SetStaminaText(playerStamina.CurrentStamina.ToString());
        playerUI.SetStaminaWidth(playerStamina.CurrentStamina * 0.01f);
    }
    #endregion
}
