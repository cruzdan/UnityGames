using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletInteractions : NetworkBehaviour
{
    [SerializeField] private int damage;
    ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];
    bool dead = false;
    public void SetDead(bool value) { dead = value; }
    public void SetDamage(int value) { damage = value; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsOwner && !dead)
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    dead = true;
                    clientId[0] = collision.GetComponent<NetworkObject>().OwnerClientId;
                    clientRpcParams.Send.TargetClientIds = clientId;
                    collision.gameObject.GetComponent<Player>().DecrementLifeClientRpc(damage, clientRpcParams);
                    NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Bullet");
                    break;
                case "Floor":
                    dead = true;
                    NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Bullet");
                    break;
            }
        }
    }
    [ClientRpc]
    public void ChangeColorClientRpc(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}