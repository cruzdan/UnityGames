using System.Collections;
using Unity.Netcode;
using UnityEngine;
public class Barrel : NetworkBehaviour
{
    #region Serialized Fields
    [SerializeField] private bool isOffline = false;
    [SerializeField] private Jump jump;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BarrelInteractions barrelInteractions;
    #endregion
    #region Public Properties
    public BarrelInteractions BarrelInteractions { get { return barrelInteractions; } }
    #endregion
    #region Functions
    private void Update()
    {
        jump.HandleJump(false);
    }

    public void SetHorizontalSpeed(float speed)
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    public void Initialize(float speed)
    {
        rb.velocity = new Vector2(speed, 0);
        StartCoroutine(DisposeAfterTime(5f));
    }

    IEnumerator DisposeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (!isOffline)
            NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Barrel Online");
        else
            ObjectPool.Singleton.ReturnObject(gameObject, "Barrel");
    }
    #endregion
}
