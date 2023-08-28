using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private int _RGBAColorIndex;
    [SerializeField] private float _paintingSpeed;
    [SerializeField] private MeshRenderer _meshRenderer;
    private Color _currentColor;
    private bool _paintingDown = true;

    private void Start()
    {
        _currentColor = _meshRenderer.material.color;
    }

    private void Update()
    {
        if (_paintingDown)
        {
            if (SelectedColorIsGreaterThanZero())
            {
                AssignSameColorsOnCurrentColor();
                DecrementSelectedColor();
            }
            else
            {
                _paintingDown = false;
            }
        }
        else
        {
            if (SelectedColorIsLessThanOne())
            {
                AssignSameColorsOnCurrentColor();
                IncrementSelectedColor();
            }
            else
            {
                _paintingDown = true;
            }
        }
    }

    bool SelectedColorIsGreaterThanZero()
    {
        return _meshRenderer.material.color[_RGBAColorIndex] > 0;
    }

    bool SelectedColorIsLessThanOne()
    {
        return _meshRenderer.material.color[_RGBAColorIndex] < 1;
    }

    void AssignSameColorsOnCurrentColor()
    {
        _currentColor = _meshRenderer.material.color;
    }

    void IncrementSelectedColor()
    {
        _currentColor[_RGBAColorIndex] += _paintingSpeed * Time.deltaTime;
        ClampAndAssignCurrentColorAsMeshColor();
    }

    void DecrementSelectedColor()
    {
        _currentColor[_RGBAColorIndex] -= _paintingSpeed * Time.deltaTime;
        ClampAndAssignCurrentColorAsMeshColor();
    }

    void ClampAndAssignCurrentColorAsMeshColor()
    {
        _currentColor[_RGBAColorIndex] = Mathf.Clamp01(_currentColor[_RGBAColorIndex]);
        _meshRenderer.material.color = _currentColor;
    }
}
