using UnityEngine;
using Unity.Netcode;
public class BoxInteractions : NetworkBehaviour
{
    #region General
    [Header("General")]
    [SerializeField] private bool isOffline = false;
    [SerializeField] private BoxType upgradeType;
    [SerializeField] private Weapon weaponType;
    [SerializeField] private int weaponBullets;
    private bool isUsed;
    private bool boxHasCorrectColor = false;
    #endregion
    #region Network
    public NetworkVariable<Color> ownColor = new NetworkVariable<Color>();
    private ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Public Properties
    public void SetIsUsed(bool value) { isUsed = value; }
    public void SetBoxType(BoxType boxType) { upgradeType = boxType; }
    public void SetWeaponIndex(Weapon index) { weaponType = index; }
    public void SetWeaponBullets(int total) { weaponBullets = total; }
    #endregion
    #region
    private void Start()
    {
        if (!IsOwner && !boxHasCorrectColor) 
        {
            GetComponent<SpriteRenderer>().color = ownColor.Value;
        }
    }

    [ClientRpc]
    public void ChangeColorClientRpc(Color color)
    {
        ChangeColor(color);
    }

    public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        boxHasCorrectColor = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOffline || (IsOwner && !isUsed))
        {
            switch (collision.tag)
            {
                case "Box":
                    collision.GetComponent<BoxInteractions>().SetIsUsed(true);
                    if (!isOffline)
                        NetworkObjectPool.Singleton.ReturnNetworkObject(collision.GetComponent<NetworkObject>(), "Box");
                    else
                        ObjectPool.Singleton.ReturnObject(collision.gameObject, "Offline Box");
                    break;
                case "Player":
                    isUsed = true;
                    if (!isOffline)
                    {
                        AddUpgrade(collision.gameObject, collision.GetComponent<NetworkObject>().OwnerClientId);
                        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Box");
                    }
                    else
                    {
                        AddUpgradeOffline();
                        ObjectPool.Singleton.ReturnObject(gameObject, "Offline Box");
                    }
                    break;
            }
        }
    }
    
    void AddUpgrade(GameObject playerObject, ulong playerId)
    {
        clientId[0] = playerId;
        clientRpcParams.Send.TargetClientIds = clientId;
        switch (upgradeType)
        {
            case BoxType.Health:
                playerObject.GetComponent<Player>().AddLifeClientRpc(clientRpcParams);
                break;
            case BoxType.Speed:
                playerObject.GetComponent<PlayerMovement>().SetMultiplierClientRpc(1.5f, clientRpcParams);
                break;
            case BoxType.Weapon:
                playerObject.GetComponent<Shoot>().SetCurrentWeaponClientRpc(weaponType, weaponBullets, clientRpcParams);
                break;
        }
    }

    void AddUpgradeOffline()
    {
        Player player = FindObjectOfType<Player>();
        switch (upgradeType)
        {
            case BoxType.Health:
                player.AddLife();
                break;
            case BoxType.Speed:
                player.GetComponent<PlayerMovement>().SetMultiplier(1.5f);
                break;
            case BoxType.Weapon:
                Shoot playerShoot = player.GetComponent<Shoot>();
                playerShoot.SetCurrentWeapon(weaponType, weaponBullets);
                player.PlayerUI.SetBulletText(playerShoot.CurrentBullets.ToString());
                break;
        }
    }
    #endregion
}