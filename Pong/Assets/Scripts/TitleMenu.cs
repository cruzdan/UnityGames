using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    public void SelectPlayers(int players)
    {
        PlayerManager.Instance.SetPlayers(players);
    }
}
