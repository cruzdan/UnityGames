using UnityEngine;
using Unity.Netcode;

public class Teleporter : NetworkBehaviour
{
    #region Network Variables
    //public NetworkVariable<Color> ownColor = new NetworkVariable<Color>();
    public NetworkVariable<int> ownSpriteIndex = new NetworkVariable<int>();
    private ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Portals
    [SerializeField] private Transform nextPortal;
    //enabled and disabled
    //[SerializeField] private Color[] portalColors;
    [SerializeField] private Sprite[] portalSprites;
    [SerializeField] private float timeToActivePortal = 20f;
    private Teleporter nextTeleporter;
    private bool active = true;
    private float timer;
    private SpriteRenderer spriteRenderer;
    #endregion
    #region Functions
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextTeleporter = nextPortal.GetComponent<Teleporter>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            //ownColor.Value = portalColors[0];
            ownSpriteIndex.Value = 0;
            spriteRenderer.sprite = portalSprites[0];
        }
        else
        {
            //spriteRenderer.color = ownColor.Value;
            spriteRenderer.sprite = portalSprites[ownSpriteIndex.Value];
        }
        if (!IsServer) enabled = false;
    }

    void Update()
    {
        if (!active)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                SetActivePortal(true);
            }
        }
    }
    public void SetActivePortal(bool value)
    {
        active = value;
        timer = timeToActivePortal;
        if (value)
        {
            if (!GameNetwork.Instance.IsOnline)
                //ChangePortalColor(portalColors[0]);
                ChangePortalSprite(0);
            else
                //ChangePortalColorClientRpc(portalColors[0]);
                ChangePortalSpriteClientRpc(0);
            //ownColor.Value = portalColors[0];
            ownSpriteIndex.Value = 0;
        }
        else
        {
            if (!GameNetwork.Instance.IsOnline)
                //ChangePortalColor(portalColors[1]);
                ChangePortalSprite(1);
            else
                //ChangePortalColorClientRpc(portalColors[1]);
                ChangePortalSpriteClientRpc(1);
            //ownColor.Value = portalColors[1];
            ownSpriteIndex.Value = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && active && (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject) || !GameNetwork.Instance.IsOnline))
        {
            if (GameNetwork.Instance.IsOnline)
            {
                clientId[0] = collision.GetComponentInParent<NetworkObject>().OwnerClientId;
                clientRpcParams.Send.TargetClientIds = clientId;
                collision.GetComponent<Player>().SetPositionClientRpc(nextPortal.position, clientRpcParams);
            }
            else
            {
                collision.GetComponent<Player>().SetPosition(nextPortal.position);
            }
            nextTeleporter.SetActivePortal(false);
            SetActivePortal(false);
        }
    }

    [ClientRpc]
    public void ChangePortalColorClientRpc(Color color)
    {
        ChangePortalColor(color);
    }

    public void ChangePortalColor(Color color)
    {
        spriteRenderer.color = color;
    }

    [ClientRpc]
    public void ChangePortalSpriteClientRpc(int spriteIndex)
    {
        ChangePortalSprite(spriteIndex);
    }

    public void ChangePortalSprite(int spriteIndex)
    {
        spriteRenderer.sprite = portalSprites[spriteIndex];
    }
    #endregion
}
