using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ghostsObjects;
    [SerializeField] private GhostBehaviour[] ghostBehaviours;
    [SerializeField] private GameManager gameManager;
    //next ghost in exit from the ghost house
    int currentCounter = 1;
    int currentDotLimit = 0;

    bool usingGlobalCounter = false;
    int globalCounter = 0;
    //timer to leave ghosts from the ghost gouse
    float leaveGhostTimer;
    [SerializeField] private float timeLimit;
    bool leaveGhostTimerIsActive = false;

    [SerializeField] private float[] changeModeTimes = new float[7];
    bool changeModeActive;
    float changeModeTimer;
    //0-> Scatter, 1-> Chase...
    int changeModeIndex = 0;

    bool eating = false;
    [SerializeField] private float eatingTime;
    float eatingTimer;

    float frightEatedGhostsTimer;
    bool isCountingEatedGhosts;
    int eatedGhosts;
    //4 ghosts eated with 1 power pellet per level
    int eatedRoundsCompleted = 0;

    [SerializeField] private SpriteRenderer scoreSprite;
    [SerializeField] private Score score;
    [SerializeField] private Sprite[] ghostScoreSprites;

    [SerializeField] private LevelManager levelManager;
    public void InitGhosts()
    {
        SetActiveGhosts(true);
        SetInitialDirections();
        SetInitialPositions();
        SetInitialGhostMovements();
        SetInitialGhostSpeeds();
        SetGhostsDead(false);
        SetGhostsFright(false);
        SetFirstGhostSprites();
        SetGridTurnActive(false);
        SetGhostsMovingToInitialPosition(false);
        SetVisibleGhosts(true);
        isCountingEatedGhosts = false;
        frightEatedGhostsTimer = 0;
        currentCounter = 1;
        currentDotLimit = ghostBehaviours[currentCounter].GetDotLimit();
        eating = false;
    }
    public void OnStartGame()
    {
        if (!usingGlobalCounter)
        {
            ExitGhostWithLimitReached();
        }
        leaveGhostTimerIsActive = true;
        leaveGhostTimer = 0;
        StartGhostMovements();
        changeModeActive = true;
        changeModeTimer = 0;
        changeModeIndex = 0;
    }
    void Update()
    {
        if (leaveGhostTimerIsActive)
        {
            leaveGhostTimer += Time.deltaTime;
            if(leaveGhostTimer >= timeLimit)
            {
                if(currentCounter < 4)
                {
                    LeaveGhost(currentCounter);
                    SetFirstGhostOnHouseAsCurrentCounter();
                    leaveGhostTimer = 0;
                }
            }
        }
        if (changeModeActive)
        {
            if(changeModeIndex < 7)
            {
                changeModeTimer += Time.deltaTime;
                if(changeModeTimer >= changeModeTimes[changeModeIndex])
                {
                    changeModeIndex++;
                    changeModeTimer = 0;
                    if (!eating)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            switch (ghostBehaviours[i].GetMovementType())
                            {
                                case GhostBehaviour.MovementType.Chase:
                                case GhostBehaviour.MovementType.Scatter:
                                case GhostBehaviour.MovementType.RandomMovement:
                                    ghostBehaviours[i].TurnGrid();

                                    if (changeModeIndex % 2 == 0)
                                    {
                                        ghostBehaviours[i].ChangeMovementToScatter();
                                    }
                                    else
                                    {
                                        ghostBehaviours[i].ChangeMovementToChase();
                                    }
                                    break;
                            }
                        }
                    }
                    if (changeModeIndex >= 7)
                    {
                        changeModeActive = false;
                    }
                }
            }
        }
        if (eating)
        {
            eatingTimer -= Time.deltaTime;
            if(eatingTimer <= 0)
            {
                eating = false;
                gameManager.SetActiveMsPacMan(true);
                gameManager.SetVisibleMsPacMan(true);
                gameManager.SetActiveFruitMovements(true);
                gameManager.SetActiveFruitAnimations(true);
                if (currentCounter < 4)
                {
                    leaveGhostTimerIsActive = true;
                }
                SetVisibleGhosts(true);
                ActiveAllghostMovementsAndAnimations();
                SetIsCountingEatedGhosts(true);
            }
        }
        if (isCountingEatedGhosts)
        {
            frightEatedGhostsTimer -= Time.deltaTime;
            if(frightEatedGhostsTimer <= 0)
            {
                isCountingEatedGhosts = false;
                eatedGhosts = 0;
            }
        }
    }
    void ActiveAllghostMovementsAndAnimations()
    {
        for (int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetActiveAnimation(true);
            ghostBehaviours[i].enabled = true;
            ghostBehaviours[i].SetActiveGridMovementIfNeeded();
        }
    }
    void SetVisibleGhosts(bool value)
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetVisible(value);
        }
    }
    public void StartEatedGhostsCounter()
    {
        frightEatedGhostsTimer = LevelInformation.Instance.FrightTime;
        eatedGhosts = 0;
        isCountingEatedGhosts = true;
    }
    void SetGhostsMovingToInitialPosition(bool value)
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetMovingToInitialPosition(value);
        }
    }
    public void SetGhostsDead(bool value)
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetDead(value);
        }
    }
    public void SetGhostsFright(bool value)
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetFright(value);
        }
    }
    public void SetActiveAnimations(bool value)
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetActiveAnimation(value);
        }
    }
    public void SetGhostTimerActive(bool value)
    {
        leaveGhostTimerIsActive = value;
    }
    void ExitGhostWithLimitReached()
    {
        //exit the ghosts who passed their dot limit
        while (currentCounter < 4 && ghostBehaviours[currentCounter].GetDotCounter() >= currentDotLimit)
        {
            LeaveGhost(currentCounter);
            SetFirstGhostOnHouseAsCurrentCounter();
        }
    }
    void LeaveGhost(int index)
    {
        ghostBehaviours[index].ChangeMovementToLeavingHouse();
        if(index == 3)
        {
            levelManager.CheckBlinkyElroyTransition();
            levelManager.SetMsPacManHasDied(false);
        }
    }
    
    void StartGhostMovements()
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].InitOnStartGameVariables();
        }
    }
    public void StopGhostMovements()
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].PauseMovement();
        }
    }
    public void SetActiveGhosts(bool value)
    {
        for (int i = 0; i < 4; i++)
        {
            ghostsObjects[i].SetActive(value);
        }
    }
    //Set the first directions of the ghosts when are on the ghost house
    void SetInitialDirections()
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetInitialDirection();
        }
    }
    public void SetInitialGhostMovements()
    {
        for (int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetInitialMovement();
        }
    }
    public void SetInitialGhostSpeeds()
    {
        for (int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetSpeed(LevelInformation.Instance.GhostSpeed);
            ghostBehaviours[i].SetGridSpeed(LevelInformation.Instance.GhostSpeed);
        }
    }
    public void SetFirstGhostSprites()
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetFirstSprite();
        }
    }
    void SetGridTurnActive(bool value)
    {
        for(int i =0; i < 4; i++)
        {
            ghostBehaviours[i].SetGridTurnActive(value);
        }
    }
    void SetInitialPositions()
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].SetInitialPosition();
        }
    }
    public void ResetDotCounters()
    {
        for(int i = 0; i < 4; i++)
        {
            ghostBehaviours[i].ResetDotCounter();
        }
    }
    public void IncrementCurrentDotCounter()
    {
        if (!usingGlobalCounter)
        {
            if (currentCounter < 4)
            {
                ghostBehaviours[currentCounter].IncrementDotCounter();
                if (ghostBehaviours[currentCounter].GetDotCounter() >= currentDotLimit)
                {
                    LeaveGhost(currentCounter);
                    SetFirstGhostOnHouseAsCurrentCounter();
                }
            }
        }
        else
        {
            globalCounter++;
            switch (globalCounter)
            {
                case 7:
                    if(ghostBehaviours[1].GetMovementType() == GhostBehaviour.MovementType.OnHouse)
                    {
                        LeaveGhost(1);
                        currentCounter = 2;
                    }
                    break;
                case 17:
                    if (ghostBehaviours[2].GetMovementType() == GhostBehaviour.MovementType.OnHouse)
                    {
                        LeaveGhost(2);
                        currentCounter = 3;
                    }
                    break;
                case 32:
                    if (ghostBehaviours[3].GetMovementType() == GhostBehaviour.MovementType.OnHouse)
                    {
                        LeaveGhost(3);
                        currentCounter = GetFirstGhostIndexOnHouse();
                        usingGlobalCounter = false;
                    }
                    break;
            }
        }
    }
    int GetFirstGhostIndexOnHouse()
    {
        for(int i = 1; i < 4; i++)
        {
            if(ghostBehaviours[i].GetMovementType() == GhostBehaviour.MovementType.OnHouse)
            {
                return i;
            }
        }
        return 4;
    }
    void SetFirstGhostOnHouseAsCurrentCounter()
    {
        currentCounter = GetFirstGhostIndexOnHouse();
        if (currentCounter < 4)
        {
            currentDotLimit = ghostBehaviours[currentCounter].GetDotLimit();
        }
    }
    public void SetCurrentCounter(string name)
    {
        int i = 5;
        switch (name)
        {
            case "Blinky":
                i = 0;
                break;
            case "Pinky":
                i = 1;
                break;
            case "Inky":
                i = 2;
                break;
            case "Sue":
                i = 3;
                break;
        }
        currentCounter = i;
        currentDotLimit = ghostBehaviours[currentCounter].GetDotLimit();
    }
    public void SetUsingGlobalCounter(bool value)
    {
        usingGlobalCounter = value;
    }
    public bool IsUsingGlobalCounter()
    {
        return usingGlobalCounter;
    }
    public bool IsCountingDotsForAGhost()
    {
        return currentCounter < 4;
    }
    public void ResetGlobalDotsCounter()
    {
        globalCounter = 0;
    }
    public void RestartLeaveGhostTimer()
    {
        leaveGhostTimer = 0;
    }
    public int GetModeIndex()
    {
        return changeModeIndex;
    }
    public void SetModeActive(bool value)
    {
        changeModeActive = value;
    }
    public void SetFrightMode()
    {
        for(int i = 0; i < 4; i++)
        {
            if (!ghostBehaviours[i].GetDead())
            {
                ghostBehaviours[i].SetFrightMode(LevelInformation.Instance.FrightTime);
                switch (ghostBehaviours[i].GetMovementType())
                {
                    case GhostBehaviour.MovementType.Chase:
                    case GhostBehaviour.MovementType.Scatter:
                    case GhostBehaviour.MovementType.RandomMovement:
                        ghostBehaviours[i].TurnGrid();
                        ghostBehaviours[i].ChangeMovementToFright();
                        break;
                }
            }
            else if (ghostBehaviours[i].IsFright())
            {
                ghostBehaviours[i].SetFrightMode(LevelInformation.Instance.FrightTime);
            }
        }
    }
    void SetActiveGhostsForDeadState(bool value)
    {
        for(int i = 0; i < 4; i++)
        {
            if (!ghostBehaviours[i].GetDead())
            {
                ghostBehaviours[i].SetActiveAnimation(value);
                ghostBehaviours[i].SetActiveGridMovement(value);
                ghostBehaviours[i].enabled = value;
            }
        
        }
    }
    public void InitGhostEated()
    {
        eating = true;
        eatingTimer = eatingTime;
        leaveGhostTimerIsActive = false;
        SetActiveGhostsForDeadState(false);
    }
    public bool IsOnEatingMode()
    {
        return eating;
    }
    public void IncrementEatedGhosts()
    {
        scoreSprite.sprite = ghostScoreSprites[eatedGhosts];
        eatedGhosts++;
        if(eatedGhosts > 3)
        {
            isCountingEatedGhosts = false;
            eatedRoundsCompleted++;
            if(eatedRoundsCompleted > 3)
            {
                score.AddScore(12000);
            }
        }
        score.AddScore(GetScoreForEatedGhost(eatedGhosts));
    }
    public void SetIsCountingEatedGhosts(bool value)
    {
        isCountingEatedGhosts = value;
    }
    int GetScoreForEatedGhost(int index)
    {
        return index switch
        {
            1 => 200,
            2 => 400,
            3 => 800,
            4 => 1600,
            _ => 0,
        };
    }
    public void RestartEatedRoundsCompleted()
    {
        eatedRoundsCompleted = 0;
    }
    public void SetModeTimes(float[] newTimes)
    {
        changeModeTimes = newTimes;
    }
    public void SetGhostLimitDots(int level)
    {
        ghostBehaviours[0].SetDotLimit(0);
        ghostBehaviours[1].SetDotLimit(0);
        switch (level)
        {
            case 1:
                ghostBehaviours[2].SetDotLimit(30);
                ghostBehaviours[3].SetDotLimit(60);
                break;
            case 2:
                ghostBehaviours[2].SetDotLimit(0);
                ghostBehaviours[3].SetDotLimit(50);
                break;
            default:
                ghostBehaviours[2].SetDotLimit(0);
                ghostBehaviours[3].SetDotLimit(0);
                break;
        }
    }
    public void SetGhostsTargetScatterPositions(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
    {
        ghostBehaviours[0].SetTargetScatterPosition(x1, y1);
        ghostBehaviours[1].SetTargetScatterPosition(x2, y2);
        ghostBehaviours[2].SetTargetScatterPosition(x3, y3);
        ghostBehaviours[3].SetTargetScatterPosition(x4, y4);
    }
}
