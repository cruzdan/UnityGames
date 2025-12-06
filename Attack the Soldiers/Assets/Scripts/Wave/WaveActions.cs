using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveActions : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private WavesUI wavesUI;
    [SerializeField] private Timer timer;
    #endregion
    #region Functions
    private void Awake()
    {
        waveManager.OnStartWaves += HandleStartWaves;
        waveManager.OnWaveCompleted += HandleWaveCompleted;
        waveManager.OnAllWavesCompleted += HandleAllWavesCompleted;
        timer.OnIntNumberChanged += SetRemainingTimeText;
        timer.OnTimerCompleted += HideMenus;
        wavesUI.ReturnButton.onClick.AddListener(OnReturnButtonPressed);
    }

    private void HandleStartWaves()
    {
        wavesUI.ShowPanel("StartingWaves");
        Invoke(nameof(HideMenus), 3);
    }

    void HideMenus()
    {
        wavesUI.HideAllPanels();
    }

    private void HandleWaveCompleted(int waveNumber)
    {
        if (GameNetwork.Instance.IsOnline)
            HandleWaveCompletedClientRpc(waveNumber);
        else
            OnWaveCompleted(waveNumber);
    }

    [ClientRpc]
    void HandleWaveCompletedClientRpc(int waveNumber, ClientRpcParams clientRpcParams = default)
    {
        OnWaveCompleted(waveNumber);
    }

    void OnWaveCompleted(int waveNumber)
    {
        wavesUI.ShowPanel("WaveCompleted");
        waveManager.CurrentWave = waveNumber;
        timer.StartTimer(waveManager.WaveDataSO.TimeBetweenWaves);
        SetRemainingTimeText(waveManager.WaveDataSO.TimeBetweenWaves);
    }

    private void HandleAllWavesCompleted()
    {
        if (GameNetwork.Instance.IsOnline)
            HandleAllWavesCompletedClientRpc();
        else
            OnHandleAllWavesCompleted();
    }

    [ClientRpc]
    void HandleAllWavesCompletedClientRpc(ClientRpcParams clientRpcParams = default)
    {
        OnHandleAllWavesCompleted();
    }

    void OnHandleAllWavesCompleted()
    {
        wavesUI.ShowPanel("TotalWavesCompleted");
        SetAllWavesCompletedText(100);
    }

    void SetRemainingTimeText(int time)
    {
        wavesUI.WaveCompletedText.text = "Wave " + waveManager.CurrentWave + " completed\nStarting next in " + time;
    }

    void SetAllWavesCompletedText(int points)
    {
        wavesUI.AllWavesCompletedText.text = "Waves completed\nTotal Points:\n" + points;
    }

    void OnReturnButtonPressed()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(0);
    }
    #endregion
}
