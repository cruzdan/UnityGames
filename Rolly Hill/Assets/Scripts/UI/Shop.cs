using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private string _itemsName;
    [SerializeField] private bool[] _itemsGetted;
    [SerializeField] private Button[] _shopBuyButtons;
    [SerializeField] private Button[] _shopSelectButtons;
    [SerializeField] private TextMeshProUGUI[] _shopPriceTexts;
    [SerializeField] private int[] _itemPrices;
    [SerializeField] private int _selectedItemIndex;
    [SerializeField] private Diamond _diamond;

    public void InitShopItems()
    {
        InitPlayerPrefsSelectedIndex();
        SetPlayerPrefsItemsGetted();
        InitItemButtons();
    }

    void InitItemButtons()
    {
        int total = _shopBuyButtons.Length;
        for(int i = 0; i < total; i++)
        {
            if (ItemHasBeenPurchased(i))
            {
                SetInteractableBuyButtonOfItem(i, false);
                SetInteractableSelectButtonOfItem(i, true);
            }
            else
            {
                SetInteractableSelectButtonOfItem(i, false);
            }
            SetItemPrice(i);
        }
        SetInteractableSelectButtonOfItem(_selectedItemIndex, false);
    }

    bool ItemHasBeenPurchased(int itemIndex)
    {
        return _itemsGetted[itemIndex];
    }

    void SetInteractableBuyButtonOfItem(int itemIndex, bool value)
    {
        _shopBuyButtons[itemIndex].interactable = value;
    }

    void SetInteractableSelectButtonOfItem(int itemIndex, bool value)
    {
        _shopSelectButtons[itemIndex].interactable = value;
    }

    void SetItemPrice(int itemIndex)
    {
        _shopPriceTexts[itemIndex].text = "$" + _itemPrices[itemIndex].ToString();
    }

    public void OnBuyPressedWithItemIndex(int itemIndex)
    {
        if (!CanPurchaseItem(itemIndex))
            return;
        SetInteractableBuyButtonOfItem(itemIndex, false);
        SetInteractableSelectButtonOfItem(itemIndex, true);
        SaveItemBoughtInPlayerPrefs(itemIndex);
        _diamond.AddDiamonds(-_itemPrices[itemIndex]);
        _diamond.SaveDiamonds();
    }

    bool CanPurchaseItem(int itemIndex)
    {
        if(_diamond.GetTotalDiamonds() >= _itemPrices[itemIndex])
            return true;
        return false;
    }

    void SaveItemBoughtInPlayerPrefs(int itemIndex)
    {
        PlayerPrefs.SetInt(_itemsName + itemIndex, 1);
        PlayerPrefs.Save();
    }

    public void OnSelectPressedWithItemIndex(int itemIndex)
    {
        SetInteractableSelectButtonOfItem(itemIndex, false);
        SetInteractableSelectButtonOfItem(_selectedItemIndex, true);
        _selectedItemIndex = itemIndex;
        SaveSelectedIndexInPlayerPrefs(_selectedItemIndex);
    }

    void SetPlayerPrefsItemsGetted()
    {
        int total = _itemsGetted.Length;
        for (int i = 1; i < total; i++)
        {
            SetPlayerPrefsItemGettedValue(i);
        }
    }

    void SetPlayerPrefsItemGettedValue(int itemIndex)
    {
        if (PlayerPrefs.GetInt(_itemsName + itemIndex, 0) == 0)
        {
            _itemsGetted[itemIndex] = false;
        }
        else
        {
            _itemsGetted[itemIndex] = true;
        }
    }

    void SaveSelectedIndexInPlayerPrefs(int selectedIndex)
    {
        PlayerPrefs.SetInt(_itemsName + "SelectedIndex", selectedIndex);
        PlayerPrefs.Save();
    }

    void InitPlayerPrefsSelectedIndex()
    {
        SetPlayerPrefsSelectedIndex();
        TryClickOnSelectButton();
    }

    void SetPlayerPrefsSelectedIndex()
    {
        _selectedItemIndex = PlayerPrefs.GetInt(_itemsName + "SelectedIndex", 0);
    }

    void TryClickOnSelectButton()
    {
        if (_selectedItemIndex == 0)
            return;
        _shopSelectButtons[_selectedItemIndex].onClick.Invoke();
    }
}
