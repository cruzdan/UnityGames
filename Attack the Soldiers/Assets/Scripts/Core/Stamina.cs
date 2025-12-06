using System;
using Unity.Netcode;
using UnityEngine;

public class Stamina : NetworkBehaviour
{
    #region Serialized Variables
    [Header("General")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float regStamAmount = 4f;
    [SerializeField] private float redStamAmount = 2;
    [SerializeField] private float regStamTime = 0.2f;
    [SerializeField] private float reduceStaminaTime = 0.2f;
    [SerializeField] private float timeToStartRegenerateStamina = 3f;
    #endregion
    #region Private Variables
    private float timerToStartRegenerateStamina;
    private float timerToReduceStamina;
    private float timerRegenerateStamina;
    #endregion
    #region Public Properties
    public float CurrentStamina { get { return currentStamina; } }
    #endregion
    #region Actions
    public Action OnStaminaChanged;
    #endregion
    #region Functions
    private void Start()
    {
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) { enabled = false; return; }
        currentStamina = maxStamina;
        timerToStartRegenerateStamina = timeToStartRegenerateStamina;
    }

    private void Update()
    {
        HandleStaminaRegen();
    }

    public void UpdateStamina()
    {
        if (timerToReduceStamina <= 0f)
        {
            currentStamina -= redStamAmount;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            OnStaminaChanged?.Invoke();
            timerToReduceStamina = reduceStaminaTime;
        }
        else
        {
            timerToReduceStamina -= Time.deltaTime;
        }
        timerToStartRegenerateStamina = timeToStartRegenerateStamina;
    }

    public void HandleStaminaRegen()
    {
        if (currentStamina < maxStamina)
        {
            if (timerToStartRegenerateStamina > 0f)
            {
                timerToStartRegenerateStamina -= Time.deltaTime;
            }
            else
            {
                if (timerRegenerateStamina <= 0f)
                {
                    timerRegenerateStamina = regStamTime;
                    currentStamina += regStamAmount;
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                    OnStaminaChanged?.Invoke();
                }
                else
                {
                    timerRegenerateStamina -= Time.deltaTime;
                }
            }
        }
    }

    public void SetMaxStamina()
    {
        currentStamina = maxStamina;
        timerToReduceStamina = 0;
        timerRegenerateStamina = 0;
        OnStaminaChanged?.Invoke();
    }
    #endregion
}
