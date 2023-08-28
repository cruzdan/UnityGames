using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelectButtons : MonoBehaviour
{
    [SerializeField] private ShopCategory _shopCategory;
    [SerializeField] private CategoryObjectsChanger _categoryObjectsChanger;
    [SerializeField] private CategoryObjectsChanger.CategoryName _categoryName;

    public void InitCategorySelectButtons()
    {
        AddOnSelectButtonPressedToAllCategorySelectButtons();
        _shopCategory.TryToClickOnSelectButton();
    }

    void AddOnSelectButtonPressedToAllCategorySelectButtons()
    {
        Button[] selectButtons;
        selectButtons = _shopCategory.GetCategorySelectButtons();
        int total = selectButtons.Length;
        for(int i = 0; i < total; i++)
        {
            int index = i;
            selectButtons[i].onClick.AddListener(() => { OnSelectButtonPressed(index); });
        }
    }

    public void OnSelectButtonPressed(int selectIndex)
    {
        _categoryObjectsChanger.ChangeCurrentCategorySelectedIndex(_categoryName, selectIndex);
    }
}
