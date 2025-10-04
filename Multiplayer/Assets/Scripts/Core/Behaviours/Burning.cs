using Unity.Netcode;
using UnityEngine;

// Makes the object burn and take damage over time
public class Burning : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private bool isOffline = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float burningDuration = 3f;
    [SerializeField] private float burningDamagePerSecond = 3f;
    [SerializeField] private bool isBurning = false;
    [SerializeField] private Health health;
    [SerializeField] private Color originalColor = Color.white;
    [SerializeField] private Color burningColor = new Color(255, 103, 0, 255);
    #endregion

    #region Public Properties
    public Health Health { get => health; set => health = value; }
    #endregion

    #region Private Properties
    public float burningTimer = 0f;
    #endregion

    #region Functions
    private void Update()
    {
        //if (!isOffline && !IsOwner) return;
        if (!isOffline) return;
        if (isBurning)
        {
            burningTimer -= Time.deltaTime;
            if (burningTimer <= 0f)
            {
                StopBurning();
                return;
            }
            if (health.CanBurn)
                health.TakeDamage(burningDamagePerSecond * Time.deltaTime);
        }
    }

    public void StartBurning()
    {
        if (isBurning) return; // Evita reiniciar si ya está quemando
        SetBurningState(true, burningDuration);
    }

    public void StopBurning()
    {
        SetBurningState(false, 0f);
    }

    private void SetBurningState(bool value, float duration)
    {
        isBurning = value;
        burningTimer = duration;

        if (!isOffline)
        {
            SetIsBurningClientRpc(value, duration);
            ChangeColorClientRpc(value ? burningColor : originalColor);
        }
        else
        {
            ChangeColor(value ? burningColor : originalColor);
        }
    }

    [ClientRpc]
    public void ChangeColorClientRpc(Color color)
    {
        ChangeColor(color);
    }

    public void ChangeColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;
    }

    [ClientRpc]
    public void SetIsBurningClientRpc(bool value, float duration)
    {
        isBurning = value;
        burningTimer = duration;
    }
    #endregion
}