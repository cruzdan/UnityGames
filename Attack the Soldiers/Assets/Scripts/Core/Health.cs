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
    [SerializeField] private int initialLife = 100;
    [SerializeField] private float currentLife;
    #endregion
    #region Defense
    [Header("Defense")]
    [SerializeField] private int maxDefense;
    [SerializeField] private int defense;
    #endregion
    #region Override
    [Header("Override")]
    [SerializeField] private bool overrideInvincibility = false;
    #endregion
    #region Public Properties
    public float CurrentLife { get { return currentLife; } set { currentLife = value; } }
    public bool OverrideInvincibility { get { return overrideInvincibility; } set { overrideInvincibility = value; } }
    public int InitialLife { get { return initialLife; } }
    #endregion
    #region Functions
    public void InitializeHealth()
    {
        currentLife = initialLife;
        defense = maxDefense;
        OnLifeChange?.Invoke(currentLife);
    }

    public void TakeDamage(float damage)
    {
        if (overrideInvincibility)
            return;
        TakeDamageAfterDefense(damage);
        DieIfPossible();
    }

    void TakeDamageAfterDefense(float damage)
    {
        float damageAfterDefense = GetDamageAfterDefense(damage);
        currentLife -= damageAfterDefense;
        OnLifeChange?.Invoke(currentLife);
    }

    float GetDamageAfterDefense(float damage)
    {
        float damageAfterDefense = damage - defense;
        if (damageAfterDefense < 0)
            damageAfterDefense = 0;
        return damageAfterDefense;
    }

    void DieIfPossible()
    {
        if (currentLife <= 0)
            OnDie?.Invoke();
    }

    public void Die()
    {
        currentLife = 0;
        TakeDamage(initialLife);
    }
    #endregion
}
