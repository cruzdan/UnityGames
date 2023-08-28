using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
    #region Categories
    [Header("Categories")]
    [SerializeField] GameObject[] _categoryPanels;
    [SerializeField] GameObject[] _categoryScrollbars;
    [SerializeField] string[] _categoryTexts;
    #endregion 

    #region Shop Menu
    [Header("Shop Menu")]
    [SerializeField] GameObject _categoryButtonsPanel;
    [SerializeField] GameObject _shopMenu;
    [SerializeField] GameObject _continueButton;
    [SerializeField] GameObject _backButton;
    #endregion

    #region Title
    [Header("Title")]
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshColorChanger _titleColorChanger;
    [SerializeField] private VertexGradient _initialTitleColorGradient;
    #endregion

    private int _currentCategoryIndex;

    public void OnCategoryButtonsPressed(int buttonIndex)
    {
        _categoryButtonsPanel.SetActive(false);
        EnableCategoryPanel(buttonIndex, true);
        _continueButton.SetActive(false);
        _backButton.SetActive(true);
        SaveCategoryIndex(buttonIndex);
        _titleText.text = _categoryTexts[buttonIndex];
        _titleColorChanger.enabled = true;
    }

    void SaveCategoryIndex(int value)
    {
        _currentCategoryIndex = value;
    }

    void EnableCategoryPanel(int panelIndex, bool value)
    {
        _categoryPanels[panelIndex].SetActive(value);
    }

    public void OnContinueButtonPressed()
    {
        _shopMenu.SetActive(false);
        _categoryButtonsPanel.SetActive(true);
        EnableAllCategoryPanels(false);
        EnableAllCategoryScrollbars(false);
    }

    void EnableAllCategoryPanels(bool value)
    {
        int total = _categoryPanels.Length;
        for (int i = 0; i < total; i++)
        {
            _categoryPanels[i].SetActive(value);
        }
    }

    void EnableAllCategoryScrollbars(bool value)
    {
        int total = _categoryScrollbars.Length;
        for (int i = 0; i < total; i++)
        {
            _categoryScrollbars[i].SetActive(value);
        }
    }

    public void OnBackButtonPressed()
    {
        EnableCategoryPanel(_currentCategoryIndex, false);
        EnableCategoryScrollbar(_currentCategoryIndex, false);
        _backButton.SetActive(false);
        _continueButton.SetActive(true);
        _titleText.text = "SHOP";
        _titleColorChanger.enabled = false;
        _titleText.colorGradient = _initialTitleColorGradient;
    }

    void EnableCategoryScrollbar(int categoryIndex, bool value)
    {
        _categoryScrollbars[categoryIndex].SetActive(value);
        _categoryButtonsPanel.SetActive(true);
    }
}
