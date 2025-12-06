using Unity.Netcode;

public class PlayerScore : NetworkBehaviour
{
    #region Public Variables
    public NetworkVariable<int> Score = new NetworkVariable<int>(0);
    #endregion
    #region Functions
    public override void OnNetworkSpawn()
    {
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) return;
        FindAnyObjectByType<UIScoreDisplay>().Initialize(GetComponent<Player>());
    }

    public void AddPoint()
    {
        if (IsServer)
        {
            Score.Value++;
        }
    }
    #endregion
}
