using System.Collections;
using Unity.Netcode;
using UnityEngine;
public class Barrel : NetworkBehaviour
{
    #region Serialized Fields
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
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
    }

    public void Initialize(float speed)
    {
        rb.linearVelocity = new Vector2(speed, 0);
        StartCoroutine(DisposeAfterTime(5f));
    }

    IEnumerator DisposeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        GameNetwork.Instance.Despawn(NetworkObject, Constants.NETWORK_OBJECT_POOL_BARREL);
    }
    #endregion
}
