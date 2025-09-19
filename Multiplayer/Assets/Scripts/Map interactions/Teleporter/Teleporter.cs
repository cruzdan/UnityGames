using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Teleporter : NetworkBehaviour
{
    [Header("General")]
    [SerializeField] private bool isOffline = false;

    public NetworkVariable<Color> ownColor = new NetworkVariable<Color>();
    [SerializeField] private Transform nextPortal;
    //enabled and disabled
    [SerializeField] private Color[] portalColors;
    private Teleporter nextTeleporter;
    bool active = true;
    [SerializeField] private float timeToActivePortal = 20f;
    float timer;

    ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];

    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextTeleporter = nextPortal.GetComponent<Teleporter>();
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            ownColor.Value = portalColors[0];
        }
        else
        {
            spriteRenderer.color = ownColor.Value;
        }
        base.OnNetworkSpawn();
    }

    void Update()
    {
        if (!isOffline && !IsOwner) return;
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
            if (isOffline)
            {
                ChangePortalColor(portalColors[0]);
            }
            else
            {
                ChangePortalColorClientRpc(portalColors[0]);
            }
            ownColor.Value = portalColors[0];
        }
        else
        {
            if (isOffline)
            {
                ChangePortalColor(portalColors[1]);
            }
            else
            {
                ChangePortalColorClientRpc(portalColors[1]);
            }
            ownColor.Value = portalColors[1];
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && active && (IsOwner || isOffline))
        {
            if (!isOffline)
            {
                clientId[0] = collision.GetComponent<NetworkObject>().OwnerClientId;
                clientRpcParams.Send.TargetClientIds = clientId;
                collision.GetComponent<Player>().SetPositionClientRpc(nextPortal.position, clientRpcParams);
                nextTeleporter.SetActivePortal(false);
            }
            else
            {
                collision.GetComponent<Player>().SetPosition(nextPortal.position);
                nextTeleporter.SetActivePortal(false);
            }
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
}
