using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWithUIMobileElements : MonoBehaviour
{
    [SerializeField] private UIMobileElement[] _elements;

    public void QuitElements()
    {
        int total = _elements.Length;
        for (int i = 0; i < total; i++)
        {
            _elements[i].MoveOut();
        }
    }

    public void EnterElements()
    {
        int total = _elements.Length;
        for (int i = 0; i < total; i++)
        {
            _elements[i].MoveIn();
        }
    }

    public void ActiveElements()
    {
        int total = _elements.Length;
        for (int i = 0; i < total; i++)
        {
            _elements[i].gameObject.SetActive(true);
        }
    }
}
