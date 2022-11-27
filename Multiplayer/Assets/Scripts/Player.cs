using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    
    [SerializeField] private const int MaxLife = 100;
    private int currentLife;
    [SerializeField] private float timeInvincible;
    
    bool invincible;
    bool visible = true;
    float timerInvincible;
    [SerializeField] private float deadWaitTime = 3f;
    bool dead;
    float timerDead;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private GameObject canvas;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Shoot playerShoot;
    SpriteRenderer spriteRenderer;
    bool restarting = false;

    ClientRpcParams clientRpcParams1;
    private readonly ulong[] clientId = new ulong[1];
    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!IsOwner) return;
        canvas.SetActive(true);
        PlayerCameraFollow.Instance.FollowPlayer(transform);
        currentLife = MaxLife;
        timerInvincible = timeInvincible;
        timerDead = deadWaitTime;
        SetSpawnPositionServerRpc();
        
    }

    void Update()
    {
        if (!IsOwner || restarting) return;
        if (dead)
        {
            int a = (int)timerDead;
            timerDead -= Time.deltaTime;
            if (a != (int)timerDead)
            {
                a = (int)timerDead;
                playerUI.SetDeadTimeText(a.ToString());
            }
            if (timerDead <= 0)
            {
                dead = false;
                timerDead = deadWaitTime;

                playerMovement.enabled = true;
                playerShoot.enabled = true;
                playerUI.ActiveDeadMenu(false);
            }
        }
        else if (invincible)
        {
            timerInvincible -= Time.deltaTime;
            int k = (int)(timerInvincible * 10) % 5;
            if (k < 2.5f)
            {
                if (visible)
                {
                    visible = false;
                    SetColorSpriteRendererServerRpc(new Color(255, 255, 255, 0));
                }
            }
            else
            {
                if (!visible)
                {
                    visible = true;
                    SetColorSpriteRendererServerRpc(new Color(255, 255, 255, 255));
                }
            }
            if (timerInvincible < 0)
            {
                ResetPlayer();
            }
            
        }
        if (transform.position.y < -25)
        {
            restarting = true;
            ResetPlayer();
            DecrementLife(MaxLife);
        }
    }

    [ClientRpc]
    public void SetColorSpriteRendererClientRpc(Color color)
    {
        spriteRenderer.color = color;
    }
    [ServerRpc]
    public void SetColorSpriteRendererServerRpc(Color color)
    {
        SetColorSpriteRendererClientRpc(color);
    }

    [ClientRpc]
    public void SetPositionClientRpc(Vector2 pos, ClientRpcParams clientRpcParams = default)
    {
        transform.position = pos;
    }

    [ClientRpc]
    public void SetSpawnPositionClientRpc(Vector2 pos, ClientRpcParams clientRpcParams = default)
    {
        transform.position = pos;
        restarting = false;
    }

    [ServerRpc]
    public void SetSpawnPositionServerRpc(ServerRpcParams serverRpcParams = default)
    {
        clientId[0] = serverRpcParams.Receive.SenderClientId;
        clientRpcParams1.Send.TargetClientIds = clientId;
        SetSpawnPositionClientRpc(Spawns.Instance.GetPlayerSpawnPoint().position, clientRpcParams1);
    }
    public void DecrementLife(int damage)
    {
        if (!invincible)
        {
            currentLife -= damage;
            if (currentLife <= 0)
            {
                currentLife = MaxLife;
                invincible = true;
                playerMovement.SetMaxStamina();
                playerMovement.RestartVelocity();
                playerShoot.SetCurrentWeapon(0, 100);
                playerMovement.enabled = false;
                playerShoot.enabled = false;
                dead = true;
                SetColorSpriteRendererServerRpc(new Color(0.1f, 0.1f, 0.1f, 0.4f));
                playerUI.ActiveDeadMenu(true);
                playerUI.SetDeadTimeText(deadWaitTime.ToString());
                SetSpawnPositionServerRpc();
            }
            playerUI.SetLifeText(currentLife.ToString());
            playerUI.SetLifeWidth(currentLife * 0.01f);
        }
    }
    [ClientRpc]
    public void DecrementLifeClientRpc(int damage, ClientRpcParams clientRpcParams = default)
    {
        DecrementLife(damage);
    }
    void ResetPlayer()
    {
        invincible = false;
        timerInvincible = timeInvincible;
        visible = true;
        SetColorSpriteRendererServerRpc(new Color(255, 255, 255, 255));
    }
    [ClientRpc]
    public void AddLifeClientRpc(ClientRpcParams clientRpcParams = default)
    {
        currentLife = MaxLife;
        playerUI.SetLifeText(currentLife.ToString());
        playerUI.SetLifeWidth(currentLife * 0.01f);
    }
}