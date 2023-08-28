using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirerateUpgradeFromPlayerPrefs : MonoBehaviour
{
    [SerializeField] private float _decrementPercentage;
    [SerializeField] private float _totalTimeIn100Percentage;
    [SerializeField] private Bullet _bullet;

    private int _maxReached;
    [SerializeField] private float _minFirerate;
    [SerializeField] private Button _buyFirerateButton;

    private void Awake()
    {
        InitFirerateFromPlayerPrefs();
        InitMaxFirerateReachedFromPlayerPrefs();
        SetBuyFirerateButtonNotInteractableIfMaxReached();
    }

    void InitFirerateFromPlayerPrefs()
    {
        SetBulletFirerate(PlayerPrefs.GetFloat("Firerate", .5f));
    }

    void SetBulletFirerate(float firerate)
    {
        _bullet.SetTimeToShoot(firerate);
    } 

    void InitMaxFirerateReachedFromPlayerPrefs()
    {
        _maxReached = PlayerPrefs.GetInt("MaxFirerateReached", 0);
    }

    void SetBuyFirerateButtonNotInteractableIfMaxReached()
    {
        if (_maxReached == 1)
        {
            _buyFirerateButton.interactable = false;
        }
    }

    public void DecrementTimeToShootInPercentage()
    {
        _bullet.SetTimeToShoot(_bullet.GetTimeToShoot() - GetTimeInPercentage());
        SaveMaxFirerateIfItIsInBestValue();
        SaveFirerate();
    }

    void SaveMaxFirerateIfItIsInBestValue()
    {
        if (_bullet.GetTimeToShoot() <= _minFirerate)
        {
            _maxReached = 1;
            SaveMaxFirerateReached();
        }
    }


    void SaveMaxFirerateReached()
    {
        PlayerPrefs.SetInt("MaxFirerateReached", _maxReached);
        PlayerPrefs.Save();
    }

    public void SaveFirerate()
    {
        PlayerPrefs.SetFloat("Firerate", _bullet.GetTimeToShoot());
        PlayerPrefs.Save();
    }

    float GetTimeInPercentage()
    {
        return (_totalTimeIn100Percentage * _decrementPercentage / 100f);
    }
}