using UnityEngine;
using Unity.Netcode;

public class BulletInteractions : NetworkBehaviour
{
    [Header("General")]
    [SerializeField] private bool isOffline = false; // ? Modo offline
    [SerializeField] private int damage;
    ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];
    bool dead = false;
    public void SetDead(bool value) { dead = value; }
    public void SetDamage(int value) { damage = value; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOffline || (IsOwner && !dead))
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    dead = true;
                    if (!isOffline)
                    {
                        clientId[0] = collision.GetComponent<NetworkObject>().OwnerClientId;
                        clientRpcParams.Send.TargetClientIds = clientId;
                        collision.gameObject.GetComponent<Player>().DecrementLifeClientRpc(damage, clientRpcParams);
                        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Bullet");
                    }
                    else
                    {
                        ObjectPool.Singleton.ReturnObject(gameObject, "Offline Bullet");
                        collision.gameObject.GetComponent<Player>().DecrementLife(damage);
                        //gameObject.SetActive(false);
                    }
                    break;
                case "Floor":
                    dead = true;
                    if (!isOffline)
                        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Bullet");
                    else
                        ObjectPool.Singleton.ReturnObject(gameObject, "Offline Bullet");
                    //gameObject.SetActive(false);
                    //NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Bullet");
                    break;
            }
        }
    }
    [ClientRpc]
    public void ChangeColorClientRpc(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}