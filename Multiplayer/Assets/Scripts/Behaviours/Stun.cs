using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private List<MonoBehaviour> componentsToDisable;
    Coroutine stunCoroutine;
    public void StartStunPlayer(float timeToStun)
    {
        rb.velocity = Vector2.zero;
        stunCoroutine = StartCoroutine(StunPlayer(timeToStun));
    }

    public void StopStunPlayer()
    {
        ActiveStunComponents(true);
    }

    void ActiveStunComponents(bool value)
    {
        for (int i = 0; i < componentsToDisable.Count; i++)
        {
            componentsToDisable[i].enabled = value;
        }
    }

    IEnumerator StunPlayer(float time)
    {
        ActiveStunComponents(false);
        yield return new WaitForSeconds(time);
        ActiveStunComponents(true);
    }
}
