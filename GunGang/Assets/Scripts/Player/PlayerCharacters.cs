using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacters : MonoBehaviour
{
    [SerializeField] private Transform _charactersParent;
    private List<Shoot> _characterShoots = new();

    public void ReturnCharactersToPool()
    {
        GameObject _auxiliarObject;
        int total = _charactersParent.childCount;
        for (int i = total - 1; i >= 0; i--)
        {
            _auxiliarObject = _charactersParent.GetChild(i).gameObject;
            ObjectPool.Instance.ReturnObjectToPoolInPoolParent(_auxiliarObject,
                _auxiliarObject.GetComponent<DeleteMapObject>().GetPoolObjectType());
        }
    }

    public void EnableSavedChildCharacterShoots(bool value)
    {
        foreach (var shoot in _characterShoots)
        {
            shoot.enabled = value;
        }
    }

    public void ClearSavedChildCharacterShoots()
    {
        _characterShoots.Clear();
    }

    public void DisableAndSaveChildCharacterShoots()
    {
        SaveChildCharacterShoots();
        EnableSavedChildCharacterShoots(false);
    }

    void SaveChildCharacterShoots()
    {
        int total = _charactersParent.childCount;
        for (int i = total - 1; i >= 0; i--)
        {
            _characterShoots.Add(_charactersParent.GetChild(i).GetComponent<Shoot>());
        }
    }
}
