using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderBonusFromPlayerPrefs : MonoBehaviour
{
    [SerializeField] private float _cylinderOriginalPoints;
    private float _bonusInPercentage;

    public static int BonusPoints;

    void Start()
    {
        _bonusInPercentage = PlayerPrefs.GetFloat("BonusPercentage", 0);
        CalculateBonusPointsWithPercentage();
    }

    void CalculateBonusPointsWithPercentage()
    {
        BonusPoints = (int)(_bonusInPercentage / 100f * _cylinderOriginalPoints);
    }

    public void SetBonusPercentageIfItIsBetter(float percentage)
    {
        if (percentage > _bonusInPercentage)
        {
            _bonusInPercentage = percentage;
            SaveBonusPercentage();
            CalculateBonusPointsWithPercentage();
        }
    }

    void SaveBonusPercentage()
    {
        PlayerPrefs.SetFloat("BonusPercentage", _bonusInPercentage);
        PlayerPrefs.Save();
    }
}
