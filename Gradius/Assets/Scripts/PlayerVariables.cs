using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public static PlayerVariables Instance;
    [SerializeField] private int players;
    public void SetPlayers(int p) { players = p; }
    public int GetPlayers() { return players; }
    private void Awake()
    {
        if (PlayerVariables.Instance == null)
        {
            PlayerVariables.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}