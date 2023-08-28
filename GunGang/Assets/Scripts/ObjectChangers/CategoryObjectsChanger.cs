using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CategoryObjectsChanger : MonoBehaviour
{
    [SerializeField] private int[] _originalCategorySelectedIndex;
    [SerializeField] private int[] _currentCategorySelectedIndex;
    [SerializeField] private BulletChanger _bulletChanger;
    [SerializeField] private CylinderChanger _cylinderChanger;
    [SerializeField] private CharacterChanger _characterChanger;
    [SerializeField] private GunChanger _gunChanger;

    public enum CategoryName
    {
        Bullet = 0,
        Cylinder = 1,
        Character = 2,
        Gun = 3
    }

    public void ChangeCurrentCategorySelectedIndex(CategoryName categoryName, int selectIndex)
    {
        _currentCategorySelectedIndex[(int)categoryName] = selectIndex;
    }

    public void ChangeObjectsIfAreNowSelectedInShop()
    {
        ChangeFirstObjectsIfAreNowSelectedInShop(2);
        if (ChangeCharacterObjectsIfAreNowSelectedInShop())
            return;
        ChangeGunObjectsIfAreNowSelectedInShop();
    }

    void ChangeFirstObjectsIfAreNowSelectedInShop(int firstNumber)
    {
        for (int i = 0; i < firstNumber; i++)
        {
            if (CurrentCategorySelectedObjectHasChanged(i))
            {
                UpdateCategoryObjects(i);
            }
        }
    }

    void UpdateCategoryObjects(int categoryIndex)
    {
        ChangeCategoryObjects(categoryIndex, _currentCategorySelectedIndex[categoryIndex]);
        _originalCategorySelectedIndex[categoryIndex] = _currentCategorySelectedIndex[categoryIndex];
    }

    bool ChangeCharacterObjectsIfAreNowSelectedInShop()
    {
        if (CurrentCategorySelectedObjectHasChanged(2))
        {
            UpdateCategoryObjects(2);
            if (!GunIsTheFirstOne())
            {
                UpdateCategoryObjects(3);
                return true;
            }
        }
        return false;
    }

    bool GunIsTheFirstOne()
    {
        return _currentCategorySelectedIndex[3] == 0;
    }

    void ChangeGunObjectsIfAreNowSelectedInShop()
    {
        if (CurrentCategorySelectedObjectHasChanged(3))
        {
            UpdateCategoryObjects(3);
        }
    }

    bool CurrentCategorySelectedObjectHasChanged(int categoryIndex)
    {
        return _originalCategorySelectedIndex[categoryIndex] != _currentCategorySelectedIndex[categoryIndex];
    }

    void ChangeCategoryObjects(int categoryIndex, int prefabIndex)
    {
        switch (categoryIndex)
        {
            case 0:
                _bulletChanger.ChangeAllBulletsForPrefabWithIndex(prefabIndex);
                break;
            case 1:
                _cylinderChanger.ChangeAllCylindersForPrefabWithIndex(prefabIndex);
                break;
            case 2:
                _characterChanger.ChangeAllCharactersForPrefabWithIndex(prefabIndex);
                break;
            case 3:
                _gunChanger.ChangeAllCharacterGuns(prefabIndex);
                break;
        }
    }
}
