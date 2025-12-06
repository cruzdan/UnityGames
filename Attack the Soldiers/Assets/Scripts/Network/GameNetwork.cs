using Unity.Netcode;

public static class GameNetwork
{
    #region Static Fields
    public static MultiplayerModeType MultiplayerModeType;
    public static INetworkHandler Instance { get; private set; }
    #endregion
    #region Functions
    public static void StartOnline()
    {
        Instance = new OnlineNetworkHandler();
    }

    public static void StartOffline()
    {
        Instance = new OfflineNetworkHandler();
    }

    public static bool IsOwnerOfflineOrOnline(NetworkObject networkObject)
    {
        if (!GameNetwork.Instance.IsOnline)
            return true;

        return networkObject.IsOwner;
    }
    #endregion
}

public enum MultiplayerModeType
{
    PVP = 0,
    Coop = 1
}
