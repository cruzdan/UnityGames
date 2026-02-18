using Unity.Netcode;
using UnityEngine;

public class TestRpcs : NetworkBehaviour
{
    [SerializeField] private CinemachineCameraShake cinemachineCameraShake;
    private void Start()
    {
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) { enabled = false; return; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Shake local");
            cinemachineCameraShake.Shake(10f, .3f);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Shake Server");
            ShakeServerRpc(10,.3f);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Shake All");
            ShakeAllRpc(10, .3f);
        }
    }
    [Rpc(SendTo.ClientsAndHost)]
    void ShakeAllRpc(float intensity, float duration)
    {
        if (CinemachineCameraShake.Instance != null)
        {
            CinemachineCameraShake.Instance.Shake(intensity, duration);
        }
    }

    [ServerRpc]
    void ShakeServerRpc(float intensity, float duration)
    {
        CinemachineCameraShake.Instance.Shake(intensity, duration);
    }
}
