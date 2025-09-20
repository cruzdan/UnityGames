using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<Player> players = new List<Player>();
    public List<Player> Players { get => players; set => players = value; }
}
