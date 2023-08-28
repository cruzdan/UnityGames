using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCategory : MonoBehaviour
{
    [SerializeField] private string _categoryName;
    [SerializeField] private bool[] _gettedItems;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private int[] _itemPrices;
    [SerializeField] private int _selectedItemIndex = 0;
    [SerializeField] private MoneyFromPlayerPrefs _money;
    
    private Button[] _shopBuyButtons;
    private Button[] _shopSelectButtons;
    private TextMeshProUGUI[] _shopPriceTexts;

    public void InitShopCategory()
    {
        InitArrays();
        FillShopBuyButtons();
        FillShopSelectButtons();
        FillShopPriceTexts();
        InitShopItems();
    }

    void InitArrays()
    {
        _shopBuyButtons = new Button[_itemsParent.childCount];
        _shopSelectButtons = new Button[_itemsParent.childCount];
        _shopPriceTexts = new TextMeshProUGUI[_itemsParent.childCount];
    }

    void FillShopBuyButtons()
    {
        int total = _itemsParent.childCount;
        for (int i = 0; i < total; i++)
        {
            _shopBuyButtons[i] = GetBuyButtonFromItemObject(i);
        }
    }

    Button GetBuyButtonFromItemObject(int itemIndex)
    {
        return GetItemInfoParent(itemIndex).GetChild(1).GetComponent<Button>();
    }

    Transform GetItemInfoParent(int itemIndex)
    {
        return _itemsParent.GetChild(itemIndex).GetChild(2);
    }

    void FillShopSelectButtons()
    {
        int total = _itemsParent.childCount;
        for (int i = 0; i < total; i++)
        {
            _shopSelectButtons[i] = GetSelectButtonFromItemObject(i);
        }
    }

    Button GetSelectButtonFromItemObject(int itemIndex)
    {
        return GetItemInfoParent(itemIndex).GetChild(2).GetComponent<Button>();
    }

    void FillShopPriceTexts()
    {
        int total = _itemsParent.childCount;
        for (int i = 0; i < total; i++)
        {
            _shopPriceTexts[i] = GetPriceTextFromItemObject(i);
        }
    }

    TextMeshProUGUI GetPriceTextFromItemObject(int itemIndex)
    {
        return GetItemInfoParent(itemIndex).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void InitShopItems()
    {
        InitPlayerPrefsSelectedIndex();
        FillGettedItemsFromPlayerPrefs();
        InitItemButtons();
    }

    void InitPlayerPrefsSelectedIndex()
    {
        SetPlayerPrefsSelectedIndex();
    }

    void SetPlayerPrefsSelectedIndex()
    {
        _selectedItemIndex = PlayerPrefs.GetInt(_categoryName + "SelectedIndex", 0);
    }

    public void TryToClickOnSelectButton()
    {
        if (SelectedItemIsTheFirsItem())
            return;
        _shopSelectButtons[_selectedItemIndex].onClick.Invoke();
    }

    bool SelectedItemIsTheFirsItem()
    {
        return _selectedItemIndex == 0;
    }

    void FillGettedItemsFromPlayerPrefs()
    {
        int total = _gettedItems.Length;
        for (int i = 1; i < total; i++)
        {
            SetGettedItemValueFromPlayerPrefs(i);
        }
    }

    void SetGettedItemValueFromPlayerPrefs(int itemIndex)
    {
        if (PlayerPrefs.GetInt(_categoryName + itemIndex, 0) == 0)
        {
            _gettedItems[itemIndex] = false;
        }
        else
        {
            _gettedItems[itemIndex] = true;
        }
    }

    void InitItemButtons()
    {
        int total = _shopBuyButtons.Length;
        for (int i = 0; i < total; i++)
        {
            InitItemBuyAndSelectInteractableButtons(i);
            AddOnSelectEventToSelectButton(i);
            AddOnBuyEventToBuyButton(i);
        }
        SetInteractableSelectButtonOfItem(_selectedItemIndex, false);
    }

    void InitItemBuyAndSelectInteractableButtons(int itemIndex)
    {
        if (ItemHasBeenPurchased(itemIndex))
        {
            SetInteractableBuyButtonOfItem(itemIndex, false);
            SetInteractableSelectButtonOfItem(itemIndex, true);
        }
        else
        {
            SetInteractableSelectButtonOfItem(itemIndex, false);
        }
        SetItemPrice(itemIndex);
    }

    void AddOnSelectEventToSelectButton(int itemIndex)
    {
        _shopSelectButtons[itemIndex].onClick.AddListener(() => { OnSelectPressedWithItemIndex(itemIndex); });
    }

    void AddOnBuyEventToBuyButton(int itemIndex)
    {
        _shopBuyButtons[itemIndex].onClick.AddListener(() => { OnBuyPressedWithItemIndex(itemIndex); });
    }

    bool ItemHasBeenPurchased(int itemIndex)
    {
        return _gettedItems[itemIndex];
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
        SaveBoughtItemInPlayerPrefs(itemIndex);
        DecrementItemPriceToMoney(itemIndex);
        SaveMoney();
    }

    bool CanPurchaseItem(int itemIndex)
    {
        if (_money.GetMoney() >= _itemPrices[itemIndex])
            return true;
        return false;
    }

    void SaveBoughtItemInPlayerPrefs(int itemIndex)
    {
        PlayerPrefs.SetInt(_categoryName + itemIndex, 1);
        PlayerPrefs.Save();
    }

    void DecrementItemPriceToMoney(int itemIndex)
    {
        _money.AddMoney(-_itemPrices[itemIndex]);
    }

    void SaveMoney()
    {
        _money.SaveMoney();
    }

    public void OnSelectPressedWithItemIndex(int itemIndex)
    {
        SetInteractableSelectButtonOfItem(_selectedItemIndex, true);
        SetInteractableSelectButtonOfItem(itemIndex, false);
        _selectedItemIndex = itemIndex;
        SaveSelectedIndexInPlayerPrefs(_selectedItemIndex);
    }

    void SaveSelectedIndexInPlayerPrefs(int selectedIndex)
    {
        PlayerPrefs.SetInt(_categoryName + "SelectedIndex", selectedIndex);
        PlayerPrefs.Save();
    }

    public Button[] GetCategorySelectButtons()
    {
        return _shopSelectButtons;
    }
}
