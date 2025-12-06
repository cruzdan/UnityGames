using UnityEngine;
using TMPro;

public class UIScoreDisplay : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private TMP_Text text;
    #endregion
    #region Private Variables
    private Player player;
    private PlayerScore playerScore;
    #endregion
    #region Public Properties
    public Player Player { get { return player; } }
    #endregion
    #region Functions
    public void Initialize(Player player)
    {
        this.player = player;
        playerScore = player.GetComponent<PlayerScore>();

        playerScore.Score.OnValueChanged += OnScoreChanged;
        OnScoreChanged(0, playerScore.Score.Value);
    }

    private void OnScoreChanged(int previous, int current)
    {
        text.text = "Score: " + current;
    }
    #endregion
}

