using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGameManager : MonoBehaviour
{
    [SerializeField] private GameObject paddle1;
    [SerializeField] private GameObject paddle2;
    [SerializeField] private GameObject wallPrefab;
    private GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        AddOuterWalls();
        paddle1.transform.position = new Vector2(-SquaresResolution.TotalSquaresX / 2f + SquaresResolution.TotalSquaresX / 20f, 0f);
        paddle2.transform.position = new Vector2(-paddle1.transform.position.x, paddle1.transform.position.y);
        if (PlayerController.Instance.GetPlayers() == 1)
        {
            paddle2.AddComponent<AIMovement>();
            paddle2.GetComponent<AIMovement>().Init();
        }
        else
        {
            paddle2.AddComponent<PlayerMovement>();
            paddle2.GetComponent<PlayerMovement>().up = KeyCode.UpArrow;
            paddle2.GetComponent<PlayerMovement>().down = KeyCode.DownArrow;
        }
    }

    void AddOuterWalls()
    {
        Vector2 position;
        Vector2 scale;

        //left
        position = new Vector2(-SquaresResolution.TotalSquaresX / 2f - wallPrefab.transform.localScale.x / 2f, 0f);
        scale = new Vector2(1f, SquaresResolution.TotalSquaresY + (2 * wallPrefab.transform.localScale.y));
        GenerateOuterWall(position, scale, "PlayerWall");


        //right
        position.x *= -1f;
        GenerateOuterWall(position, scale, "PlayerWall");

        //up
        position = new Vector2(0f, SquaresResolution.TotalSquaresY / 2f + wallPrefab.transform.localScale.y / 2f);
        scale = new Vector2(SquaresResolution.TotalSquaresX + (2 * wallPrefab.transform.localScale.x), 1f);
        GenerateOuterWall(position, scale, "Untagged");

        //down
        position.y *= -1f;
        GenerateOuterWall(position, scale, "Untagged");
    }

    void GenerateOuterWall(Vector2 position, Vector2 scale, string tag)
    {
        wall = Instantiate(wallPrefab) as GameObject;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.tag = tag;
    }
}
