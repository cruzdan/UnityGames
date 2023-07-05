using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private UIMobileElement[] _gameMenuElements;
    [SerializeField] private MenuWithUIMobileElements _endMenuElements;
    [SerializeField] private MenuWithUIMobileElements _beginMenuElements;

    private void OnEnable()
    {
        Firework.OnFireworksEnd += QuitGameMenuElements;
        Firework.OnFireworksEnd += ActiveEndGameMenu;
        LoosingPhase.OnLoose += ActiveBeginGameMenu;
    }

    private void OnDisable()
    {
        Firework.OnFireworksEnd -= QuitGameMenuElements;
        Firework.OnFireworksEnd -= ActiveEndGameMenu;
        LoosingPhase.OnLoose -= ActiveBeginGameMenu;
    }

    void ActiveEndGameMenu()
    {
        _endMenuElements.ActiveElements();
        _endMenuElements.EnterElements();
    }

    void ActiveBeginGameMenu()
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
