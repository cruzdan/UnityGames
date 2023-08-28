using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private UIMobileElement[] _gameMenuElements;
    [SerializeField] private MenuWithUIMobileElements _endMenuElements;
    [SerializeField] private MenuWithUIMobileElements _beginMenuElements;

    public void ActiveEndGameMenu()
    {
        _endMenuElements.ActiveElements();
        _endMenuElements.EnterElements();
    }

    public void ActiveBeginGameMenu()
    {
        _beginMenuElements.ActiveElements();
        _beginMenuElements.EnterElements();
    }

    public void QuitGameMenuElements()
    {
        int total = _gameMenuElements.Length;
        for(int i = 0; i < total; i++)
        {
            _gameMenuElements[i].MoveOut();
        }
    }
}
