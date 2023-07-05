using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseElement : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] _elements;
    bool[] _isOnPause;

    void Start()
    {
        Pause.OnPause += DisableElementsIfNeeded;
        Pause.OnContinue += EnableElementsIfNeeded;
        _isOnPause = new bool[_elements.Length];
    }

    private void OnDestroy()
    {
        Pause.OnPause -= DisableElementsIfNeeded;
        Pause.OnContinue -= EnableElementsIfNeeded;
    }

    void DisableElementsIfNeeded()
    {
        int total = _elements.Length;
        for(int i = 0; i < total; i++)
        {
            DisableElementIfNeeded(i);
        }
    }

    void DisableElementIfNeeded(int elementIndex)
    {
        if (_elements[elementIndex].enabled)
        {
            DisableElement(elementIndex);
            _isOnPause[elementIndex] = true;
        }
    }

    void DisableElement(int elementIndex)
    {
        _elements[elementIndex].enabled = false;
    }

    void EnableElementsIfNeeded()
    {
        int total = _elements.Length;
        for (int i = 0; i < total; i++)
        {
            EnableElementIfNeeded(i);
        }
    }

    void EnableElementIfNeeded(int elementIndex)
    {
        if (_isOnPause[elementIndex])
        {
            EnableElement(elementIndex);
            _isOnPause[elementIndex] = false;
        }
    }

    void EnableElement(int elementIndex)
    {
        _elements[elementIndex].enabled = true;
    }
}
