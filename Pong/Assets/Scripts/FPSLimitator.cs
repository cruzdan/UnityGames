using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimitator : MonoBehaviour
{
    // NOTE(isaveg): Belongs to the PongGameManager, no extra class for this functionality needed
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Destroy(this.gameObject);
    }
}
