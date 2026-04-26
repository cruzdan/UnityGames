using UnityEngine;
using Unity.Netcode;
public class BoxInteractions : NetworkBehaviour
{
    #region General
    [Header("General")]
    [SerializeField] private BoxType upgradeType;
    [SerializeField] private Weapon weaponType;
    [SerializeField] private int weaponBullets;
    [SerializeField] private SpriteRenderer boxRenderer;
    [SerializeField] private BoxInfoSO boxInfoSO;
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
    public void SetWeaponType(Weapon index) { weaponType = index; }
    public void SetWeaponBullets(int total) { weaponBullets = total; }
    public SpriteRenderer BoxRenderer { get { return boxRenderer; } }
    #endregion
    #region Functions

    public override void OnNetworkSpawn()
    {
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject) && !boxHasCorrectColor)
        {
            //GetComponent<SpriteRenderer>().color = ownColor.Value;
            GetComponent<SpriteRenderer>().sprite = boxInfoSO.GetBoxInfo(upgradeType).BoxSprite;
        }
        if (!IsServer) enabled = false;
    }

    [Rpc(SendTo.ClientsAndHost)]
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
        if ((GameNetwork.IsOwnerOfflineOrOnline(NetworkObject) && !isUsed))
        {
            switch (collision.tag)
            {
                case "Box":
                    collision.GetComponent<BoxInteractions>().SetIsUsed(true);
                    GameNetwork.Instance.Despawn(collision.GetComponent<NetworkObject>(), Constants.NETWORK_OBJECT_POOL_BOX);
                    break;
                case "Player":
                    isUsed = true;
                    AddUpgrade(collision.gameObject, collision.GetComponentInParent<NetworkObject>().OwnerClientId);
                    GameNetwork.Instance.Despawn(NetworkObject, Constants.NETWORK_OBJECT_POOL_BOX);
                    break;
            }
        }
    }
    
    void AddUpgrade(GameObject playerObject, ulong playerId)
    {
        if (GameNetwork.Instance.IsOnline)
        {
            clientId[0] = playerId;
            clientRpcParams.Send.TargetClientIds = clientId;
        }
        switch (upgradeType)
        {
            case BoxType.Health:
                if (GameNetwork.Instance.IsOnline)
                    playerObject.GetComponent<Player>().AddLifeClientRpc(clientRpcParams);
                else
                    playerObject.GetComponent<Player>().InitializeLife();
                break;
            case BoxType.Speed:
                if (GameNetwork.Instance.IsOnline)
                    playerObject.GetComponent<PlayerMovement>().SetSpeedMultiplierClientRpc(1.5f, clientRpcParams);
                else
                    playerObject.GetComponent<PlayerMovement>().SetSpeedMultiplier(1.5f);
                break;
            case BoxType.Weapon:
                if (GameNetwork.Instance.IsOnline)
                    playerObject.GetComponent<Shoot>().SetCurrentWeaponClientRpc(weaponType, weaponBullets, clientRpcParams);
                else
                    playerObject.GetComponent<Shoot>().SetCurrentWeapon(weaponType, weaponBullets);
                break;
        }
    }
    #endregion
}