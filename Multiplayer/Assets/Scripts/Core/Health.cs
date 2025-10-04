using UnityEngine;
using System;

[Serializable]
public class Health
{
    #region Events
    public Action OnDie;
    public Action<float> OnLifeChange;
    #endregion
    #region Life
    [Header("Life")]
    [SerializeField] private int maxLife = 100;
    [SerializeField] private float currentLife;
    #endregion
    #region Defense
    [Header("Defense")]
    [SerializeField] private int maxDefense;
    [SerializeField] private int defense;
    #endregion
    #region Burn
    [Header("Burn")]
    [SerializeField] private bool canBurn = true;
    #endregion
    #region Override
    [Header("Override")]
    [SerializeField] private bool overrideInvincibility = false;
    #endregion
    #region Public Properties
    public float CurrentLife { get { return currentLife; } }
    public bool CanBurn { get { return canBurn; } set => canBurn = value; }
    #endregion
    #region Functions
    public void InitializeHealth()
    {
        currentLife = maxLife;
        defense = maxDefense;
        OnLifeChange?.Invoke(currentLife);
    }

    public void TakeDamage(float damage)
    {
        if (overrideInvincibility)
            return;
        float damageAfterDefense = damage - defense;
        if (damageAfterDefense < 0)
            damageAfterDefense = 0;
        currentLife -= damageAfterDefense;
        OnLifeChange?.Invoke(currentLife);
        if (currentLife <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void Die()
    {
        currentLife = 0;
        TakeDamage(maxLife);
    }
    #endregion
}
