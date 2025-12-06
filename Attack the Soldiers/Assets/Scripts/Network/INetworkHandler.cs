using Unity.Netcode;
using UnityEngine;
public interface INetworkHandler
{
    bool IsOnline { get; }
    ulong LocalClientId { get; }

    NetworkObject Spawn(string prefabName, Vector3 pos, Quaternion rot);
    void Despawn(NetworkObject networkObject, string prefabName);
}
