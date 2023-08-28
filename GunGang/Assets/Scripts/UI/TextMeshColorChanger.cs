using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshColorChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _UIText;
    [SerializeField] private float _speed;
    private VertexGradient _vertexGradient;
    const int TotalColors = 2;
    private Color[] _colors;
    private bool[] _isIncrementingColor;
    int i;

    void Start()
    {
        AssignUIGradientAsVertexGradient();
        InitColors();
    }

    void AssignUIGradientAsVertexGradient()
    {
        _vertexGradient = _UIText.colorGradient;
    }

    void InitColors()
    {
        _colors = new Color[TotalColors];
        _colors[0] = _vertexGradient.bottomLeft;
        _colors[1] = _vertexGradient.topLeft;
        _isIncrementingColor = new bool[TotalColors];
    }

    void Update()
    {
        for(i = 0; i < TotalColors; i++)
        {
            ChangeColor(i);
        }
        AssignColorsAsVertexGradientColors();
        AssignVertexGradientAsUITextColors();
    }

    void ChangeColor(int colorIndex)
    {
        if (_isIncrementingColor[colorIndex])
        {
            IncrementRedColor(colorIndex);
            if (HasReachedMaxRedColor(colorIndex))
            {
                OnReachedMaxRedColor(colorIndex);
            }
        }
        else
        {
            DecrementRedColor(colorIndex);
            if (HasReachedMinRedColor(colorIndex))
            {
                OnReachedMinRedColor(colorIndex);
            }
        }
    }

    void IncrementRedColor(int colorIndex)
    {
        _colors[colorIndex].r += _speed * Time.deltaTime;
    }

    bool HasReachedMaxRedColor(int colorIndex)
    {
        return _colors[colorIndex].r >= 1;
    }

    void OnReachedMaxRedColor(int colorIndex)
    {
        _isIncrementingColor[colorIndex] = false;
        _colors[colorIndex].r = 1;
    }

    void DecrementRedColor(int colorIndex)
    {
        _colors[colorIndex].r -= _speed * Time.deltaTime;
    }

    bool HasReachedMinRedColor(int colorIndex)
    {
        return _colors[colorIndex].r <= 0;
    }

    void OnReachedMinRedColor(int colorIndex)
    {
        _isIncrementingColor[colorIndex] = true;
        _colors[colorIndex].r = 0;
    }

    void AssignColorsAsVertexGradientColors()
    {
        _vertexGradient.bottomLeft = _colors[0];
        _vertexGradient.topLeft = _colors[1];
    }

    void AssignVertexGradientAsUITextColors()
    {
        _UIText.colorGradient = _vertexGradient;
    }
}
