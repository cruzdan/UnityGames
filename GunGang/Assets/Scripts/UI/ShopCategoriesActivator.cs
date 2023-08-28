using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCategoriesActivator : MonoBehaviour
{
    [SerializeField] private ShopCategory[] _shopCategories;
    [SerializeField] private CategorySelectButtons[] _categorySelectButtons;
    [SerializeField] private ShopUI _shopUI;
    [SerializeField] private CategoryObjectsChanger _categoryObjectsChanger;


    void Start()
    {
        foreach(var shopCategory in _shopCategories)
        {
            shopCategory.InitShopCategory();
        }
        foreach (var categorySelectButtons in _categorySelectButtons)
        {
            categorySelectButtons.InitCategorySelectButtons();
        }
        _shopUI.OnContinueButtonPressed();
        _categoryObjectsChanger.ChangeObjectsIfAreNowSelectedInShop();
    }
}
