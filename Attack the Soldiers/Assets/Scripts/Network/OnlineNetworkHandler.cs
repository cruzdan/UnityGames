using Unity.Netcode;
using UnityEngine;

public class OnlineNetworkHandler : INetworkHandler
{
    private NetworkObject objectToSpawn;
    public bool IsOnline => true;

    public ulong LocalClientId => NetworkManager.Singleton.LocalClientId;

    public NetworkObject Spawn(string prefabName, Vector3 pos, Quaternion rot)
    {
        objectToSpawn = NetworkObjectPool.Singleton.GetNetworkObject(prefabName, pos, rot);
        objectToSpawn.Spawn();
        return objectToSpawn;
    }

    public void Despawn(GameObject obj)
    {
        obj.GetComponent<NetworkObject>().Despawn();
    }

    public void Despawn(NetworkObject networkObject, string prefabName)
    {
        networkObject.Despawn();
    }
}
