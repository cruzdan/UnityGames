using Unity.Netcode;

public static class NetworkBehaviourExtensions
{
    public static  string GetDebugInfo(this NetworkBehaviour behaviour)
    {
        return $"IsSpawned: {behaviour.IsSpawned}, IsServer: {behaviour.IsServer}, IsClient: {behaviour.IsClient}, IsOwner: {behaviour.IsOwner}, NetworkObjectId: {behaviour.NetworkObjectId}, OwnerClientId: {behaviour.OwnerClientId}";
    }
}