using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Tilemap[] tileMaps;
    [SerializeField] private Tilemap tileMap;
    GameObject ball;
    [SerializeField] private Vector2[] powerPelletPositions;
    [SerializeField] private DotManager dotManager;
    [SerializeField] private TileBase blackTile;
    const int rows = 28;
    const int columns = 31;
    private readonly bool[] mapCollisions = new bool[rows * columns];

    public void GenerateDots()
    {
        int totalDots = 0;
        int mapIndex = 0;

        int totalY = LevelInformation.Instance.FirstTileMapPosition.y - columns + 1;
        int totalX = LevelInformation.Instance.FirstTileMapPosition.x + rows;


        for (int j = LevelInformation.Instance.FirstTileMapPosition.y; j >= totalY; j--)
        {
            for (int i = LevelInformation.Instance.FirstTileMapPosition.x; i < totalX; i++)
            {
                if (tileMap.HasTile(new Vector3Int(i, j)))
                {
                    if (tileMap.GetTile(new Vector3Int(i, j)).Equals(blackTile))
                    {
                        mapCollisions[mapIndex] = false;
                        mapIndex++;
                    }
                    else
                    {
                        mapCollisions[mapIndex] = true;
                        mapIndex++;
                    }
                }
                else
                {
                    mapCollisions[mapIndex] = false;
                    mapIndex++;
                    ball = dotManager.GetPacDot();
                    ball.transform.position = new Vector2(i + tileMap.transform.position.x + 0.5f, j + tileMap.transform.position.y + 0.5f);
                    totalDots++;
                }
            }
        }
        for (int i = 0; i < powerPelletPositions.Length; i++)
        {
            ball = dotManager.GetPowerPellet();
            ball.transform.position = new Vector2(powerPelletPositions[i].x, powerPelletPositions[i].y);
            totalDots++;
        }
        GetComponent<LevelManager>().SetTotalDots(totalDots);
    }
    public bool GetMapCollision(int index)
    {
        return mapCollisions[index];
    }
    public bool GetMapCollision(Vector2Int position)
    {
        return mapCollisions[position.x + position.y * rows];
    }
    public bool GetUpMapCollision(Vector2Int position)
    {
        return mapCollisions[position.x + (position.y - 1) * rows];
    }
    public bool GetDownMapCollision(Vector2Int position)
    {
        return mapCollisions[position.x + (position.y + 1) * rows];
    }
    public bool GetRightMapCollision(Vector2Int position)
    {
        return mapCollisions[position.x + position.y * rows + 1];
    }
    public bool GetLeftMapCollision(Vector2Int position)
    {
        return mapCollisions[position.x + position.y * rows - 1];
    }
    public void SetTileMapByIndex(int index)
    {
        tileMap = tileMaps[index];
    }
    public void SetPowerPelletPositions(Vector2[] newPositions)
    {
        powerPelletPositions = newPositions;
    }
}