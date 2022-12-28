using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int level = 1;
    int totalDots;
    [SerializeField] private Fruit[] fruit;
    [SerializeField] private SpriteRenderer[] fruitSpriteRenderer;
    [SerializeField] private Sprite[] fruitSprites;
    [SerializeField] private MovementForGrid[] fruitMovement;
    [SerializeField] private GameObject[] fruitsFounded;
    [SerializeField] private GhostManager ghostManager;

    //fruit spawn
    int firstLeftFruitDots;
    const int secondLeftFruitDots = 66;

    Vector2Int targetPos = new Vector2Int();
    Vector2 fruitPosition;
    int fruitDirection;

    //Cruise Elroy
    [SerializeField] private BlinkyBehaviour blinkyBehaviour;
    [SerializeField] private int elroy1DotsLeft;
    [SerializeField] private int elroy2DotsLeft;
    bool msPacManHasDied = false;
    private void Start()
    {
        elroy1DotsLeft = LevelInformation.Instance.Elroy1DotsLeft;
        elroy2DotsLeft = LevelInformation.Instance.Elroy2DotsLeft;
    }
    public void SetTotalDots(int value)
    {
        totalDots = value;
        firstLeftFruitDots = totalDots - 64;
    }
    public void SetElroy1DotsLeft(int value)
    {
        elroy1DotsLeft = value;
    }
    public void SetElroy2DotsLeft(int value)
    {
        elroy2DotsLeft = value;
    }
    public void DecrementTotalDots()
    {
        totalDots--;
        if(totalDots == firstLeftFruitDots)
        {
            GenerateFruit(0);
        }
        else if(totalDots == secondLeftFruitDots)
        {
            GenerateFruit(1);
        }
        if (!msPacManHasDied)
        {
            if (!blinkyBehaviour.GetElroyActive()) 
            {
                if (totalDots == elroy1DotsLeft)
                {
                    blinkyBehaviour.SetElroyActive(true);
                    blinkyBehaviour.ChangeModeToElroy();
                    blinkyBehaviour.SetElroyPhase(1);
                    switch (blinkyBehaviour.GetMovementType())
                    {
                        case GhostBehaviour.MovementType.Scatter:
                        case GhostBehaviour.MovementType.Chase:
                            blinkyBehaviour.SetSpeed(LevelInformation.Instance.Elroy1Speed);
                            blinkyBehaviour.SetGridSpeed(LevelInformation.Instance.Elroy1Speed);
                            break;
                    }
                }
            }
            else
            {
                if (totalDots == elroy2DotsLeft)
                {
                    blinkyBehaviour.SetElroyPhase(2);
                    switch (blinkyBehaviour.GetMovementType())
                    {
                        case GhostBehaviour.MovementType.Scatter:
                        case GhostBehaviour.MovementType.Chase:
                            blinkyBehaviour.SetSpeed(LevelInformation.Instance.Elroy2Speed);
                            blinkyBehaviour.SetGridSpeed(LevelInformation.Instance.Elroy2Speed);
                            break;
                    }
                }
            }
        }
        if(totalDots <= 0)
        {
            level++;
            int currentMapIndex = LevelInformation.Instance.MapIndex;
            LevelInformation.Instance.SetLevelInformation(level);
            if (currentMapIndex != LevelInformation.Instance.MapIndex)
            {
                if (LevelInformation.Instance.MapIndex == 1)
                {
                    ghostManager.SetGhostsTargetScatterPositions(24, 6, 3, 6, 22, 31, 5, 31);
                }
                else
                {
                    ghostManager.SetGhostsTargetScatterPositions(28, -4, -1, -4, 28, 34, -1, 34);
                }
            }
            GameManager.Instance.PassLevel();
        }
    }
    public void ActiveFruit()
    {
        fruitsFounded[level - 2].SetActive(true);
    }
    void GenerateFruit(int index)
    {
        //0->up, left  | 1-> up, right  | 2-> down, left  | 3-> down, right
        int firstTunnelIndex = Random.Range(0, LevelInformation.Instance.TunnelEntrances.Length);
        int secondTunnelIndex;
        do
        {
            secondTunnelIndex = Random.Range(0, LevelInformation.Instance.TunnelEntrances.Length);
        } while (secondTunnelIndex == firstTunnelIndex);
        UpdateFruitVariables(firstTunnelIndex, secondTunnelIndex);
        fruit[index].Init();
        fruit[index].SetTunnelEntrance(LevelInformation.Instance.TunnelEntrances[firstTunnelIndex]);
        fruitMovement[index].SetTargetPosition(targetPos);
        fruitMovement[index].SetDirection(fruitDirection);
        fruitSpriteRenderer[index].sprite = fruitSprites[LevelInformation.Instance.FruitTypes[index]];
        fruit[index].transform.position = fruitPosition;
        fruit[index].gameObject.SetActive(true);
    }
    void UpdateFruitVariables(int firstTunnel, int secondTunnel)
    {
        switch (firstTunnel)
        {
            case 0:
                //up left
                fruitDirection = 1;
                fruitPosition = new Vector2(-0.75f, LevelInformation.Instance.FruitSpawnPositionY[0]);
                break;
            case 1:
                //up right
                fruitDirection = 3;
                fruitPosition = new Vector2(28.8f, LevelInformation.Instance.FruitSpawnPositionY[0]);
                break;
            case 2:
                //down left 
                fruitDirection = 1;
                fruitPosition = new Vector2(-0.75f, LevelInformation.Instance.FruitSpawnPositionY[1]);
                break;
            case 3:
                //down right 
                fruitDirection = 3;
                fruitPosition = new Vector2(28.8f, LevelInformation.Instance.FruitSpawnPositionY[1]);
                break;
        }
        switch (secondTunnel)
        {
            case 0:
                //up left
                targetPos = LevelInformation.Instance.TunnelEntrances[0];
                break;
            case 1:
                //up right
                targetPos = LevelInformation.Instance.TunnelEntrances[1];
                break;
            case 2:
                //down left 
                targetPos = LevelInformation.Instance.TunnelEntrances[2];
                break;
            case 3:
                //down right
                targetPos = LevelInformation.Instance.TunnelEntrances[3];
                break;
        }
    }
    public void CheckBlinkyElroyTransition()
    {
        if (msPacManHasDied && totalDots <= elroy1DotsLeft)
        {
            blinkyBehaviour.SetElroyActive(true);
            blinkyBehaviour.ChangeModeToElroy();
            if (totalDots > elroy2DotsLeft)
            {
                blinkyBehaviour.SetSpeed(LevelInformation.Instance.Elroy1Speed);
                blinkyBehaviour.SetGridSpeed(LevelInformation.Instance.Elroy1Speed);
            }
            else
            {
                blinkyBehaviour.SetSpeed(LevelInformation.Instance.Elroy2Speed);
                blinkyBehaviour.SetGridSpeed(LevelInformation.Instance.Elroy2Speed);
            }
        }
    }
    public void SetMsPacManHasDied(bool value)
    {
        msPacManHasDied = value;
    }
    public void SetElroyActive(bool value)
    {
        blinkyBehaviour.SetElroyActive(value);
    }
    public int GetLevel()
    {
        return level;
    }
}
