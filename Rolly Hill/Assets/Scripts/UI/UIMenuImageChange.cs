using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuImageChange : MonoBehaviour
{
    [SerializeField] private Image _beginMenuImage;
    [SerializeField] private Image _gameMenuImage;
    [SerializeField] private Image _endMenuImage;
    [SerializeField] private Image _cardMenuImage;

    public void PassFromBeginToGame()
    {
        _beginMenuImage.enabled = false;
        _gameMenuImage.enabled = true;
    }

    public void PassFromGameToEnd()
    {
        _gameMenuImage.enabled = false;
        _endMenuImage.enabled = true;
    }

    public void PassFromEndToCard()
    {
        _endMenuImage.enabled = false;
        _cardMenuImage.enabled = true;
    }

    public void PassFromEndToBegin()
    {
        _endMenuImage.enabled = false;
        _beginMenuImage.enabled = true;
    }

    public void PassFromGameToBegin()
    {
        _gameMenuImage.enabled = false;
        _beginMenuImage.enabled = true;
    }
}
