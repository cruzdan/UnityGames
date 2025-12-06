using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WavesUI : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private TextMeshProUGUI waveCompletedText;
    [SerializeField] private TextMeshProUGUI allWavesCompletedText;
    [SerializeField] private Button returnButton;
    [SerializeField] private List<MenuID> menusWithID = new List<MenuID>();
    [SerializeField] private List<string> panelIDs = new List<string> { "StartingWaves", "WaveCompleted", "TotalWavesCompleted" };
    #endregion
    #region Private Variables
    private Dictionary<string, MenuID> panelsDictionary = new Dictionary<string, MenuID>();
    #endregion
    #region Public Properties
    public TextMeshProUGUI WaveCompletedText => waveCompletedText;
    public TextMeshProUGUI AllWavesCompletedText => allWavesCompletedText;
    public Button ReturnButton => returnButton;
    #endregion
    #region Functions
    private void Awake()
    {
        panelsDictionary.InitializeFromList(menusWithID, x => x.ID);
        
    }

    public void ShowPanel(string id)
    {
        HideAllPanels();
        panelsDictionary[id].menuObject.SetActive(true);
    }

    public void HideAllPanels()
    {
        foreach (var panelID in panelIDs)
        {
            if (panelsDictionary.TryGetValue(panelID, out var menu))
            {
                menu.menuObject.SetActive(false);
            }
        }
    }
    #endregion
}
