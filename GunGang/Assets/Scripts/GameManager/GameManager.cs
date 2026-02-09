using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameEvent OnPassLevel;
    [SerializeField] private GameEvent OnRestartLevel;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerCharacterParent;
    [SerializeField] private Bounce[] scoreBounces;
    [SerializeField] private Bounce[] moneyBounces;

    public Bounce[] ScoreBounces => scoreBounces;
    public Bounce[] MoneyBounces => moneyBounces;

    private void Start()
    {
        Application.targetFrameRate = 30;
        MusicManager.Instance.PlayRandomMusicWithoutRepetition();
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    public Transform GetPlayerCharacterParent()
    {
        return _playerCharacterParent;
    }

    public void PassLevel()
    {
        OnPassLevel.TriggerEvent();
    }

    public void RestartLevel()
    {
        OnRestartLevel.TriggerEvent();
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void ContinueTime()
    {
        Time.timeScale = 1;
    }

    public void PassLevelShake()
    {
        CameraShake.Instance.Shake(0.3f, 0.5f);
    }
    public void StartArrayBounce(Bounce[] bounces)
    {
        foreach (var bounce in bounces)
        {
            bounce.StartBounce();
        }
    }

    public void PlayWinAudio()
    {
        SFXManager.Instance.PlaySFX(AudioConstants.Instance.WinClip);
    }

    public void PlayButtonAudio()
    {
        SFXManager.Instance.PlaySFX(AudioConstants.Instance.buttonClickClip);
    }
}
