using System.Collections.Generic;
using UnityEngine;
//Class in charge of managing the interactions of the Faster enemy with the player,
//it can only hit a player once per attack
public class FasterEnemyInteractions : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private EnemyTouchAttack enemyTouchAttack;
    #endregion
    #region Private Variables
    private List<Player> playersHitted = new List<Player>();
    #endregion
    #region Functions
    void Start()
    {
        enemyTouchAttack.OnPlayerHitted += OnPlayerHitted;
    }

    void OnPlayerHitted(Player player)
    {
        if (!IsPlayerHitted(player))
        {
            AddPlayerHitted(player);
            player.DecrementLife(enemyTouchAttack.Damage);
        }
    }

    public void AddPlayerHitted(Player player)
    {
        if (!playersHitted.Contains(player))
            playersHitted.Add(player);
    }

    public void RemovePlayerHitted(Player player)
    {
        if (playersHitted.Contains(player))
            playersHitted.Remove(player);
    }

    public void ClearPlayersHitted()
    {
        playersHitted.Clear();
    }

    public bool IsPlayerHitted(Player player)
    {
        return playersHitted.Contains(player);
    }
    #endregion
}
