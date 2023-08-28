using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Start()
    {
        Score.OnScoreChanged += UpdateScoreBar;
        Map.OnTotalBlocksChanged += SetScoreBarMaxValue;
    }

    private void OnDestroy()
    {
        Score.OnScoreChanged -= UpdateScoreBar;
        Map.OnTotalBlocksChanged -= SetScoreBarMaxValue;
    }

    public void UpdateScoreBar(int value)
    {
        _slider.value = value;
    }

    void SetScoreBarMaxValue(int value)
    {
        _slider.maxValue = value;
    }

    public void ResetScoreBarValue()
    {
        _slider.value = 0;
    }
}
