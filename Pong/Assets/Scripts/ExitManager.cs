using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    // NOTE(isaveg): Belongs to the PongGameManager, no extra class for this functionality needed
    public void Exit()
    {
        Application.Quit();
    }
}
