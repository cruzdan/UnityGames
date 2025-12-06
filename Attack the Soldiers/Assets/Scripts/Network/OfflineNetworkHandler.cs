using Unity.Netcode;
using UnityEngine;

public class OfflineNetworkHandler : INetworkHandler
{
    public bool IsOnline => false;

    public ulong LocalClientId => 0;

    public NetworkObject Spawn(string prefabName, Vector3 pos, Quaternion rot)
    {
        return NetworkObjectPool.Singleton.GetNetworkObject(prefabName, pos, rot);
    }

    public void Despawn(NetworkObject networkObject, string prefabName)
    {
        NetworkObjectPool.Singleton.ReturnNetworkObjectByName(networkObject, prefabName);
    }
}
