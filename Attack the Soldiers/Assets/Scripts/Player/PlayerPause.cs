using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

//Class in charge of pausing the game for the player
public class PlayerPause : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private PlayerInput playerInput;
    #endregion
    #region Private Variables
    private bool pause = false;
    #endregion
    #region Public Properties
    public bool Pause { get => pause; set => pause = value; }
    #endregion
    #region Actions
    public Action OnPausePressed;
    #endregion
    #region Functions
    public override void OnNetworkSpawn()
    {
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) enabled = false;
    }

    private void Update()
    {
        HandlePause();
    }

    private void HandlePause()
    {
        if (playerInput.actions[Constants.INPUT_KEY_PAUSE].WasPressedThisFrame())
        {
            OnPausePressed?.Invoke();
        }
    }
    #endregion
}
