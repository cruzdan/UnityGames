using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private ObjectContainerUI playerContainerUI;
    [SerializeField] private List<Player> players = new List<Player>();
    #endregion
    #region Static Variables
    public static PlayerManager Instance { get; private set; }
    #endregion
    #region Public Properties
    public List<Player> Players { get => players; set => players = value; }
    #endregion
    #region Functions
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddPlayer(Player player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
            playerContainerUI.AddObject(player.gameObject);
        }
    }

    public void RemovePlayer(Player player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
            playerContainerUI.RemoveObject(player.gameObject);
            EnemyManager.OnPlayerDisconnected(player);
        }
    }
    #endregion
}
