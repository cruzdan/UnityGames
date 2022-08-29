using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField] private int players;
    private void Awake()
    {
        if(PlayerController.Instance == null)
        {
            PlayerController.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayers(int p)
    {
        players = p;
        SceneManager.LoadScene("GamePong");
    }

    public int GetPlayers()
    {
        return players;
    }
}
