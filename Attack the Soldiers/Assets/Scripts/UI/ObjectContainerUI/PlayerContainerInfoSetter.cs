using Unity.Netcode;
using UnityEngine;

//Class to set player information in a UI container for cheat management.
public class PlayerContainerInfoSetter : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private ObjectContainerUI playerContainerUI;
    #endregion
    #region Functions
    public override void OnNetworkSpawn()
    {
        if (!GameNetwork.Instance.IsOnline) return;
        playerContainerUI.OnInitialized += HandleContainerInitialized;
        playerContainerUI.Initialize();
    }

    void HandleContainerInitialized()
    {
        int objectsPerPage = playerContainerUI.ObjectsInUI.Length;
        int startIndex = playerContainerUI.CurrentPage * objectsPerPage;
        for (int i = 0; i < playerContainerUI.ObjectsInUI.Length; i++)
        {
            int objectIndex = startIndex + i;
            if (objectIndex < playerContainerUI.ObjectListToShow.Count)
            {
                PlayerCheatInfo playerInfo = playerContainerUI.ObjectsInUI[i].GetComponent<PlayerCheatInfo>();
                Player player = PlayerManager.Instance.Players[objectIndex];
                SetPlayerInfo(playerInfo, player);
            }
            else
            {
                break;
            }
        }
    }

    void SetPlayerInfo(PlayerCheatInfo playerCheatInfo, Player player)
    {
        playerCheatInfo.PlayerIDText.text = player.NetworkObjectId.ToString();
        playerCheatInfo.PlayerWeaponDropdown.SetValueWithoutNotify((int)player.Shoot.CurrentBulletInfo.WeaponType);
        playerCheatInfo.PlayerSpeedSlider.SetValueWithoutNotify(player.PlayerMovement.WalkSpeedX);
        playerCheatInfo.StaminaToggle.SetIsOnWithoutNotify(player.PlayerMovement.UseStamina);
        playerCheatInfo.AmmoToggle.SetIsOnWithoutNotify(player.Shoot.Infinite);
        playerCheatInfo.JumpSlider.SetValueWithoutNotify(player.PlayerMovement.Jump.JumpSpeed);
    }
    #endregion
}
