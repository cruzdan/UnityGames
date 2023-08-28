using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyTextOnUI : MonoBehaviour
{
    [SerializeField] private MoneyFromPlayerPrefs _money;
    [SerializeField] private TextMeshProUGUI _moneyText;

    public void Start()
    {
        SetMoneyNumberOnUI();
    }

    public void SetMoneyNumberOnUI()
    {
        _moneyText.text = _money.GetMoney().ToString();
    }
}
