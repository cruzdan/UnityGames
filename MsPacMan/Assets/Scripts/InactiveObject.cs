using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveObject : MonoBehaviour
{
    [SerializeField] private float timeToSetInactive;
    float timer;
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void Restart()
    {
        timer = timeToSetInactive;
        gameObject.SetActive(true);
    }
}
