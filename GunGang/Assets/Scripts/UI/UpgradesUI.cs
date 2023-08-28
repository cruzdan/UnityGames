using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField] private MoneyFromPlayerPrefs _money;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private int[] _itemPrices;
    [SerializeField] private string[] _itemNames;
    [SerializeField] private DamageUpgradeFromPlayerPrefs _damageUpgrade;
    [SerializeField] private FirerateUpgradeFromPlayerPrefs _firerateUpgrade;

    [SerializeField] private Bullet _bullet;
    [SerializeField] private float _minTimeToShoot;
    [SerializeField] private TextMeshProUGUI _firerateText;

    [SerializeField] private int _priceIncrement;

    private List<TextMeshProUGUI> _itemPriceTexts = new();
    private List<Button> _upgradeBuyButtons = new();

    private void Start()
    {
        FillUpgradeBuyButtons();
        FillUpgradePriceTexts();
        AddOnBuyButtonPressedActionToBuyButtons();
        InitPricesFromPlayerPrefs();
    }

    void FillUpgradeBuyButtons()
    {
        int total = _itemsParent.childCount;
        for(int i = 0; i < total; i++)
        {
            _upgradeBuyButtons.Add(GetItemInfoParent(i).GetChild(1).GetComponent<Button>());
        }
    }

    void FillUpgradePriceTexts()
    {
        int total = _itemsParent.childCount;
        for (int i = 0; i < total; i++)
        {
            _itemPriceTexts.Add(GetItemInfoParent(i).GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }

    Transform GetItemInfoParent(int itemIndex)
    {
        return _itemsParent.GetChild(itemIndex).GetChild(2);
    }

    public void OnBuyButtonPressed(int itemIndex)
    {
        if (!CanBuyUpgrade(itemIndex))
            return;
        DecrementItemPriceToMoney(itemIndex);
        IncrementItemPrice(itemIndex, _priceIncrement);
        UpdatePriceText(itemIndex);
        GetUpgrade(itemIndex);
        SaveMoney();
    }

    bool CanBuyUpgrade(int itemIndex)
    {
        return _money.GetMoney() >= _itemPrices[itemIndex];
    } 

    void DecrementItemPriceToMoney(int itemIndex)
    {
        _money.AddMoney(-_itemPrices[itemIndex]);
    }

    void IncrementItemPrice(int itemIndex, int increment)
    {
        _itemPrices[itemIndex] += increment;
        SaveItemPrice(itemIndex);
    }

    void SaveItemPrice(int itemIndex)
    {
        PlayerPrefs.SetInt(_itemNames[itemIndex] + "Price", _itemPrices[itemIndex]);
        PlayerPrefs.Save();
    }

    void UpdatePriceText(int itemIndex)
    {
        _itemPriceTexts[itemIndex].text = _itemPrices[itemIndex].ToString();
    }

    void AddOnBuyButtonPressedActionToBuyButtons()
    {
        int total = _upgradeBuyButtons.Count;
        for(int i = 0; i < total; i++)
        {
            int buttonIndex = i;
            _upgradeBuyButtons[i].onClick.AddListener(() => { OnBuyButtonPressed(buttonIndex); });
        }
    }

    void InitPricesFromPlayerPrefs()
    {
        for(int i = 0; i < _itemPrices.Length; i++)
        {
            _itemPrices[i] = PlayerPrefs.GetInt(_itemNames[i] + "Price", 20);
            UpdatePriceText(i);
        }
    }

    void GetUpgrade(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0:
                _damageUpgrade.IncrementDamage();
                break;
            case 1:
                _firerateUpgrade.DecrementTimeToShootInPercentage();
                DisableFireRateBuyButtonIfMaxLimitIsReached(itemIndex);
                break;
        }
    }

    void DisableFireRateBuyButtonIfMaxLimitIsReached(int itemIndex)
    {
        if(_bullet.GetTimeToShoot() <= _minTimeToShoot)
        {
            _upgradeBuyButtons[itemIndex].interactable = false;
            _firerateText.text = "Max";
        }
    }

    void SaveMoney()
    {
        _money.SaveMoney();
    }
}
