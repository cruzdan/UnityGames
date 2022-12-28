using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int lifes = 5;
    [SerializeField] private GameObject msPacManUI;
    private readonly float firstPositionX = 3f;

    private float timeToChangeState = 2f;
    private float timer;

    [SerializeField] private GameObject playerOneText;
    [SerializeField] private GameObject readyText;
    private GameObject[] msPacManLifes;

    [SerializeField] private GameObject msPacMan;
    [SerializeField] private GhostManager ghostManager;
    [SerializeField] private GameObject[] mapGrids;
    [SerializeField] private GameObject mapGrid;
    [SerializeField] private GameObject gameOverTextPrefab;
    [SerializeField] private GameObject Credit0TextPrefab;

    private int state = 0;

    float timeToPassLevel = 1;
    float timeMsPacManDead = 1;
    float timeToChangeMapTransition = 0.5f;
    float gameOverTime = 2;
    int timesMapBlinked = 0;
    bool dead = false;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Fruit[] fruit;
    [SerializeField] private SpriteRenderer[] scoreSpriteRenderer;
    [SerializeField] InactiveObject[] scoreInactiveObjects;
    [SerializeField] private Sprite[] scoreSprites;

    [SerializeField] private SpriteRenderer ghostScoreSprite;

    Vector2Int msPacManPositionOnGrid = new Vector2Int();

    PlayerMovement playerMovement;
    MapManager mapManager;
    private void Start()
    {
        Application.targetFrameRate = 60;
        timer = timeToChangeState;

        msPacManLifes = new GameObject[lifes + 1];

        float width = msPacManUI.GetComponent<SpriteRenderer>().bounds.size.x;
        for(int i = 0; i < lifes + 1; i++)
        {
            msPacManLifes[i] = Instantiate(msPacManUI) as GameObject;
            msPacManLifes[i].transform.position = new(firstPositionX + i * width, -32);
            if(i == lifes)
            {
                msPacManLifes[i].SetActive(false);
            }
        }
        msPacManLifes[lifes ].SetActive(false);
        playerMovement = msPacMan.GetComponent<PlayerMovement>();
        mapManager = GetComponent<MapManager>();
        SetBeginGameState();
    }
    public Vector2Int GetMsPacManPositionOnGrid()
    {
        msPacManPositionOnGrid.x = (int)msPacMan.transform.position.x;
        msPacManPositionOnGrid.y = -(int)msPacMan.transform.position.y;
        return msPacManPositionOnGrid;
    }
    private void Update()
    {
        switch (state)
        {
            case 0:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    SetReadyGameState();
                }
                break;
            case 1:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer = timeToChangeState;
                    StartGame();
                }
                break;
            case 3:
                //pass Level
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    PassLevelToMapTransition();
                }
                break;
            case 4:
                //map white-color transition
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    ColorMapToWhiteMapTransition();
                }
                break;
            case 5:
                //ms pacman dead
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    //ms pac man start turning (dead)
                    ghostManager.SetActiveGhosts(false);
                    playerMovement.StartDead();
                    state = -1;
                }
                    break;
            case 7:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    SceneManager.LoadScene("TitleScene");
                }
                break;
        }
    }
    public void DecrementLifes()
    {
        lifes--;
        if (lifes <= 0)
        {
            SetGameOverState();
        }
        else
        {
            msPacManLifes[lifes - 1].SetActive(false);
            SetReadyGameState();
        }
    }
    void SetActiveLifesUI(bool value)
    {
        for (int i = 0; i < lifes; i++)
        {
            msPacManLifes[i].SetActive(value);
        }
    }
    void CenterMsPacMan()
    {
        msPacMan.transform.position = new(14, -23.5f);
        playerMovement.ChangePacmanDirection(3);
    }
    void ColorMapToWhiteMapTransition()
    {
        RestartTimerToChangeMapTransition();
        if (mapGrid.activeSelf)
        {
            mapGrid.SetActive(false);
        }
        else
        {
            mapGrid.SetActive(true);
            timesMapBlinked++;
            if (timesMapBlinked > 3)
            {
                mapGrid.SetActive(false);
                mapGrid = mapGrids[LevelInformation.Instance.MapIndex];
                mapGrid.SetActive(true);
                mapManager.SetTileMapByIndex(LevelInformation.Instance.MapIndex);
                mapManager.SetPowerPelletPositions(LevelInformation.Instance.PowerPelletPositions);
                mapManager.GenerateDots();
                SetReadyGameState();
                ghostManager.ResetDotCounters();
                ghostManager.SetGhostLimitDots(levelManager.GetLevel());
                timesMapBlinked = 0;
                if (levelManager.GetLevel() < 8)
                {
                    levelManager.ActiveFruit();
                }
            }
        }
    }
    void PassLevelToMapTransition()
    {
        state = 4;
        ghostManager.SetActiveGhosts(false);
        mapGrid.SetActive(false);
        RestartTimerToChangeMapTransition();
    }
    void SetBeginGameState()
    {
        state = 0;
        msPacMan.SetActive(false);
        ghostManager.SetActiveGhosts(false);
        playerOneText.SetActive(true);
        readyText.SetActive(true);
        lifes = 5;
        SetActiveLifesUI(true);
        MapManager.Instance.GenerateDots();
        if (!DotManager.Instance.GetInitialized())
        {
            DotManager.Instance.InitPowerPellets();
            DotManager.Instance.InitPacDots();
        }
        DotManager.Instance.ActivePowerPelletAnimation(false);
    }
    public void SetReadyGameState()
    {
        state = 1;
        AddMsPacManOnInitialPosition();
        playerMovement.SetActiveAnimator(false);
        playerMovement.SetFirstSprite();
        playerMovement.SetPause(true);
        playerMovement.SetFright(false);
        playerOneText.SetActive(false);
        readyText.SetActive(true);
        msPacManLifes[lifes - 1].SetActive(false);
        ghostManager.InitGhosts();
        ghostManager.StopGhostMovements();
        ghostManager.SetActiveAnimations(false);
        DotManager.Instance.SetVisiblePowerPellets(true);
        DotManager.Instance.ActivePowerPelletAnimation(false);
        SetVisibleMsPacMan(true);
        timer = timeToChangeState;
        levelManager.SetElroyActive(false);
        dead = false;
    }
    void StartGame()
    {
        state = 2;
        readyText.SetActive(false);
        playerMovement.SetPause(false);
        playerMovement.RestartSpeed();
        ghostManager.OnStartGame();
        ghostManager.SetActiveAnimations(true);
        DotManager.Instance.ActivePowerPelletAnimation(true);
    }
    public void PassLevel()
    {
        state = 3;
        playerMovement.InitLevelVariables();
        levelManager.SetElroy1DotsLeft(LevelInformation.Instance.Elroy1DotsLeft);
        levelManager.SetElroy2DotsLeft(LevelInformation.Instance.Elroy2DotsLeft);
        ghostManager.SetModeTimes(LevelInformation.Instance.ModeTimes);
        playerMovement.SetPause(true);
        playerMovement.SetFirstSprite();
        RestartTimerToPassLevel();
        SetActiveFruitsAndScoreSprites(false);
        ghostManager.SetUsingGlobalCounter(false);
        ghostManager.SetGhostTimerActive(false);
        ghostManager.SetModeActive(false);
        ghostManager.StopGhostMovements();
        ghostManager.SetActiveAnimations(false);
        ghostManager.RestartEatedRoundsCompleted();
        ghostManager.SetGhostsFright(false);
        levelManager.SetMsPacManHasDied(false);
    }
    public void SetDeadState()
    {
        if (!dead)
        {
            state = 5;
            RestartTimerToDead();
            playerMovement.SetPause(true);
            playerMovement.SetFirstSprite();
            SetActiveFruitsAndScoreSprites(false);
            ghostManager.SetUsingGlobalCounter(true);
            ghostManager.ResetGlobalDotsCounter();
            ghostManager.SetGhostTimerActive(false);
            ghostManager.SetModeActive(false);
            ghostManager.StopGhostMovements();
            levelManager.SetMsPacManHasDied(true);
            dead = true;
        }
    }
    void SetGameOverState()
    {
        state = 7;
        timer = gameOverTime;
        Instantiate(gameOverTextPrefab, new Vector3(13.5f, -11.5f, 0f), Quaternion.identity);
        Instantiate(Credit0TextPrefab, new Vector3(4.5f, -32, 0f), Quaternion.identity);
        msPacMan.SetActive(false);
        DotManager.Instance.SetVisiblePowerPellets(true);
        DotManager.Instance.ActivePowerPelletAnimation(false);
    }
    public void RestartTimerToPassLevel()
    {
        timer = timeToPassLevel;
    }
    public void RestartTimerToDead()
    {
        timer = timeMsPacManDead;
    }
    void RestartTimerToChangeMapTransition()
    {
        timer = timeToChangeMapTransition;
    }
    public void SetActiveMsPacMan(bool value)
    {
        playerMovement.SetPause(!value);
    }
    public void SetVisibleMsPacMan(bool value)
    {
        playerMovement.SetVisible(value);
    }
    void AddMsPacManOnInitialPosition()
    {
        msPacMan.SetActive(true);
        CenterMsPacMan();
    }
    public void AddScoreSprite(int fruitIndex, int fruitType)
    {
        scoreSpriteRenderer[fruitIndex].sprite = scoreSprites[fruitType];
        scoreSpriteRenderer[fruitIndex].transform.position = fruit[fruitIndex].transform.position;
        scoreSpriteRenderer[fruitIndex].gameObject.SetActive(true);
        scoreInactiveObjects[fruitIndex].Restart();
    }
    void SetActiveFruitsAndScoreSprites(bool value)
    {
        for(int i = 0; i < 2; i++)
        {
            fruit[i].gameObject.SetActive(value);
            scoreSpriteRenderer[i].gameObject.SetActive(value);
        }
    }
    public void AddLife()
    {
        msPacManLifes[lifes - 1].SetActive(true);
        lifes++;
    }
    public void IncrementDotCounter()
    {
        ghostManager.IncrementCurrentDotCounter();
    }
    public void RestartGhostTimer()
    {
        ghostManager.RestartLeaveGhostTimer();
    }
    public int GetGhostModeIndex()
    {
        return ghostManager.GetModeIndex();
    }
    public int GetMsPacManDirection()
    {
        return playerMovement.GetDirection();
    }
    public void SetFrightGhostMode()
    {
        ghostManager.SetFrightMode();
    }
    public bool GhostIsUsingGlobalCounter()
    {
        return ghostManager.IsUsingGlobalCounter();
    }
    public bool IsCountingDotsForAGhost()
    {
        return ghostManager.IsCountingDotsForAGhost();
    }
    public void SetCurrentCounterGhost(string name)
    {
        ghostManager.SetCurrentCounter(name);
    }
    public void SetGhostEatedActions()
    {
        ghostManager.InitGhostEated();
        SetVisibleMsPacMan(false);
        playerMovement.SetPause(true);
        SetActiveFruitMovements(false);
        SetActiveFruitAnimations(false);
        ghostManager.IncrementEatedGhosts();
        ghostManager.SetIsCountingEatedGhosts(false);
    }
    public bool GhostIsOnEatingMode()
    {
        return ghostManager.IsOnEatingMode();
    }
    public void SetActiveFruitMovements(bool value)
    {
        for (int i = 0; i < 2; i++)
        {
            if (fruit[i].gameObject.activeSelf)
            {
                fruit[i].SetActiveGridMovement(value);
            }
        }
    }
    public void SetActiveFruitAnimations(bool value)
    {
        for (int i = 0; i < 2; i++)
        {
            if (fruit[i].gameObject.activeSelf)
            {
                fruit[i].SetActiveAnimation(value);
            }
        }
    }
    public void SpawnGhostScore(Vector2 position)
    {
        ghostScoreSprite.transform.position = position;
        ghostScoreSprite.gameObject.SetActive(true);
        scoreInactiveObjects[2].Restart();
    }
    public void StartEatedGhostsCounter()
    {
        ghostManager.StartEatedGhostsCounter();
    }
}