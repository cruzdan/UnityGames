using TMPro;
using UnityEngine;


public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private void OnEnable()
    {
        Score.OnScoreChanged += SetScoreText;
    }

    void SetScoreText(int score)
    {
        _scoreText.SetText(score.ToString());
    }
}
