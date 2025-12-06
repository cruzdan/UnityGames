using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private int pointsToWin = 10;
    #endregion
    #region Network Variables
    private ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Static Instance
    public static ScoreManager Instance;
    #endregion
    #region Functions
    private void Awake()
    {
        Instance = this;
        PlayerBurn.OnPlayerKilled += ScoreManager.Instance.AddScoreToPlayer;
        BulletInteractions.OnPlayerKilled += ScoreManager.Instance.AddScoreToPlayer;
    }

    public void AddScoreToPlayer(Player player)
    {
        if (!IsServer) return;
        var score = player.GetComponent<PlayerScore>();
        score.AddPoint();

        if (score.Score.Value >= pointsToWin)
        {
            ShowEndGameMenu();
        }
    }

    [ClientRpc]
    private void EndGameClientRpc(int score, ClientRpcParams clientRpcParams = default)
    {
        FindAnyObjectByType<EndGameUI>().ShowEndGameMenu(score);
        FindAnyObjectByType<UIScoreDisplay>().Player.canActiveDeadMenu = false;
    }

    void ShowEndGameMenu()
    {
        foreach (var playerConnected in PlayerManager.Instance.Players)
        {
            clientId[0] = playerConnected.NetworkObject.OwnerClientId;
            clientRpcParams.Send.TargetClientIds = clientId;
            EndGameClientRpc(playerConnected.GetComponent<PlayerScore>().Score.Value, clientRpcParams);
        }
    }
    #endregion
}
