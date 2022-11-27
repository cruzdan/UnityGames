using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGameManager : MonoBehaviour
{
    [SerializeField] private GameObject paddle1;
    [SerializeField] private GameObject paddle2;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject wallTriggerPrefab;
    [SerializeField] private GameObject ball;
    [SerializeField] private bool inGame;
    private GameObject wall;
    void Start()
    {
        Application.targetFrameRate = 60;
        if (!inGame) return;
        AddOuterWalls();
        paddle1.transform.position = new Vector2(-SquaresResolution.TotalSquaresX / 2f + SquaresResolution.TotalSquaresX / 20f, 0f);
        paddle2.transform.position = new Vector2(-paddle1.transform.position.x, paddle1.transform.position.y);
        if (PlayerManager.Instance.GetPlayers() == 1)
        {
            paddle2.AddComponent<AIMovement>();
            paddle2.GetComponent<AIMovement>().Init();
        }
        else
        {
            PlayerMovement paddle2Movement = paddle2.AddComponent<PlayerMovement>();
            paddle2Movement.up = KeyCode.UpArrow;
            paddle2Movement.down = KeyCode.DownArrow;
            paddle2Movement.SetSpeed(6);
        }
    }

    void AddOuterWalls()
    {
        Vector2 position;
        Vector2 scale;

        //left
        position = new Vector2(-SquaresResolution.TotalSquaresX / 2f - wallPrefab.transform.localScale.x / 2f, 0f);
        scale = new Vector2(1f, SquaresResolution.TotalSquaresY + (2 * wallPrefab.transform.localScale.y));
        GenerateOuterWall(position, scale, "BoundWall", wallTriggerPrefab);


        //right
        position.x *= -1f;
        GenerateOuterWall(position, scale, "BoundWall", wallTriggerPrefab);

        //up
        position = new Vector2(0f, SquaresResolution.TotalSquaresY / 2f + wallPrefab.transform.localScale.y / 2f);
        scale = new Vector2(SquaresResolution.TotalSquaresX + (2 * wallPrefab.transform.localScale.x), 1f);
        GenerateOuterWall(position, scale, "Untagged", wallPrefab);
        GenerateOuterWall(position, scale, "PlayerWall", wallTriggerPrefab);


        //down
        position.y *= -1f;
        GenerateOuterWall(position, scale, "Untagged", wallPrefab);
        GenerateOuterWall(position, scale, "PlayerWall", wallTriggerPrefab);
    }

    void GenerateOuterWall(Vector2 position, Vector2 scale, string tag, GameObject prefab)
    {
        wall = Instantiate(prefab) as GameObject;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.tag = tag;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
