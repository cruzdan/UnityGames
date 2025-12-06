using Unity.Netcode;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private GameObject cameraObject;

    private void Start()
    {
        if (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject))
        {
            cameraObject.SetActive(true);
        }
    }
}
