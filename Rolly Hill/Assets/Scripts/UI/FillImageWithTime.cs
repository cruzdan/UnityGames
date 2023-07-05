using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillImageWithTime : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxPercentage;
    private float _currentPercentage;
    private float _lerpObjetive;

    public void Init()
    {
        _currentPercentage = 0;
        _lerpObjetive = _maxPercentage + 10;
        _slider.value = _currentPercentage;
    }

    void Update()
    {
        _currentPercentage = Mathf.Lerp(_currentPercentage, _lerpObjetive, _speed * Time.deltaTime);
        if (HasReachedMaxPercentage())
        {
            enabled = false;
            _currentPercentage = _maxPercentage;
        }
        _slider.value = _currentPercentage;
    }

    bool HasReachedMaxPercentage()
    {
        return _currentPercentage >= _maxPercentage;
    }

    public void SetMaxPercentage(float percentage)
    {
        _maxPercentage = percentage;
    }
}
