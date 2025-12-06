using Unity.Netcode;
using UnityEngine;
//Class in charge of detecting collisions with the player and use an event with a 2D trigger
public class EnemyTouchAttack : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private int damage;
    #endregion
    #region Network Variables
    private ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];
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
                Player player = collision.GetComponent<Player>();
                if (GameNetwork.Instance.IsOnline)
                {
                    clientId[0] = collision.GetComponentInParent<NetworkObject>().OwnerClientId;
                    clientRpcParams.Send.TargetClientIds = clientId;
                    player.DecrementLifeClientRpc(damage, clientRpcParams);
                }
                else
                {
                    player.DecrementLife(damage);
                }
                break;
        }
    }
    #endregion
}
