using System;
using UnityEngine;
//Class in charge of detecting collisions with the player and use an event with a 2D trigger
public class EnemyTouchAttack : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private bool isOffline = false;
    [SerializeField] private int damage;
    #endregion
    #region Private Variables
    private Player playerHitted;
    #endregion
    #region Public Events
    public Action<Player> OnPlayerHitted;
    #endregion
    #region Public Properties
    public int Damage { get => damage; set => damage = value; }
    #endregion
    #region Functions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                playerHitted = collision.gameObject.GetComponent<Player>();
                OnPlayerHitted?.Invoke(playerHitted);
                break;
        }
    }
    #endregion
}
