using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextWhenChangeMoney : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        MoneyFromPlayerPrefs.OnMoneyChange += ChangeTextForInt;
    }

    void ChangeTextForInt(int value)
    {
        _text.text = value.ToString();
    }
}
