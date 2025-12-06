using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Stun : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private List<MonoBehaviour> componentsToDisable;
    #endregion
    #region Private Variables
    private Coroutine stunCoroutine;
    #endregion
    #region Functions
    public void StartStunPlayerCoroutine(float timeToStun)
    {
        rb.linearVelocity = Vector2.zero;
        stunCoroutine = StartCoroutine(StunPlayer(timeToStun));
    }

    [ClientRpc()]
    public void StartStunPlayerCoroutineClientRpc(float timeToStun)
    {
        StartStunPlayer(timeToStun);
    }

    void StartStunPlayer(float timeToStun)
    {
        rb.linearVelocity = Vector2.zero;
        stunCoroutine = StartCoroutine(StunPlayer(timeToStun));
    }

    public void StopStunPlayer()
    {
        CoroutineExtensions.StopCoroutineSafe(this, ref stunCoroutine);
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
    #endregion
}
