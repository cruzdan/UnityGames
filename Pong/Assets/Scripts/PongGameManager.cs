using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGameManager : MonoBehaviour
{
    [SerializeField] private GameObject paddle2;
    // Start is called before the first frame update
    void Start()
    {
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
}
