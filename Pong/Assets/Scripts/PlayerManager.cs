using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [SerializeField] private int players = 1;
    private void Awake()
    {
        if(PlayerManager.Instance == null)
        {
            PlayerManager.Instance = this;
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
        ChangeScene("GamePong");
    }
    public void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public int GetPlayers()
    {
        return players;
    }
}
