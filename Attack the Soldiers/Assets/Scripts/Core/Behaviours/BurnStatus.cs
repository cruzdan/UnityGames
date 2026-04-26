using UnityEngine;
using Unity.Netcode;
using System;

public class BurnStatus : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private NetworkVariable<bool> isBurning = new();
    [SerializeField] private NetworkVariable<bool> canBurn = new(true);
    [SerializeField] private float burnDuration = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject burnObject;
    [SerializeField] private Color originalColor = Color.white;
    [SerializeField] private Color burningColor = new Color(255, 103, 0, 255);
    [SerializeField] private Player ownerPlayer;
    #endregion
    #region Actions
    public Action OnBurn;
    #endregion
    #region Private Variables
    private float timer = 0;
    #endregion
    #region Public Variables
    public Player AttackingPlayer;
    #endregion
    #region Functions
    private void Start()
    {
        isBurning.OnValueChanged += OnBurnStateChanged;
        //Update the player color when a new client spawns and the object is already burning
        UpdateVisual(isBurning.Value);
        if (GameNetwork.Instance.IsOnline && !IsServer) { enabled = false; return; }
        canBurn.Value = true;
    }

    private void OnBurnStateChanged(bool previous, bool current)
    {
        UpdateVisual(current);
    }

    private void UpdateVisual(bool burning)
    {
        if (ownerPlayer != null)
        {
            if (!ownerPlayer.dead.Value)
            {
                spriteRenderer.color = burning ? burningColor : originalColor;
                if (burnObject != null)
                {
                    burnObject.SetActive(burning);
                }
            }
        }
        else
        {
            spriteRenderer.color = burning ? burningColor : originalColor;
            if (burnObject != null)
            {
                burnObject.SetActive(burning);
            }
        }
    }

    // Call to apply burn effect to this object.
    public void ApplyBurn()
    {
        if (!canBurn.Value) return;
        if (GameNetwork.Instance.IsOnline)
            StartBurnServerRpc();
        else
            StartBurn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartBurnServerRpc()
    {
        StartBurn();
    }

    private void StartBurn()
    {
        isBurning.Value = true;
        timer = 0;
    }

    public void StopBurn()
    {
        if (GameNetwork.Instance.IsOnline)
            StopBurnServerRpc();
        else
            StopBurnLocal();
    }

    [ServerRpc(RequireOwnership = false)]
    void StopBurnServerRpc()
    {
        StopBurnLocal();
    }

    void StopBurnLocal()
    {
        isBurning.Value = false;
        timer = burnDuration;
    }

    public void SetCanBurn(bool value)
    {
        SetCanBurnServerRpc(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void SetCanBurnServerRpc(bool value)
    {
        SetCanBurnLocal(value);
    }

    void SetCanBurnLocal(bool value)
    {
        canBurn.Value = value;
    }

    private void Update()
    {
        if (isBurning.Value)
        {
            timer += Time.deltaTime;
            OnBurn?.Invoke();
            if (timer >= burnDuration)
            {
                isBurning.Value = false;
                timer = 0f;
            }
        }
    }
    #endregion
}
