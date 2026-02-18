using Unity.Netcode;
using UnityEngine;

public class PlayerCanvasEnabler : NetworkBehaviour
{
    [SerializeField] private GameObject playerCanvas;
    private void Start()
    {
        playerCanvas.SetActive(GameNetwork.IsOwnerOfflineOrOnline(NetworkObject));
    }
}
