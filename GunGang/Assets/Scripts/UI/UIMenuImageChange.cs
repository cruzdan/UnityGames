using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuImageChange : MonoBehaviour
{
    [SerializeField] private Image _beginMenuImage;
    [SerializeField] private Image _gameMenuImage;

    public void PassFromBeginToGame()
    {
        _beginMenuImage.enabled = false;
        _gameMenuImage.enabled = true;
    }

    public void PassFromGameToBegin()
    {
        _gameMenuImage.enabled = false;
        _beginMenuImage.enabled = true;
    }
}
