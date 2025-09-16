using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BoxInteractions : NetworkBehaviour
{
    public NetworkVariable<Color> ownColor = new NetworkVariable<Color>();
    bool destroyed;

    bool started = false;

    //health, speed, weapon
    [SerializeField] private int upgradeIndex;
    //pistol, shotgun, machine gun, sniper
    [SerializeField] private int weaponIndex;
    [SerializeField] private int weaponBullets;
    ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];
    public void SetDestroyed(bool value) { destroyed = value; }
    public void SetUpgradeIndex(int index) { upgradeIndex = index; }
    public void SetWeaponIndex(int index) { weaponIndex = index; }
    public void SetWeaponBullets(int total) { weaponBullets = total; }
    private void Start()
    {
        if (!IsOwner && !started) 
        {
            GetComponent<SpriteRenderer>().color = ownColor.Value;
        }
    }

    [ClientRpc]
    public void ChangeColorClientRpc(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        started = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsOwner && !destroyed)
        {
            switch (collision.tag)
            {
                case "Box":
                    collision.GetComponent<BoxInteractions>().SetDestroyed(true);
                    NetworkObjectPool.Singleton.ReturnNetworkObject(collision.GetComponent<NetworkObject>(), "Box");
                    break;
                case "Player":
                    destroyed = true;
                    AddUpgrade(collision.gameObject, collision.GetComponent<NetworkObject>().OwnerClientId);
                    NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Box");
                    break;
            }
        }
    }
    
    void AddUpgrade(GameObject playerObject, ulong playerId)
    {
        clientId[0] = playerId;
        clientRpcParams.Send.TargetClientIds = clientId;
        switch (upgradeIndex)
        {
            case 0:
                playerObject.GetComponent<Player>().AddLifeClientRpc(clientRpcParams);
                break;
            case 1:
                playerObject.GetComponent<PlayerMovement>().SetMultiplierClientRpc(1.5f, clientRpcParams);
                break;
            case 2:
                playerObject.GetComponent<Shoot>().SetCurrentWeaponClientRpc(weaponIndex, weaponBullets, clientRpcParams);
                break;
        }
    }
}