using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    private void Start()
    {
        Map.OnLevelChanged += IncrementLevelText;
    }

    private void OnDestroy()
    {
        Map.OnLevelChanged -= IncrementLevelText;
    }

    void IncrementLevelText(int level)
    {
        _levelText.text = level.ToString();
    }
}
