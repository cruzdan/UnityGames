using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelInformation : Singleton<LevelInformation>
{
    //100% speed = 75.75757625 pixels / sec, 1 square = 8 pixels
    public float MaxSpeed { get; private set; } = 75.75757625f / 8f;
    //pacman speed on level
    public float PacManSpeed { get; private set; } = 0.8f * 75.75757625f / 8f;
    //speed when pacman eats a dot
    public float PacManDotsSpeed { get; private set; } = 0.71f * 75.75757625f / 8f;
    //speed when pacman eats a Power Pellet
    public float FrightPacManDotsSpeed { get; private set; } = 0.79f * 75.75757625f / 8f;
    //pacman speed on fright mode
    public float FrightPacManSpeed { get; private set; } = 0.9f * 75.75757625f / 8f;
    public float GhostSpeed { get; private set; } = 0.75f * 75.75757625f / 8f;
    public float GhostTunnelSpeed { get; private set; } = 0.4f * 75.75757625f / 8f;
    public float FrightGhostSpeed { get; private set; } = 0.5f * 75.75757625f / 8f;
    public float FrightTime { get; private set; } = 6;
    public int[] FruitTypes { get; private set; } = { 0, 0 };
    public int Elroy1DotsLeft { get; private set; } = 20;
    public int Elroy2DotsLeft { get; private set; } = 10;
    public float Elroy1Speed { get; private set; } = 0.8f * 75.75757625f / 8f;
    public float Elroy2Speed { get; private set; } = 0.85f * 75.75757625f / 8f;
    public int MapIndex { get; private set; } = 0;
    public Vector2Int FirstTileMapPosition { get; private set; } = new Vector2Int(-14, 14);
    public Vector2Int[] TunnelEntrances { get; private set; } = { new Vector2Int(2, 8), new Vector2Int(25, 8), new Vector2Int(2, 17), new Vector2Int(25, 17) };
    public Vector2[] PowerPelletPositions { get; private set; } = { new Vector2(1.5f, -2.5f), new Vector2(26.5f, -2.5f), new Vector2(1.5f, -27.5f), new Vector2(26.5f, -27.5f) };
    public float[] ModeTimes { get; private set; } = { 7, 20, 7, 20, 5, 20, 5 };
    public float[] FruitSpawnPositionY { get; private set; } = { -8.5f, -17.5f };
    string[] fileContentLines = new string[27];
    string[] data;
    private void Start()
    {
        fileContentLines = FileReader.GetContentFromFileBuild("PacMan Info.csv");
    }
    public void SetLevelInformation(int level)
    {
        if (level < 21)
            data = fileContentLines[level].Split(',');
        else
            data = fileContentLines[21].Split(',');
        PacManSpeed = float.Parse(data[1]) * MaxSpeed;
        PacManDotsSpeed = float.Parse(data[2]) * MaxSpeed;
        GhostSpeed = float.Parse(data[3]) * MaxSpeed;
        GhostTunnelSpeed = float.Parse(data[4]) * MaxSpeed;
        Elroy1DotsLeft = int.Parse(data[5]);
        Elroy1Speed = float.Parse(data[6]) * MaxSpeed;
        Elroy2DotsLeft = int.Parse(data[7]);
        Elroy2Speed = float.Parse(data[8]) * MaxSpeed;
        FrightPacManSpeed = float.Parse(data[9]) * MaxSpeed;
        FrightPacManDotsSpeed = float.Parse(data[10]) * MaxSpeed;
        FrightGhostSpeed = float.Parse(data[11]) * MaxSpeed;
        FrightTime = float.Parse(data[12]);
        ModeTimes[0] = float.Parse(data[13]);
        ModeTimes[1] = float.Parse(data[14]);
        ModeTimes[2] = float.Parse(data[15]);
        ModeTimes[3] = float.Parse(data[16]);
        ModeTimes[4] = float.Parse(data[17]);
        ModeTimes[5] = float.Parse(data[18]);
        ModeTimes[6] = float.Parse(data[19]);

        SetFruitTypes(level);
        SetMapIndex(level);
        SetMapVariables(MapIndex);
        FruitSpawnPositionY[0] = float.Parse(data[19]);
        FruitSpawnPositionY[1] = float.Parse(data[20]);

    }
    void SetFruitTypes(int level)
    {
        if(level < 8)
        {
            FruitTypes[0] = FruitTypes[1] = int.Parse(data[20]);
        }
        else
        {
            FruitTypes[0] = Random.Range(0, 7);
            FruitTypes[1] = Random.Range(0, 7);
        }
    }
    void SetMapIndex(int level)
    {
        if(level > 13)
        {
            level -= 13;
            if(level % 4 == 1)
            {
                int lastMapIndex = MapIndex;
                do
                {
                    MapIndex = Random.Range(0, 4);
                } while (MapIndex == lastMapIndex);
                DotManager.Instance.SetDotColorsByIndex(MapIndex);
            }
        }
        else
        {
            switch (level)
            {
                case 3:
                    //map 2
                    MapIndex = 1;
                    DotManager.Instance.SetDotColorsByIndex(MapIndex);
                    break;
                case 6:
                    //map 3
                    MapIndex = 2;
                    DotManager.Instance.SetDotColorsByIndex(MapIndex);
                    break;
                case 10:
                    //map 4
                    MapIndex = 3;
                    DotManager.Instance.SetDotColorsByIndex(MapIndex);
                    break;
            }
        }
    }
    void SetMapVariables(int index)
    {
        data = fileContentLines[index + 23].Split(',');
        FirstTileMapPosition = new Vector2Int(int.Parse(data[1]), int.Parse(data[2]));
        int totalEntrances;
        if(index == 2)
        {
            TunnelEntrances = new Vector2Int[2];
            totalEntrances = 2;
        }
        else
        {
            TunnelEntrances = new Vector2Int[4];
            totalEntrances = 4;
        }
        for(int i = 0; i < totalEntrances; i++)
        {
            TunnelEntrances[i] = new Vector2Int(int.Parse(data[3 + i * 2]), int.Parse(data[4 + i * 2]));
        }
        for(int i = 0; i < 4; i++)
        {
            PowerPelletPositions[i] = new Vector2(float.Parse(data[11 + i * 2]), float.Parse(data[12 + i * 2]));
        }
    }
}